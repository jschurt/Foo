using System;
using System.Collections.Generic;
using System.Text;

namespace Solstice.Members.EnrollmentModels
{
    public static class LookupAttributes
    {
        public enum LookupType
        {
            BrokerSignup = 2,
            GroupStructure = 4,
            Phone = 6,
            Address = 7,
            ContactType = 8,
            PlanDefaultOption = 9,
            UserAgreementStatus = 20,
            GroupStatus = 3,
            GroupType = 12,
            EmployeeSignup = 13,
            PromotionType = 27,
            ChargeType = 28,
            PlanDetailType = 29,
            DataType = 30,
            EmploymentType = 22,
            PayCycle = 23,
            Frequency = 32,
            BenefitsType = 33,
            PlanTiers = 15,
            PlanCoverage = 16,
            FramesBenefitsType = 36,
            Relationship = 14,
            ConfirmStatus = 40,
            FormCategory = 42,
            EmailType = 43,
            LogType = 44,
            EmailStatusType = 45,
            RateAreaType = 46,
            RateZipType = 47,
            RateType = 49,
            BenefitsAdminRegistration = 50,
            PlanDuration = 51,
            FaceAmountType = 52,
            FaceAmountIncrement = 53,
            InvoiceDetailType = 54,
            ReminderType = 55,
            InvoiceAdjustmentType = 57,
            AdjustmentReasonCode = 58,
            EmployeeEnrollmentStatus = 61,
            EmployeeEnrollmentVerification = 62,
            InvoicePaymentStatus = 64,
            InvoicePaymentSteps = 65,
            ChargeStatusType = 66,
            InvoiceGroupType = 67,
            TerminationReason = 68,
            EliminationWaitingPeriodType = 69,
            BenefitChangeQualifyingEvents = 70,
            EventType = 71,
            EffectedActionType = 72,
            TableType = 73,
            ColumnType = 74,
            LifePlanOptionType = 75,
            EventUpdateMethodType = 76,
            BenefitBaseType = 77,
            RateBaseType = 78,
            InvoiceGPSStatus = 79,
            Application = 80,
            ApplicationSection = 81,
            InvoiceType = 82,
            AuthenticationAttemptResult = 85,
            Fee = 86,
            AgeBandOptionType = 87,
            ManualCalculationOptionType = 88,
            InsuranceAgeCalculationType = 89,
            ExchangeType = 1089,
            DepositAccountDeleteReason = 1090,
            EDIFrequency = 1091
        }

        public enum Gender
        {
            Male = 0,
            Female = 1
        }

        public enum BrokerSignup
        {
            PersonalProfile = 1,
            BusinessProfile = 2,
            UserAgreement = 3,
            VerifyCredentials = 4,
            RequiredDocuments = 5,
            BrokerAgreement = 6,
            AccountAuthorization = 7,
            AccountAccess = 8,
            Confirmation = 9,
            ConfirmDetails = 10,
            BrokerActivation = 39,
            PaymentConfirmation = 67
        }

        public enum BenefitsAdminRegistration
        {
            Profile = 1218,
            UserAgreement = 1219,
            CompanyInformation = 1220,
            CorporateStructure = 1221,
            AdministrativeContacts = 1222,
            ActivateAccount = 1223,
            Dashboard = 1224
        }

        public enum EmployeeEnrollment
        {
            SignIn = 157,
            ConfirmIdentity = 158,
            ChangePassword = 159,
            UserAgreement = 160,
            ProfileOverview = 161,
            ProfileVerification = 162,
            ProfileDependents = 163,
            WelcomeBenefits = 164,
            SelectPackage = 165,
            SelectMedical = 166,
            SelectDental = 167,
            SelectVision = 168,
            SelectLife = 169,
            HealthcareAccount = 170,
            BenefitsSelectionSummary = 171,
            BenefitsCostSummary = 172,
            SubmitBenefits = 173,
            SubscriberForm = 1204,
            PostEnrollmentBenefitsCostSummary = 1408
        }

        public enum PhoneType
        {
            Home = 15,
            Fax = 16,
            Business = 17,
            Cell = 18
        }

