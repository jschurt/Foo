using Microsoft.EntityFrameworkCore;
using Solstice.DAL;
using Solstice.DAL.Marketplace;
using Solstice.DAL.Marketplace.Models;
using Solstice.Members.MemberAddressModels;
using Solstice.Products.WelcomePacketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Solstice.Members.EnrollmentModels.LookupAttributes;

namespace Solstice.Members
{
    sealed class WelcomePacketBusinessLogic : IWelcomePacketBusinessLogic
    {
        private const int WELCOME_PACKET_BILLINGID_THRESHOLD___THE_ONE_THAT_MATTERS = 7;
        private readonly IMarketplaceDbContext _marketplaceDbContext;
        private readonly IMemberAddressBusinessLogic _memberAddressBusinessLogic;
        private readonly IMemberBusinessLogic _memberBusinessLogic;
        private readonly IBenefitBusinessLogic _benefitBusinessLogic;
        private readonly Products.IWelcomePacketBusinessLogic _productsWelcomePacketBusinessLogic;
        

        public WelcomePacketBusinessLogic(
            IMarketplaceDbContext marketplaceDbContext,
            IMemberAddressBusinessLogic memberAddressBusinessLogic,
            IMemberBusinessLogic memberBusinessLogic,
            IBenefitBusinessLogic benefitBusinessLogic,
            Products.IWelcomePacketBusinessLogic productsWelcomePacketBusinessLogic
            )
        {
            _marketplaceDbContext = marketplaceDbContext;
            _memberAddressBusinessLogic = memberAddressBusinessLogic;
            _memberBusinessLogic = memberBusinessLogic;
            _benefitBusinessLogic = benefitBusinessLogic;
            _productsWelcomePacketBusinessLogic = productsWelcomePacketBusinessLogic;
        }

        public async Task GenerateWelcomePacketsInQueueAsync()
        {
            //* Retrieve unprocessed CustomerMessage records for WelcomeLetter using the PhysicalMail channel
            var queuedWelcomePackets = await _marketplaceDbContext.CustomerMessage
                .Where(cm =>
                   cm.MessageChannelID == (int)MessageChannelKey.PhysicalMail &&
                   cm.MessageTemplateID == (int)MessageTemplateKey.CustomersIndividualEssentialSmileWelcomeLetter &&
                   !cm.IsProcessed
                ).ToListAsync();

            Exception error = null;

            //* Call GenerateWelcomePacket(personId) per pending record
            foreach (var pendingPacket in queuedWelcomePackets)
            {
                try
                {
                    var recepientData = await GenerateWelcomePacketAsync((int)pendingPacket.PersonID);
                    
                    //* Mark record processed if response 200 and update RecipientDataJson with address used
                    pendingPacket.ProcessedDate = DateTime.Now;
                    pendingPacket.IsProcessed = true;
                    pendingPacket.RecipientDataJson = recepientData.PrimaryAddress.ToJson();

                    await _marketplaceDbContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    error = ex;
                }
            }

            if (error != null)
            {
                throw new Exception("Error occurred while Generating Welcome Packets.", error);
            }
        }

        private async Task<MemberAddressInfo> GenerateWelcomePacketAsync(int personId)
        {
            // * Get member address, and plan, and SubscriberFirstName, SubscriberLastName
            var memberAddress = await _memberAddressBusinessLogic.GetMemberAddressAsync(personId);
            var memberInfo =  await _memberBusinessLogic.GetMemberAsync(personId);
            var memberBenefits = (await _benefitBusinessLogic.GetAllBenefitsAsync(personId, false))
                .OrderBy(b => b.StartDate)
                .Last();

            // * After you have the the member information you call the method in the Product service that will create and store the
            await _productsWelcomePacketBusinessLogic.GenerateWelcomePacketAsync(
                memberBenefits.Plan.PlanNumber,
                memberBenefits.StartDate,
                new SubscriberName()
                {
                    FirstName = memberInfo.FirstName,
                    LastName = memberInfo.LastName
                },
                new SubscriberAddress()
                {
                    AddressLine1 = memberAddress.PrimaryAddress.Line1,
                    AddressLine2 = memberAddress.PrimaryAddress.Line2,
                    CityName = memberAddress.PrimaryAddress.CityName,
                    StateCode = memberAddress.PrimaryAddress.StateAbbreviation,
                    ZIPCode = memberAddress.PrimaryAddress.ZipCode
                });

            return memberAddress;
        }

