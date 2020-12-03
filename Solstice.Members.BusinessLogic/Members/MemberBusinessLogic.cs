using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Solstice.Core.Models;
using Solstice.DAL.Marketplace;
using Solstice.DAL.MySolstice;
using Solstice.Members.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Solstice.Members
{
    sealed class MemberBusinessLogic : IMemberBusinessLogic
    {
        private class SecurityExtensions
        {
            private readonly string CryptoPassword;
            private readonly string CryptoSalt;
            private readonly string CryptoIV;
            private readonly byte[] KeyBytes;

            public SecurityExtensions(IConfiguration configuration)
            {
                var crypto = configuration.GetSection("Settings:Members:Member:SecurityExtensions");

                if (crypto != null)
                {
                    CryptoPassword = crypto["Password"];
                    CryptoSalt = crypto["Salt"];
                    CryptoIV = crypto["IV"];
                    KeyBytes = new Rfc2898DeriveBytes(CryptoPassword, Encoding.ASCII.GetBytes(CryptoSalt)).GetBytes(256 / 8);
                }
            }

            public string GetSalt(string str)
            {
                var _saltLength = Convert.ToInt32(str.Substring(str.LastIndexOf("??") + 2));
                return str.Substring(str.Length - _saltLength - 4, _saltLength);
            }

            public string Encrypt(string plainText)
            {
                ValidateCryptoKeys();

                var _plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                var _symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                var _encryptor = _symmetricKey.CreateEncryptor(KeyBytes, Encoding.ASCII.GetBytes(CryptoIV));

                byte[] _cipherTextBytes;

                using (var _memoryStream = new System.IO.MemoryStream())
                {
                    using (var _cryptoStream = new CryptoStream(_memoryStream, _encryptor, CryptoStreamMode.Write))
                    {
                        _cryptoStream.Write(_plainTextBytes, 0, _plainTextBytes.Length);
                        _cryptoStream.FlushFinalBlock();
                        _cipherTextBytes = _memoryStream.ToArray();
                        _cryptoStream.Close();
                    }
                    _memoryStream.Close();
                }
                return Convert.ToBase64String(_cipherTextBytes);
            }

            private void ValidateCryptoKeys()
            {
                if (CryptoPassword == null || CryptoSalt == null || CryptoIV == null)
                    throw new Exception("The marketplace.crypto configuration section is missing.");
            }

            public string Decrypt(string encryptedText)
            {
                if (string.IsNullOrWhiteSpace(encryptedText))
                    return "";

                ValidateCryptoKeys();

                byte[] _cipherTextBytes;
                try
                {
                    _cipherTextBytes = Convert.FromBase64String(encryptedText);

                    var _symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
                    var _decryptor = _symmetricKey.CreateDecryptor(KeyBytes, Encoding.ASCII.GetBytes(CryptoIV));
                    using (var _memoryStream = new System.IO.MemoryStream(_cipherTextBytes))
                    using (var _cryptoStream = new CryptoStream(_memoryStream, _decryptor, CryptoStreamMode.Read))
                    {
                        var _plainTextBytes = new byte[_cipherTextBytes.Length];
                        var _decryptedByteCount = _cryptoStream.Read(_plainTextBytes, 0, _plainTextBytes.Length);
                        return Encoding.UTF8.GetString(_plainTextBytes, 0, _decryptedByteCount).TrimEnd("\0".ToCharArray());
                    }

                }
                catch
                {
                    return "";
                }
            }
        }

        private static SecurityExtensions _securityExtensions;
        private static readonly object _securityExtensionsLock = new object();

        private IMarketplaceDbContext Db { get; }
        private IMySolsticeDbContext MySolsticeDB { get; }

        public MemberBusinessLogic(IMarketplaceDbContext marketplaceDbContext, IMySolsticeDbContext mySolsticeDb, IConfiguration configuration)
        {
            Db = marketplaceDbContext;
            MySolsticeDB = mySolsticeDb;

            if (_securityExtensions == null)
            {
                lock (_securityExtensionsLock)
                {
                    if (_securityExtensions == null)
                    {
                        _securityExtensions = new SecurityExtensions(configuration);
                    }
                }
            }
        }

        private IQueryable<MemberInfo> GetMemberQuery(IQueryable<DAL.Marketplace.Models.vw_Member> memberQuery)
        {
            var query =
                from m in memberQuery
                select new MemberInfo
                {
                    PersonId = (int)m.PersonID,
                    MemberNumber = m.MemberNumber,
                    DisplayMemberNumber = m.DisplayMemberNumber,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    Gender = (GenderID)m.GenderID,
                    DateOfBirth = m.DateOfBirth.GetValueOrDefault(),
                    IsSubscriber = m.IsSubscriber,
                    LastFourSsn = m.LastFourSSN,
                    EmailAddress = m.EmailAddress,
                    Relationship = m.Relationship
                };

            return query;
        }

        public Task<MemberInfo> GetMemberAsync(int personId)
        {
            var baseQuery =
                from m in Db.Members_vw_Member
                where m.PersonID == personId
                select m;

            var query = GetMemberQuery(baseQuery);

            return query.SingleOrDefaultAsync();
        }

        public Task<List<MemberInfo>> GetDependentsAsync(int parentPersonId)
        {
            var baseQuery =
                from m in Db.Members_vw_Member
                where m.ParentPersonID == parentPersonId
                select m;

            var query = GetMemberQuery(baseQuery);

            return query.ToListAsync();
        }

        public Task<MemberGroupInfo> GetMemberGroupInfoAsync(int personId)
        {
            var baseQuery =
                from m in Db.Members_vw_MemberGroup
                where m.PersonID == personId
                select new MemberGroupInfo
                {
                    GroupNumber = m.GroupNumber,
                    IsIndividual = m.IsIndividual,
                    GroupName = m.GroupName
                };

            return baseQuery.SingleOrDefaultAsync();
        }

        public Task<int> GetMemberPersonIdAsync(int userId)
        {
            var baseQuery =
                from um in Db.Members_vw_UserMember
                where um.UserID == userId
                select (int)um.PersonID;

            return baseQuery.SingleOrDefaultAsync();
        }

        public async Task<MemberInfo> GetMemberForUserRegistrationAsync(DateTime dateOfBirth, string zipCode, string memberNumber, string socialSecurityNumber, string partnerTemplateName)
        {
            if (!zipCode.HasValue())
            {
                throw new ArgumentException("zipCode");
            }
            if (dateOfBirth == DateTime.MinValue)
            {
                throw new ArgumentException("dateOfBirth");
            }
            if (!memberNumber.HasValue() && !socialSecurityNumber.HasValue())
            {
                throw new ArgumentException("memberNumber or socialSecurityNumber");
            }

            var errorMessage = "We were unable to verify your information. Please try again or contact Member Services at 877.760.2247.";
            var dob = dateOfBirth.Date;

            var baseQuery =
                from m in Db.Members_vw_Member
                join a in Db.Members_vw_MemberAddress on m.PersonID equals a.PersonID
                join mg in Db.Members_vw_MemberGroup on m.PersonID equals mg.PersonID
                join g in Db.Group on mg.GroupID equals g.GroupID
                where 
                    m.DateOfBirth == dob && 
                    a.ZipCode == zipCode && 
                    g.MedicareGroup == false
                select m;

            if (!string.IsNullOrEmpty(memberNumber))
            {
                baseQuery = baseQuery.Where(m => (m.MemberNumber == memberNumber)
                                            || (m.CustomerMemberNumber == memberNumber)
                                            || (m.WltAltId == memberNumber)
                                            );
            }
            else if (!string.IsNullOrEmpty(socialSecurityNumber))
            {
                byte[] ssnEncrypted = GetByteValues(_securityExtensions.Encrypt(socialSecurityNumber));
                baseQuery = baseQuery.Where(m => m.SsnBinary == ssnEncrypted);
                errorMessage = "Our system didn’t recognize that SSN. Try using your member number, or give us a call for further assistance.";
            }

            var query = GetMemberQuery(baseQuery);
            var memberList = await query.Take(2).ToListAsync();

            if (memberList.Count == 0)
            {
                throw new BusinessLogicException(errorMessage);
            }

            if (memberList.Count > 1)
            {
                throw new BusinessLogicException(errorMessage);
            }

            var member = memberList[0];

            var memberHasActiveEligibilityTask = MemberHasActiveEligibilityAsync(member.PersonId);
            var memberHasAccountTask = AccountExistsAsync(member.PersonId);

            await Task.WhenAll(memberHasActiveEligibilityTask, memberHasAccountTask);

            var memberHasActiveEligibility = memberHasActiveEligibilityTask.Result;
            var memberHasAccount = memberHasAccountTask.Result;

            if (!memberHasActiveEligibility)
            {
                throw new BusinessLogicException(errorMessage);
            }

            if (memberHasAccount)
            {
                throw new BusinessLogicException(errorMessage);
            }

            if (partnerTemplateName.HasValue())
            {
                var currentPartnerTemplate = await GetPartnerTemplateAsync(member.PersonId);

                if (!partnerTemplateName.Equals(currentPartnerTemplate, StringComparison.OrdinalIgnoreCase))
                {
                    throw new BusinessLogicException(errorMessage);
                }
            }

            return member;
        }

        private async Task<bool> MemberHasActiveEligibilityAsync(int personID)
        {
            var TodaysDate = DateTime.Today;

            var memberEligibility =
                await (
                    from e in Db.Members_udf_MemberBenefitAggregated(personID)
                    where (e.BenefitExpireDate >= TodaysDate && e.BenefitEffectiveDate <= TodaysDate)
                    select e
                ).AnyAsync();

            return memberEligibility;
        }

        private Task<bool> AccountExistsAsync(int personID)
        {
            var account =
                    from u in MySolsticeDB.Users_vw_User
                    where u.PersonID == personID
                    select u.AccountID;

            return account.AnyAsync();
        }

        private byte[] GetByteValues(string str)
        {
            if (str == null)
                return new byte[0];

            var result = Encoding.ASCII.GetBytes(str);
            return result;
        }

        public async Task<string> GetPartnerTemplateAsync(int personId)
        {
            var partnerTemplateId = await Db.Members_vw_MemberGroup
                .Where(g => g.PersonID == personId)
                .Select(g => g.PartnerTemplateID)
                .FirstOrDefaultAsync();

            var partnerTemplateName = await Db.PartnerTemplate
                .Where(t => t.PartnerTemplateID == partnerTemplateId)
                .Select(i => i.Name)
                .SingleAsync();

            return partnerTemplateName;
        }

        public async Task<List<MemberInfo>> GetFamilyAsync(int memberPersonId)
        {
            var subscriberPersonId = await (
                from m in Db.Members_vw_Member
                where m.PersonID == memberPersonId
                select (int)(m.ParentPersonID ?? m.PersonID)
                )
                .SingleOrDefaultAsync();

            var baseQuery =
                from m in Db.Members_vw_Member
                where
                    m.PersonID == subscriberPersonId ||
                    m.ParentPersonID == subscriberPersonId
                orderby
                    m.ParentPersonID,
                    m.PersonID
                select m;

            var result = await GetMemberQuery(baseQuery)
                .ToListAsync();

            return result;
        }
    }
}