        public enum AddressType
        {
            Home = 19,
            Shipping = 20,
            Business = 21,
            Billing = 61,
            Alternate = 6433
        }

        public enum GroupStructure
        {
            Group = 11,
            Division = 12,
            Location = 13,
            Class = 14
        }

        public enum Role
        {
            Broker = 2,
            Employee = 4,
            PrimaryGroupBenefitsAdmin = 5,
            Internal_GlobalAdmin = 6,
            Internal_GroupAdmin = 7,
            Internal_GroupAdmin_Superuser = 8,
            //Internal_Sales = 9,
            Internal_SalesAccountExecutive = 9,
            Internal_Sales_Superuser = 10,
            Internal_MemberServices = 11,
            Internal_MemberServices_Superuser = 12,
            GroupBillingAdmin = 13,
            GroupEligibilityAdmin = 14,
            Internal_IT = 15,
            Internal_GroupAdmin_Del_Invoices = 16,
            Internal_SalesAccountManager = 17,
            Internal_CanAccessSolsticeEmployee = 18,
            Internal_BillingManagementAccess = 19,
            Internal_ManualAdjustmentAccess = 20,
            Internal_SystemAdjustmentOverride = 21,
            Internal_RefundAccess = 22,
            Internal_InvoiceVoidAdminAccess = 23,
            Internal_CanReallocateCharge = 24,
            Internal_GroupUploadAccess = 25,
            Internal_GroupEligibilityUploadAccess = 26
        }

        public enum DocumentType
        {
            LiabilityInsurance = 24,
            W9 = 25,
            License = 26,
            BrokerAgreement = 27,
            ScheduleOfBenefits = 141,
            PharmacyFormulary = 142,
            UnderwritingGuidelines = 1214,
            RatesTable = 1215
        }

        public enum WorkbenchDocumentType
        {
            ScheduleOfBenefits = 1,
            MemberCertificate = 2,
            Handbook = 3,
            PharmacyBenefits = 4,
            EnrollmentForm = 5,
            AdminKitDocs = 6,
            UserGuides = 7,
            ProviderDocs = 8,
            AgentDocs = 9,
            SarasotaReports = 10,
            SolsticePPOCertificate = 11,
            CertificateOfCoverage = 12,
            Forms = 13,
            HealthInformation = 14,
            Newsletters = 15,
            HealthInfoPlanSpecific = 16,
            Other = 17,
            BrokerNewsletter = 18,
            ProviderNewsletter = 19,
            ProviderScheduleOfBenefits = 20
        }

        public enum UserAgreementStatus
        {
            NotAccepted = 22,
            Accepted = 23
        }

        public enum ReminderType
        {
            EnrollmentCloseDate = 1253
        }

        public enum GroupStatus
        {
            Lead = 42,
            Prospect = 43,
            OpenEnrollment = 44,
            Active = 45,
            Renewal = 46,
            Inactive = 47,
            ReadyForOpenEnrollment = 1165
        }

        public enum GroupType
        {
            Employee = 48,
            Individual = 51,
            EmployeeMarketplace = 1227,
            IndividualMarketplace = 1228,
            SuspenseAccount = -1 // TODO: Redefine Attribute
        }

        public enum PayCycle
        {
            Weekly = 52,
            BiWeekly = 53,
            SemiMonthly = 54,
            Monthly = 55,
            Quaterly = 56,
            SemiAnnually = 57,
            Annually = 58
        }

        public enum InsuranceAgeCalculationType
        {
            Birthdate = 2416,
            PlanYear = 2417
        }

        public enum PromotionType
        {
            BrokerSignup = 28,
            BrokerActivationDiscount = 29
        }

        public enum EmployeeType
        {
            FullTime = 59,
            PartTime = 60,
            Terminated = 6455,
            Retired = 6456
        }

        public enum ContactType
        {
            General = 65,
            Underwriter = 66,
            Internal = 1226,
            Marketer = 7468
        }

