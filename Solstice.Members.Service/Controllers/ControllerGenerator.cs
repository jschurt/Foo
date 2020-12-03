using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Solstice.Infrastructure;

namespace Solstice.Members.Service.Controllers
{
	[Authorize]
	[Route("v1/Benefit")]
	public partial class Solstice_Members_IBenefitBusinessLogic : ApiController
	{
		private Solstice.Members.IBenefitBusinessLogic Api { get; }

		public Solstice_Members_IBenefitBusinessLogic(Solstice.Members.IBenefitBusinessLogic api)
		{
			Api = api;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetAllBenefitsAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public System.Boolean includeFamily { get; set; }
			public System.Boolean excludePremium { get; set; } = false;
		}

        [HttpPost("GetAllBenefits")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.BenefitInfo>> GetAllBenefitsAsync(GetAllBenefitsAsync_Parameters parameters)
        {
			return Api.GetAllBenefitsAsync(personId: parameters.personId, includeFamily: parameters.includeFamily, excludePremium: parameters.excludePremium);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetCurrentBenefitsAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public System.Boolean includeFamily { get; set; }
			public System.Boolean excludePremium { get; set; } = false;
		}

        [HttpPost("GetCurrentBenefits")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.BenefitInfo>> GetCurrentBenefitsAsync(GetCurrentBenefitsAsync_Parameters parameters)
        {
			return Api.GetCurrentBenefitsAsync(personId: parameters.personId, includeFamily: parameters.includeFamily, excludePremium: parameters.excludePremium);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetAllProductsAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public System.Boolean includeFamily { get; set; }
		}

        [HttpPost("GetAllProducts")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ProductInfo>> GetAllProductsAsync(GetAllProductsAsync_Parameters parameters)
        {
			return Api.GetAllProductsAsync(personId: parameters.personId, includeFamily: parameters.includeFamily);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetCurrentProductsAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public System.Boolean includeFamily { get; set; }
		}

        [HttpPost("GetCurrentProducts")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ProductInfo>> GetCurrentProductsAsync(GetCurrentProductsAsync_Parameters parameters)
        {
			return Api.GetCurrentProductsAsync(personId: parameters.personId, includeFamily: parameters.includeFamily);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetAllPlansAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public System.Boolean includeFamily { get; set; }
		}

        [HttpPost("GetAllPlans")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.PlanInfo>> GetAllPlansAsync(GetAllPlansAsync_Parameters parameters)
        {
			return Api.GetAllPlansAsync(personId: parameters.personId, includeFamily: parameters.includeFamily);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetCurrentPlansAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public System.Boolean includeFamily { get; set; }
		}

        [HttpPost("GetCurrentPlans")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.PlanInfo>> GetCurrentPlansAsync(GetCurrentPlansAsync_Parameters parameters)
        {
			return Api.GetCurrentPlansAsync(personId: parameters.personId, includeFamily: parameters.includeFamily);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetCurrentScheduleOfBenefitsAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetCurrentScheduleOfBenefits")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ScheduleOfBenefitsInfo>> GetCurrentScheduleOfBenefitsAsync(GetCurrentScheduleOfBenefitsAsync_Parameters parameters)
        {
			return Api.GetCurrentScheduleOfBenefitsAsync(personId: parameters.personId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class HasCurrentDavisVisionPlan_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("HasCurrentDavisVisionPlan")]
		public System.Threading.Tasks.Task<System.Boolean> HasCurrentDavisVisionPlan(HasCurrentDavisVisionPlan_Parameters parameters)
        {
			return Api.HasCurrentDavisVisionPlan(personId: parameters.personId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetCurrentValueAddonDocumentsAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetCurrentValueAddonDocuments")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ValueAddonInfo>> GetCurrentValueAddonDocumentsAsync(GetCurrentValueAddonDocumentsAsync_Parameters parameters)
        {
			return Api.GetCurrentValueAddonDocumentsAsync(personId: parameters.personId);
		}
	}

	[Authorize]
	[Route("v1/MemberAddress")]
	public partial class Solstice_Members_IMemberAddressBusinessLogic : ApiController
	{
		private Solstice.Members.IMemberAddressBusinessLogic Api { get; }

		public Solstice_Members_IMemberAddressBusinessLogic(Solstice.Members.IMemberAddressBusinessLogic api)
		{
			Api = api;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberAddressAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetMemberAddress")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberAddressModels.MemberAddressInfo> GetMemberAddressAsync(GetMemberAddressAsync_Parameters parameters)
        {
			return Api.GetMemberAddressAsync(personId: parameters.personId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetCoordinatesFromPrimaryAddressAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetCoordinatesFromPrimaryAddress")]
		public System.Threading.Tasks.Task<Solstice.Core.Models.GeographicCoordinates> GetCoordinatesFromPrimaryAddressAsync(GetCoordinatesFromPrimaryAddressAsync_Parameters parameters)
        {
			return Api.GetCoordinatesFromPrimaryAddressAsync(personId: parameters.personId);
		}
	}

	[Authorize]
	[Route("v1/Member")]
	public partial class Solstice_Members_IMemberBusinessLogic : ApiController
	{
		private Solstice.Members.IMemberBusinessLogic Api { get; }

		public Solstice_Members_IMemberBusinessLogic(Solstice.Members.IMemberBusinessLogic api)
		{
			Api = api;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetDependentsAsync_Parameters
		{
			public System.Int32 parentPersonId { get; set; }
		}

        [HttpPost("GetDependents")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.MemberModels.MemberInfo>> GetDependentsAsync(GetDependentsAsync_Parameters parameters)
        {
			return Api.GetDependentsAsync(parentPersonId: parameters.parentPersonId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetMember")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberModels.MemberInfo> GetMemberAsync(GetMemberAsync_Parameters parameters)
        {
			return Api.GetMemberAsync(personId: parameters.personId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberPersonIdAsync_Parameters
		{
			public System.Int32 userId { get; set; }
		}

        [HttpPost("GetMemberPersonId")]
		public System.Threading.Tasks.Task<System.Int32> GetMemberPersonIdAsync(GetMemberPersonIdAsync_Parameters parameters)
        {
			return Api.GetMemberPersonIdAsync(userId: parameters.userId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberGroupInfoAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [AllowAnonymous]
        [HttpPost("GetMemberGroupInfo")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberModels.MemberGroupInfo> GetMemberGroupInfoAsync(GetMemberGroupInfoAsync_Parameters parameters)
        {
			return Api.GetMemberGroupInfoAsync(personId: parameters.personId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberForUserRegistrationAsync_Parameters
		{
			public System.DateTime dateOfBirth { get; set; }
			public System.String zipCode { get; set; }
			public System.String memberNumber { get; set; }
			public System.String socialSecurityNumber { get; set; }
			public System.String partnerTemplateName { get; set; }
		}

        [AllowAnonymous]
        [HttpPost("GetMemberForUserRegistration")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberModels.MemberInfo> GetMemberForUserRegistrationAsync(GetMemberForUserRegistrationAsync_Parameters parameters)
        {
			return Api.GetMemberForUserRegistrationAsync(dateOfBirth: parameters.dateOfBirth, zipCode: parameters.zipCode, memberNumber: parameters.memberNumber, socialSecurityNumber: parameters.socialSecurityNumber, partnerTemplateName: parameters.partnerTemplateName);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetPartnerTemplateAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetPartnerTemplate")]
		public System.Threading.Tasks.Task<System.String> GetPartnerTemplateAsync(GetPartnerTemplateAsync_Parameters parameters)
        {
			return Api.GetPartnerTemplateAsync(personId: parameters.personId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetFamilyAsync_Parameters
		{
			public System.Int32 memberPersonId { get; set; }
		}

        [HttpPost("GetFamily")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.MemberModels.MemberInfo>> GetFamilyAsync(GetFamilyAsync_Parameters parameters)
        {
			return Api.GetFamilyAsync(memberPersonId: parameters.memberPersonId);
		}
	}

	[Authorize]
	[Route("v1/MemberContact")]
	public partial class Solstice_Members_IMemberContactBusinessLogic : ApiController
	{
		private Solstice.Members.IMemberContactBusinessLogic Api { get; }

		public Solstice_Members_IMemberContactBusinessLogic(Solstice.Members.IMemberContactBusinessLogic api)
		{
			Api = api;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberContactAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetMemberContact")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberContactModels.MemberContactInfo> GetMemberContactAsync(GetMemberContactAsync_Parameters parameters)
        {
			return Api.GetMemberContactAsync(personId: parameters.personId);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class UpdateMemberEmailAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public System.String emailAddress { get; set; }
		}

        [AllowAnonymous]
        [HttpPost("UpdateMemberEmail")]
		public System.Threading.Tasks.Task UpdateMemberEmailAsync(UpdateMemberEmailAsync_Parameters parameters)
        {
			return Api.UpdateMemberEmailAsync(personId: parameters.personId, emailAddress: parameters.emailAddress);
		}
	}

	[Authorize]
	[Route("v1/MemberDateOfLastCleaning")]
	public partial class Solstice_Members_IMemberDateOfLastCleaningBusinessLogic : ApiController
	{
		private Solstice.Members.IMemberDateOfLastCleaningBusinessLogic Api { get; }

		public Solstice_Members_IMemberDateOfLastCleaningBusinessLogic(Solstice.Members.IMemberDateOfLastCleaningBusinessLogic api)
		{
			Api = api;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberDateOfLastCleaningAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("GetMemberDateOfLastCleaning")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberDateOfLastCleaningModels.MemberDateOfLastCleaning> GetMemberDateOfLastCleaningAsync(GetMemberDateOfLastCleaningAsync_Parameters parameters)
        {
			return Api.GetMemberDateOfLastCleaningAsync(personId: parameters.personId);
		}
	}

	[Authorize]
	[Route("v1/MemberIdCard")]
	public partial class Solstice_Members_IMemberIdCardBusinessLogic : ApiController
	{
		private Solstice.Members.IMemberIdCardBusinessLogic Api { get; }

		public Solstice_Members_IMemberIdCardBusinessLogic(Solstice.Members.IMemberIdCardBusinessLogic api)
		{
			Api = api;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class GetMemberIdCardAsync_Parameters
		{
			public System.Int32 personId { get; set; }
			public Solstice.Members.MemberIdCardModels.IdCardFormat format { get; set; }
		}

        [HttpPost("GetMemberIdCard")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberIdCardModels.GetMemberIdCardResult> GetMemberIdCardAsync(GetMemberIdCardAsync_Parameters parameters)
        {
			return Api.GetMemberIdCardAsync(personId: parameters.personId, format: parameters.format);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class IsMemberIdCardEligibleAsync_Parameters
		{
			public System.Int32 personId { get; set; }
		}

        [HttpPost("IsMemberIdCardEligible")]
		public System.Threading.Tasks.Task<System.Boolean> IsMemberIdCardEligibleAsync(IsMemberIdCardEligibleAsync_Parameters parameters)
        {
			return Api.IsMemberIdCardEligibleAsync(personId: parameters.personId);
		}
	}

	[Authorize]
	[Route("v1/MemberVoe")]
	public partial class Solstice_Members_IMemberVoeBusinessLogic : ApiController
	{
		private Solstice.Members.IMemberVoeBusinessLogic Api { get; }

		public Solstice_Members_IMemberVoeBusinessLogic(Solstice.Members.IMemberVoeBusinessLogic api)
		{
			Api = api;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
		public class RequestVoeByFaxAsync_Parameters
		{
			public System.String memberNumber { get; set; }
			public System.String faxNumber { get; set; }
			public System.Int32 productId { get; set; }
			public System.Boolean inNetwork { get; set; }
		}

        [HttpPost("RequestVoeByFax")]
		public System.Threading.Tasks.Task<Solstice.Members.MemberVoeModels.VoeRequestResult> RequestVoeByFaxAsync(RequestVoeByFaxAsync_Parameters parameters)
        {
			return Api.RequestVoeByFaxAsync(memberNumber: parameters.memberNumber, faxNumber: parameters.faxNumber, productId: parameters.productId, inNetwork: parameters.inNetwork);
		}
	}

	[Authorize]
	[Route("v1/PassiveEnrollment")]
	public partial class Solstice_Members_IPassiveEnrollmentBusinessLogic : ApiController
	{
		private Solstice.Members.IPassiveEnrollmentBusinessLogic Api { get; }

		public Solstice_Members_IPassiveEnrollmentBusinessLogic(Solstice.Members.IPassiveEnrollmentBusinessLogic api)
		{
			Api = api;
		}

        [HttpPost("PassivelyEnrollAllEligibleMembers")]
		public System.Threading.Tasks.Task PassivelyEnrollAllEligibleMembersAsync()
        {
			return Api.PassivelyEnrollAllEligibleMembersAsync();
		}

        [HttpPost("GetMembersToPassivelyEnroll")]
		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.PassiveEnrollmentModels.PassiveEnrollmentMember>> GetMembersToPassivelyEnrollAsync()
        {
			return Api.GetMembersToPassivelyEnrollAsync();
		}
	}

	[Authorize]
	[Route("v1/WelcomePacket")]
	public partial class Solstice_Members_IWelcomePacketBusinessLogic : ApiController
	{
		private Solstice.Members.IWelcomePacketBusinessLogic Api { get; }

		public Solstice_Members_IWelcomePacketBusinessLogic(Solstice.Members.IWelcomePacketBusinessLogic api)
		{
			Api = api;
		}

        [AllowAnonymous]
        [HttpPost("GenerateWelcomePacketsInQueue")]
		public System.Threading.Tasks.Task GenerateWelcomePacketsInQueueAsync()
        {
			return Api.GenerateWelcomePacketsInQueueAsync();
		}

        [AllowAnonymous]
        [HttpPost("QueueWelcomePacketsForAllEligibleMembers")]
		public System.Threading.Tasks.Task QueueWelcomePacketsForAllEligibleMembersAsync()
        {
			return Api.QueueWelcomePacketsForAllEligibleMembersAsync();
		}
	}
}
