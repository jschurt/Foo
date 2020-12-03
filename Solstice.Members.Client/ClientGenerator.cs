using System.Net.Http;
using Solstice.Infrastructure;

namespace Solstice.Members
{
	internal partial class BenefitBusinessLogicServiceClient : ServiceClient, IBenefitBusinessLogic
	{
        public BenefitBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.BenefitInfo>> GetAllBenefitsAsync(System.Int32 personId, System.Boolean includeFamily, System.Boolean excludePremium = false)
        {
			var __parameters = new
			{
				personId,
				includeFamily,
				excludePremium,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.BenefitInfo>>("v1/Benefit/GetAllBenefits", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.BenefitInfo>> GetCurrentBenefitsAsync(System.Int32 personId, System.Boolean includeFamily, System.Boolean excludePremium = false)
        {
			var __parameters = new
			{
				personId,
				includeFamily,
				excludePremium,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.BenefitInfo>>("v1/Benefit/GetCurrentBenefits", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ProductInfo>> GetAllProductsAsync(System.Int32 personId, System.Boolean includeFamily)
        {
			var __parameters = new
			{
				personId,
				includeFamily,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.ProductInfo>>("v1/Benefit/GetAllProducts", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ProductInfo>> GetCurrentProductsAsync(System.Int32 personId, System.Boolean includeFamily)
        {
			var __parameters = new
			{
				personId,
				includeFamily,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.ProductInfo>>("v1/Benefit/GetCurrentProducts", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.PlanInfo>> GetAllPlansAsync(System.Int32 personId, System.Boolean includeFamily)
        {
			var __parameters = new
			{
				personId,
				includeFamily,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.PlanInfo>>("v1/Benefit/GetAllPlans", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.PlanInfo>> GetCurrentPlansAsync(System.Int32 personId, System.Boolean includeFamily)
        {
			var __parameters = new
			{
				personId,
				includeFamily,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.PlanInfo>>("v1/Benefit/GetCurrentPlans", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ScheduleOfBenefitsInfo>> GetCurrentScheduleOfBenefitsAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.ScheduleOfBenefitsInfo>>("v1/Benefit/GetCurrentScheduleOfBenefits", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Boolean> HasCurrentDavisVisionPlan(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<System.Boolean>("v1/Benefit/HasCurrentDavisVisionPlan", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.BenefitModels.ValueAddonInfo>> GetCurrentValueAddonDocumentsAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.BenefitModels.ValueAddonInfo>>("v1/Benefit/GetCurrentValueAddonDocuments", parameters: __parameters);
		}
	}

	internal partial class MemberAddressBusinessLogicServiceClient : ServiceClient, IMemberAddressBusinessLogic
	{
        public MemberAddressBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task<Solstice.Members.MemberAddressModels.MemberAddressInfo> GetMemberAddressAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<Solstice.Members.MemberAddressModels.MemberAddressInfo>("v1/MemberAddress/GetMemberAddress", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<Solstice.Core.Models.GeographicCoordinates> GetCoordinatesFromPrimaryAddressAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<Solstice.Core.Models.GeographicCoordinates>("v1/MemberAddress/GetCoordinatesFromPrimaryAddress", parameters: __parameters);
		}
	}

	internal partial class MemberBusinessLogicServiceClient : ServiceClient, IMemberBusinessLogic
	{
        public MemberBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.MemberModels.MemberInfo>> GetDependentsAsync(System.Int32 parentPersonId)
        {
			var __parameters = new
			{
				parentPersonId,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.MemberModels.MemberInfo>>("v1/Member/GetDependents", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<Solstice.Members.MemberModels.MemberInfo> GetMemberAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<Solstice.Members.MemberModels.MemberInfo>("v1/Member/GetMember", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Int32> GetMemberPersonIdAsync(System.Int32 userId)
        {
			var __parameters = new
			{
				userId,
			};

			return ExecuteAsync<System.Int32>("v1/Member/GetMemberPersonId", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<Solstice.Members.MemberModels.MemberGroupInfo> GetMemberGroupInfoAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<Solstice.Members.MemberModels.MemberGroupInfo>("v1/Member/GetMemberGroupInfo", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<Solstice.Members.MemberModels.MemberInfo> GetMemberForUserRegistrationAsync(System.DateTime dateOfBirth, System.String zipCode, System.String memberNumber, System.String socialSecurityNumber, System.String partnerTemplateName)
        {
			var __parameters = new
			{
				dateOfBirth,
				zipCode,
				memberNumber,
				socialSecurityNumber,
				partnerTemplateName,
			};

			return ExecuteAsync<Solstice.Members.MemberModels.MemberInfo>("v1/Member/GetMemberForUserRegistration", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.String> GetPartnerTemplateAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<System.String>("v1/Member/GetPartnerTemplate", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.MemberModels.MemberInfo>> GetFamilyAsync(System.Int32 memberPersonId)
        {
			var __parameters = new
			{
				memberPersonId,
			};

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.MemberModels.MemberInfo>>("v1/Member/GetFamily", parameters: __parameters);
		}
	}

	internal partial class MemberContactBusinessLogicServiceClient : ServiceClient, IMemberContactBusinessLogic
	{
        public MemberContactBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task<Solstice.Members.MemberContactModels.MemberContactInfo> GetMemberContactAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<Solstice.Members.MemberContactModels.MemberContactInfo>("v1/MemberContact/GetMemberContact", parameters: __parameters);
		}

		public System.Threading.Tasks.Task UpdateMemberEmailAsync(System.Int32 personId, System.String emailAddress)
        {
			var __parameters = new
			{
				personId,
				emailAddress,
			};

			return ExecuteAsync("v1/MemberContact/UpdateMemberEmail", parameters: __parameters);
		}
	}

	internal partial class MemberDateOfLastCleaningBusinessLogicServiceClient : ServiceClient, IMemberDateOfLastCleaningBusinessLogic
	{
        public MemberDateOfLastCleaningBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task<Solstice.Members.MemberDateOfLastCleaningModels.MemberDateOfLastCleaning> GetMemberDateOfLastCleaningAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<Solstice.Members.MemberDateOfLastCleaningModels.MemberDateOfLastCleaning>("v1/MemberDateOfLastCleaning/GetMemberDateOfLastCleaning", parameters: __parameters);
		}
	}

	internal partial class MemberIdCardBusinessLogicServiceClient : ServiceClient, IMemberIdCardBusinessLogic
	{
        public MemberIdCardBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task<Solstice.Members.MemberIdCardModels.GetMemberIdCardResult> GetMemberIdCardAsync(System.Int32 personId, Solstice.Members.MemberIdCardModels.IdCardFormat format)
        {
			var __parameters = new
			{
				personId,
				format,
			};

			return ExecuteAsync<Solstice.Members.MemberIdCardModels.GetMemberIdCardResult>("v1/MemberIdCard/GetMemberIdCard", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Boolean> IsMemberIdCardEligibleAsync(System.Int32 personId)
        {
			var __parameters = new
			{
				personId,
			};

			return ExecuteAsync<System.Boolean>("v1/MemberIdCard/IsMemberIdCardEligible", parameters: __parameters);
		}
	}

	internal partial class MemberVoeBusinessLogicServiceClient : ServiceClient, IMemberVoeBusinessLogic
	{
        public MemberVoeBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task<Solstice.Members.MemberVoeModels.VoeRequestResult> RequestVoeByFaxAsync(System.String memberNumber, System.String faxNumber, System.Int32 productId, System.Boolean inNetwork)
        {
			var __parameters = new
			{
				memberNumber,
				faxNumber,
				productId,
				inNetwork,
			};

			return ExecuteAsync<Solstice.Members.MemberVoeModels.VoeRequestResult>("v1/MemberVoe/RequestVoeByFax", parameters: __parameters);
		}
	}

	internal partial class PassiveEnrollmentBusinessLogicServiceClient : ServiceClient, IPassiveEnrollmentBusinessLogic
	{
        public PassiveEnrollmentBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task PassivelyEnrollAllEligibleMembersAsync()
        {
            object __parameters = null;

			return ExecuteAsync("v1/PassiveEnrollment/PassivelyEnrollAllEligibleMembers", parameters: __parameters);
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.List<Solstice.Members.PassiveEnrollmentModels.PassiveEnrollmentMember>> GetMembersToPassivelyEnrollAsync()
        {
            object __parameters = null;

			return ExecuteAsync<System.Collections.Generic.List<Solstice.Members.PassiveEnrollmentModels.PassiveEnrollmentMember>>("v1/PassiveEnrollment/GetMembersToPassivelyEnroll", parameters: __parameters);
		}
	}

	internal partial class WelcomePacketBusinessLogicServiceClient : ServiceClient, IWelcomePacketBusinessLogic
	{
        public WelcomePacketBusinessLogicServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

		public System.Threading.Tasks.Task GenerateWelcomePacketsInQueueAsync()
        {
            object __parameters = null;

			return ExecuteAsync("v1/WelcomePacket/GenerateWelcomePacketsInQueue", parameters: __parameters);
		}

		public System.Threading.Tasks.Task QueueWelcomePacketsForAllEligibleMembersAsync()
        {
            object __parameters = null;

			return ExecuteAsync("v1/WelcomePacket/QueueWelcomePacketsForAllEligibleMembers", parameters: __parameters);
		}
	}
}