        public enum PlanDetailType
        {
            InNetwork = 68,
            OutOfNetwork = 69,
            OutOfNetwork2 = 70,
            InNet = 71,
            OutNet = 72,
            Condition1 = 73,
            Condition2 = 74,
            InNetworkWaitingPeriod = 75,
            OutOfNetworkWaitingPeriod = 76,
            Class1 = 77,
            Class2 = 78,
            Class3 = 79,
            Reimbursement = 80,
            CalendarYear = 96,
            PlanYear = 97,
            Mac = 98,
            Ucr = 99,
            DeductibleWaivedInNetwork = 100,
            DeductibleWaivedOutOfNetwork = 101,
            AdultChild = 102,
            ChildOnly = 103,
            Retail = 139,
            MailOrder = 140,
            Deductible = 1212
        }

        public enum DataType
        {
            Percentage = 81,
            Dollar = 83,
            String = 84
        }

        public enum BenefitsType
        {
            Reimbursement = 89,
            Copay = 90,
            Discount = 91,
            NotCovered = 92,
            PaidinFull = 138,
            Allowance = 146
        }

        public enum FramesBenefitsType
        {
            Copay = 133,
            Allowance = 134
        }

        public enum CustomField
        {
            DeductibleWaived = 104,
            OrthodantiaCondition = 105,
            UcrPercentile = 106,
            Frequency = 143,
            AllBenefitTypes = 144,
            BenefitTypesWithAllowance = 147,
            EnableOutOfNetwork = 1195
        }

        public enum PlanProductType
        {
            Medical = 1,
            Dental = 2,
            Life = 3,
            Vision = 4,
            HSA = 11,
            HRA = 12,
            FSA = 13,
            Cafeteria = 14,
            Packaged = 16
        }

        public enum ContributionOptionType
        {
            EmployerPaid = 1355,
            Voluntary = 1356
        }

        public enum WalletMethod
        {
            CREDITCARD = 1,
            ECHECK = 2,
            Promo = 3,
            PHYSICALCHECK = 4,
            SUSPENSE_ACCOUNT = 5
        }

        public enum ConfirmStatus
        {
            Selected = 148,
            Declined = 151
        }

        public enum Relationship
        {
            Spouse = 154,
            Child = 155,
            Employee = 1354,
            CustodialParent = 6429,
            Other = 6432
        }

        public enum FormCategory
        {
            Employer = 1156,
            Employee = 1157,
            Waiver = 1158,
            HIPAANotice = 1159,
            General = 1160
        }

        public enum EmailType
        {
            // ****************************************
            //  NOTE - PLEASE ADD NEW EMAILTYPES BELOW IN ALPHABETICAL ORDER TO HELP WITH ORGANIZATION
            // ****************************************
            BenefitsAdministratorWelcomeEmail = 1225,
            BenefitsAdministratorResetPassword = 7459,
            BenefitsAdminsGroupTerminated = 1235,
            BenefitsAdminsInvoiceNotPaid10Days = 1233,
            BenefitsAdminsInvoiceNotPaid1Day = 1232,
            BenefitsAdminsInvoiceReady = 1231,
            BenefitsAdminsPaymentReceived = 1237,
            BenefitsAdminsPaymentScheduled = 1236,
            BrokerEnrollmentEnded = 1229,
            BrokerGroupTerminated = 1234,
            BrokerLoginCredentials = 1196,
            BrokerOpenEnrollmentStart = 1161,
            BrokerQuote = 8467,
            ChangePasswordConfirmationEmail = 1201,
            ForgottenPassword = 1200,
            ForgottenUsername = 1199,
            InvoicePaymentConfirmation = 1298,
            NotificationMCS = 1197,
            ODMDocuments = 1203,
            PrimaryAdminEnrollmentEnded = 1230,
            RequestAuthorizationCodeEmail = 1202,
            SubscriberDeclineNotification = 1198,
            SubscriberEnrollmentComplete = 1164,
            SubscriberOpenEnrollmentReminder = 1163,
            SubscriberOpenEnrollmentStart = 1162,
            SendNotificationMCSBrokerEnrollment = 4424,
            IndividualInvoiceReady = 5422,
            IndividualWelcome = 5423,
            IndividualPaymentReceipt = 5424,
            IndividualPaymentDecline = 5425,
            GroupApplicationSubmittedEmail = 8466,
            GroupTerminationEmail = 7458,
            IndividualInvoiceCopy = 7460,
            TemporaryIdCardRequest = 7465
        }