        public async Task QueueWelcomePacketsForAllEligibleMembersAsync()
        {
            // 1.Retrieving all members(GroupEmployeeID, PersonID) eligible for a Welcome Packet(based on invoice, elig, etc...) via EF query
            var eligibleMembers = await GetMembersEligibleForWelcomePacketsAsync();

            var customerMessages = eligibleMembers.Select(em =>
                new CustomerMessage()
                {
                    ActiveStatus = 1,
                    GroupID = em.GroupID,
                    IsProcessed = false,
                    MessageChannelID = (int)MessageChannelKey.PhysicalMail,
                    MessageTemplateID = (int)MessageTemplateKey.CustomersIndividualEssentialSmileWelcomeLetter,
                    PersonID = em.PersonID
                }
            );

            // 2.Members eligible, and not yet accounted for the in the CustomerMessage table, are inserted('queued') with subscriber GroupID and PersonID
            _marketplaceDbContext.CustomerMessage.AddRange(customerMessages);
            await _marketplaceDbContext.SaveChangesAsync();            
        }

        private async Task<List<GroupPerson>> GetMembersEligibleForWelcomePacketsAsync()
        {
            var invoiceBaseQuery =
                from i in _marketplaceDbContext.Invoice.AllActiveLegacy()
                where
                    i.BillingPeriodID >= WELCOME_PACKET_BILLINGID_THRESHOLD___THE_ONE_THAT_MATTERS && 
                    i.PaymentStatus == (int)InvoicePaymentStatus.Paid
                group i by i.AccountID into groupByAccount
                select new
                {
                    AccountID = groupByAccount.Key,
                    InvoiceID = groupByAccount.Min(c => c.InvoiceID)
                };

            var memberBaseQuery =
                from ib in invoiceBaseQuery
                join a in _marketplaceDbContext.Account.AllActiveLegacy() on ib.AccountID equals a.AccountID
                join g in _marketplaceDbContext.Group.AllActiveLegacy() on a.GroupID equals g.GroupID
                join ge in _marketplaceDbContext.GroupEmployee.AllActiveLegacy() on g.GroupID equals ge.GroupID
                join i in _marketplaceDbContext.Invoice.AllActiveLegacy() on ib.InvoiceID equals i.InvoiceID
                join p in _marketplaceDbContext.Person.AllActiveLegacy() on ge.PersonID equals p.PersonID
                join ex in _marketplaceDbContext.OnExchangeEnrollment.AllActiveLegacy() on ge.GroupEmployeeID equals ex.GroupEmployeeID
                where
                    p.Email == null
                select new
                {
                    InvoiceID = ib.InvoiceID,
                    AccountID = a.AccountID,
                    GroupID = g.GroupID,
                    PersonID = ge.PersonID,
                    GroupEmployeeID = ge.GroupEmployeeID,
                    BillingPeriodID = (int)i.BillingPeriodID,
                    InvoicePeriodStartDate = i.PeriodStartDate
                };

            var memberCoverage =
                from mb in memberBaseQuery
                where
                     _marketplaceDbContext.vGroupPlanSubscriberNormalized.AllActiveLegacy().Any(gps =>
                    gps.ConfirmStatus == (int)ConfirmStatus.Selected &&
                    gps.GroupEmployeeID == mb.GroupEmployeeID &&
                    mb.InvoicePeriodStartDate >= gps.BenefitEffectiveDate &&
                    mb.InvoicePeriodStartDate <= gps.BenefitExpireDate)
                select mb;

            var membersNotQueued =
                from mc in memberCoverage
                where !_marketplaceDbContext.CustomerMessage.AllActiveLegacy().Any(cm =>
                   cm.PersonID == mc.PersonID &&
                   cm.MessageChannelID == (int)MessageChannelKey.PhysicalMail &&
                   cm.MessageTemplateID == (int)MessageTemplateKey.CustomersIndividualEssentialSmileWelcomeLetter)
                select mc;

            var notPassivelyEnrolled =
                from mnq in membersNotQueued
                where !_marketplaceDbContext.PassivelyEnrolledMember.Any(pem =>
                    pem.BillingPeriodID == mnq.BillingPeriodID &&
                    pem.GroupEmployeeID == mnq.GroupEmployeeID)
                select mnq;

            return await notPassivelyEnrolled.Select(npe => new GroupPerson()
            {
                GroupID = (int)npe.GroupID,
                PersonID = (int)npe.PersonID
            }).ToListAsync();
        }

        private class GroupPerson
        {
            public int GroupID { get; set; }
            public int PersonID { get; set; }
        }
    }
}
