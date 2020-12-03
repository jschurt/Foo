using Microsoft.EntityFrameworkCore;
using Solstice.Core.Models;
using Solstice.DAL.Marketplace;
using Solstice.DAL.Workbench;
using Solstice.Members.MemberAddressModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.Members
{
    sealed class MemberAddressBusinessLogic : IMemberAddressBusinessLogic
    {
        private readonly IMarketplaceDbContext _db;
        private readonly IWorkbenchDbContext _workbenchDbContext;

        public MemberAddressBusinessLogic(IMarketplaceDbContext marketplaceDbContext, IWorkbenchDbContext workbenchDbContext)
        {
            _db = marketplaceDbContext;
            _workbenchDbContext = workbenchDbContext;
        }

        public async Task<MemberAddressInfo> GetMemberAddressAsync(int personId)
        {
            var primaryAddressQuery =
                from mad in _db.Members_vw_MemberAddress
                where
                    mad.PersonID == personId &&
                    mad.IsCurrent == true
                select new StreetAddress
                {
                    Line1 = mad.AddressLine1,
                    Line2 = mad.AddressLine2,
                    CityName = mad.City,
                    StateId = mad.StateID,
                    StateName = mad.StateName,
                    StateAbbreviation = mad.StateAbbreviation,
                    ZipCode = mad.ZipCode,
                    ZipCodeExtension = mad.ZipCodeExtension,
                    CountyId = mad.CountyID.GetValueOrDefault(),
                    CountyName = mad.CountyName,
                    CountryId = mad.CountryID.GetValueOrDefault(),
                    CountryName = mad.CountryName
                };

            var addressResult = await primaryAddressQuery.SingleOrDefaultAsync();

            var result = new MemberAddressInfo
            {
                PersonId = personId,
                PrimaryAddress = addressResult
            };

            return result;
        }

        public async Task<GeographicCoordinates> GetCoordinatesFromPrimaryAddressAsync(int personId)
        {
            var memberZipCode =
               await (from mad in _db.Members_vw_MemberAddress
                      where
                          mad.PersonID == personId &&
                          mad.IsCurrent == true
                      select
                         mad.ZipCode
                 ).SingleOrDefaultAsync();

            if (memberZipCode.HasValue())
            {
                var memberZipCodeInt = Convert.ToInt32(memberZipCode);
                var memberLatLon =
                    await (from UsZipcode in _workbenchDbContext.USZipCodes
                           where UsZipcode.zip == memberZipCodeInt
                           select new GeographicCoordinates
                           {
                               Latitude = Convert.ToDecimal(UsZipcode.lat),
                               Longitude = Convert.ToDecimal(UsZipcode.lon)
                           }
                        ).SingleOrDefaultAsync();

                return memberLatLon;
            }

            return null;
        }
    }
}