        public enum LogType
        {
            Error = 1166,
            Info = 1167
        }

        public enum RateType
        {
            AgeBand = 1217,
            Tier = 1348,
            AgeBandTier = 1349,
            ManualCalculation = 1360,
            TierComposite = 6448,
            Tier2 = 6449,
            Tier3 = 6450,
            Tier4 = 6451,
            Tier4a = 6452 // DEPRECATED FOR NOW - 12/22 - Tiers on PlanRates 
        }

        public enum RateAreaType
        {
            Msa = 1205,
            ZipCode = 1206,
            County = 1207,
            Combination = 1208
        }

        public enum RateZipType
        {
            Group = 1209,
            Employee = 1210,
            Individual = 1211
        }

        public enum PlanCoverage
        {
            EmployeeOnly = 112,
            EmployeeSpouse = 113,
            Employee1Dependent = 114,
            EmployeeChildren = 115,
            EmployeeFamily = 116,
            Employee2Dependents = 6453,
            Employee3moreDependents = 6454,
            EmployeeChild = 6459,
        }

        public enum PlanTiers
        {
            Composite = 107,
            Tier2 = 108,
            Tier3 = 110,
            Tier4 = 111,
            Tier4a = 6457,
            Tier5 = 6458
        }

        public enum PlanDuration
        {
            Whole = 1238,
            Term = 1239
        }

        public enum FaceAmountType
        {
            FlatAmount = 1240,
            Salary = 1241,
            EnrolledAmount = 1242
        }

        public enum EmailStatusType
        {
            Sent = 1168,
            Failed = 1169,
            Failed_SmtpStatusCode_BadCommandSequence = 1170,
            Failed_SmtpStatusCode_CannotVerifyUserWillAttemptDelivery = 1171,
            Failed_SmtpStatusCode_ClientNotPermitted = 1172,
            Failed_SmtpStatusCode_CommandNotImplemented = 1173,
            Failed_SmtpStatusCode_CommandParameterNotImplemented = 1174,
            Failed_SmtpStatusCode_CommandUnrecognized = 1175,
            Failed_SmtpStatusCode_ExceededStorageAllocation = 1176,
            Failed_SmtpStatusCode_GeneralFailure = 1177,
            Failed_SmtpStatusCode_HelpMessage = 1178,
            Failed_SmtpStatusCode_InsufficientStorage = 1179,
            Failed_SmtpStatusCode_LocalErrorInProcessing = 1180,
            Failed_SmtpStatusCode_MailboxBusy = 1181,
            Failed_SmtpStatusCode_MailboxNameNotAllowed = 1182,
            Failed_SmtpStatusCode_MailboxUnavailable = 1183,
            Failed_SmtpStatusCode_MustIssueStartTlsFirst = 1184,
            Failed_SmtpStatusCode_Ok = 1185,
            Failed_SmtpStatusCode_ServiceClosingTransmissionChannel = 1186,
            Failed_SmtpStatusCode_ServiceNotAvailable = 1187,
            Failed_SmtpStatusCode_ServiceReady = 1188,
            Failed_SmtpStatusCode_StartMailInput = 1189,
            Failed_SmtpStatusCode_SyntaxError = 1190,
            Failed_SmtpStatusCode_SystemStatus = 1191,
            Failed_SmtpStatusCode_TransactionFailed = 1192,
            Failed_SmtpStatusCode_UserNotLocalTryAlternatePath = 1193,
            Failed_SmtpStatusCode_UserNotLocalWillForward = 1194,
            Failed_BadEmailSyntax = 1247
        }

        public enum InvoiceDetailType
        {
            Administration_Fee = 1252,
            Other = 1271,
            Fees = 5426,
            CarryoverBalance = 6428
        }

        public enum InvoiceAdjustmentType
        {
            General_Adjustments = 1258,
            Additions = 1259,
            Terminations = 1260,
            Rate_Change = 1261,
            TerminationsRollback = 1340,
            PEPMAdjustment = 1413,
            RetroPlanAdd = 1373,
            QualifyingEvent = 1299,
            Employee = 1417,
            ServiceFee = 1416,
            Group = 6435,
            Plan = 6460
        }

        public enum AdjustmentReasonCode
        {
            RemovedEmpCoverage = 1263,
            RemovedDepCoverage = 1264,
            AddedEmpCoverage = 1265,
            AddedDepCoverage = 1266,
            GeneralAdjustment = 1405,
            AchReturned = 6436,
            BankCharges = 6437,
            BillingError = 6438,
            CcChargeback = 6439,
            GroupTerm = 6440,
            RateCorrection = 6441,
            Refund = 6442,
            RetroEmployee = 6443,
            RetroDependent = 6444,
            RetroPlanChange = 6445,
            SelfBillAdjustment = 6446,
            WriteOffBadDebt = 6447
        }

        public enum InvoicePaymentStatus
        {
            Unpaid = 1286,
            Partial = 1287,
            Paid = 1288,
            NotApplicable = 7467
        }

        public enum InvoiceGroupType
        {
            Basic = 1248,
            ADAndD = 1250,
            Accident = 1251,
            EmployerPaidLife = 1254,
            VoluntaryLife = 1255,
            STD = 1256,
            LTD = 1257,
            Supplemental_Life = 1267,
            CriticalIllness = 1268,
            Wellness = 1269,
            Gap = 1270,
            Whole_Life = 1298,
            Cancer = 1300,
            HospitalIndemnity = 1301,
            Universal_Life = 4420
        }

        public enum TerminationReason
        {
            Voluntary = 1352,
            Involuntary = 1353
        }

        public enum EmployeeEnrollmentStatus
        {
            NotStarted = 1278,
            InProgress = 1279,
            Submitted = 1280,
            DeclinedBenefits = 1281
        }

        public enum EmployeeEnrollmentVerification
        {
            Verified = 1282,
            Unverified = 1283
        }

        public enum InvoicePaymentSteps
        {
            // Version 2 steps
            Overview = 1090,
            PaymentInformationV2 = 1091,
            PaymentVerificationV2 = 1092,
            PaymentConfirmationV2 = 1093,

            GlobalAdmin_BillingOverview = 1289,
            GlobalAdmin_PaymentInformation = 1290,
            GlobalAdmin_PaymentVerification = 1291,
            GlobalAdmin_PaymentConfirmation = 1292
        }

        public enum ChargeType
        {
            Ach = 30,
            CreditCard = 31
        }

        public enum ChargeStatusType
        {
            pending_settlement = 1294,
            settlement_complete = 1295,
            pending_refund = 1296,
            refund_complete = 1297,
            echeck_hold = 1409,
            Declined = 5427
        }

        public enum EliminationWaitingPeriodType
        {
            Injury = 1302,
            Sickness = 1303,
            CatastrophicAccidental = 1304,
            Pregnancy = 1305,
            Other = 1306
        }

        public enum QualifyingLifeEventReason
        {
            Marriage = 1307,
            DeclarationOfDomesticPartner = 1308,
            Birth = 1309,
            Adoption = 1310,
            EmployeeReturnsFromAbsence = 1311,
            Divorce = 1312,
            TerminationOfDomesticPartner = 1313,
            LegalSeparation = 1314,
            PlacementForAdoption = 1315,
            DependentsAgeEligibilityChange = 1316,
            DependentsBenefitsEligibilityChange = 1317,
            EmployeeTakesAbsence = 1318,
            DependentsChangeOfResidence = 1319,
            DeathOfDependent = 1320,
            ChangeOfEmployeeType = 1321,
            ChangeOfEmployeesWorkLocation = 1322,
            ChangeOfEmployeesResidence = 1323,
            SignificantCostIncreaseDecrease = 1324,
            EmployeeTermination = 1325,
            ErrorCorrection = 1395,
            OpenEnrollment = 7469
        }

        public enum EventType
        {
            Termination = 1326,
            TerminationRollback = 1327,
            EmployeeBenefitsChange = 1328,
            Enrollment = 1378,
            GroupStructureChange = 6461,
            GroupRenewal = 6462,
            GroupTermination = 7466
        }

        public enum EffectedActionType
        {
            Added = 1329,
            Updated = 1330,
            Enabled = 1331,
            Disabled = 1332,
            Archived = 1366,
            Removed = 1371
        }

        public enum TableType
        {
            GroupEmployee = 1333,
            GroupPlanSubscriber = 1334,
            GroupPlanPerson = 1335,
            Adjustment = 1336,
            Person = 1372,
            InvoiceGroupPlanSubscriber = 1396,
        }

        public enum ColumnType
        {
            BenefitsExpireDate = 1337,
            TerminationDate = 1338,
            StatusReason = 1339,
            GroupEmployeeID = 1341,
            GroupPlanSubscriberID = 1342,
            PersonID = 1343,
            AdjustmentID = 1344,
            IsCobraEligible = 1345,
            IsTerminated = 1346,
            TerminateReason = 1347,
            BenefitsEffectiveDate = 1359,
            ArchiveBenefitsExpireDate = 1370,
            InvoiceGroupPlanSubscriberID = 1397,
        }

        public enum EventUpdateMethodType
        {
            EmployeeUpdate = 1357,
            GroupAdministratorUpdate = 1358,
            MemberServiceUpdate = 7460,
            UploadEligibility = 8473
        }

        public enum BenefitBaseType
        {
            Flat = 1361,
            EmployeeSalary = 1362,
            EmployeeBenefit = 1363
        }

        public enum RateBaseType
        {
            Multiplier = 1364,
            Whole = 1365
        }

        public enum InvoiceGPSStatus
        {
            Initial = 1367,     // GPS was on the invoice at the time of generation
            Added = 1368,       // CHARGE Adjustment was made for this GPS subsequent to invoice generation
            Removed = 1369      // CREDIT Adjustment was made for this GPS subsequent to invoice generation
        }

        public enum Application
        {
            Marketplace = 1379,
            Workbench = 1380
        }

        public enum ApplicationSection
        {
            Account = 1381,
            BrokerEnrollment = 1382,
            BrokerAdmin = 1383,
            GroupEnrollment = 1384,
            BenefitsAdminRegistration = 1385,
            BenefitsAdminBilling = 1386,
            BenefitsAdmin = 1387,
            GroupAdmin = 1388,
            GlobalAdmin = 1403
        }

        public enum EmployeeStatus
        {
            Inactive = 0,
            Active = 1,
            New = 2, // should only be set to this when employee is added by benefits admin and we will then be checking for needed adjustments after their OE is complete
            Declined = 3,
        }

        public enum InvoiceType
        {
            MainWithNoSubInvoices = 1392,
            MainWithSubInvoices = 1393,
            Sub = 1394,
            PaymentToCarrier = 1414,
            IndividualInvoice = 4421,
            GSIndividualInvoice = 6434
        }

        public enum AuthenticationAttemptResult
        {
            UnknownUsername = 1399,
            LockedAccount = 1400,
            IncorrectPassword = 1401,
            Success = 1402,
            PasswordReset = 4419
        }

        public enum Fees
        {
            PSPMFee = 1410
        }

        public enum AgeBandOptionType
        {
            Employee = 1411,
            Dependent = 1412
        }

        public enum ManualCalculationOptionType
        {
            Employee = 1415
        }

        public enum ExchangeType
        {
            FederalExchange = 6426,
            NYExchange = 6427
        }

        public enum DepositAccountDeleteReason
        {
            IncorrectAccount = 7461,
            NewAccount = 7462,
            UpdatedAccount = 7463,
            ClosedAccount = 7464,
        }

        public enum EDIFrequency
        {
            Daily = 8468,
            TwiceAWeek = 8469,
            Weekly = 8470,
            BiWeekly = 8471,
            Monthly = 8472
        }

        public enum PassivelyEnrolledMemberStatusKey
        {
            Awaiting_Approval = 1,
            Approved = 2,
            Terminated = 3
        }
    }
}
