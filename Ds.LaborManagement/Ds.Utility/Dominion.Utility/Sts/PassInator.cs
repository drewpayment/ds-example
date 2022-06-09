using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using BCrypt.Net;
using Dominion.Utility.Configs;

namespace Dominion.Utility.Sts
{
    public static class PassInator
    {
        //xxxhashxxx 
        //yyyhashyyy
        //xxxyyyxxx  -- i accidently used this in DS Source
        private static readonly string[] _validMachines = new[] { "DOM-WS072", "DUCATI", "DUCATI2", "ZMONSTER" };

        private static bool CanSkipPassCheck()
        {
            if (ConfigValues.PassAlwaysMatches)
            {
                var check = _validMachines.Contains(Environment.MachineName.ToUpper());
                return check;
            }

            return false;
        }

        #region Tokenizer
        public class TokenOptions
        {
            public int WorkFactor { get; set; }
            public SaltRevision SaltRevisionType { get; set; }

            public TokenOptions(int workFactor, SaltRevision saltRevisionType)
            {
                WorkFactor = workFactor;
                SaltRevisionType = saltRevisionType;
            }

            public static TokenOptions ApplicantAuthenticationToken => 
                new TokenOptions(5, SaltRevision.Revision2B);
        }

        public static string GenerateToken(string text, TokenOptions tokenOptions)
        {
            return BCrypt.Net.BCrypt.HashString(text, tokenOptions.WorkFactor, tokenOptions.SaltRevisionType);
        }
        #endregion

        #region NEW WAY

        /// <summary>
        /// http://davismj.me/blog/bcrypt/
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="dbPass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool MatchesDb(string pass, string dbPass, string salt)
        {
            if (CanSkipPassCheck())
                return true;

            //var result = BCrypt.Net.BCrypt.Verify(pass, dbPass);
            //var result = MatchesDbOld(pass, dbPass, salt);
            var result = MatchesDbDetermine(pass, dbPass, salt);
            return result;

        }

        /// <summary>
        /// http://davismj.me/blog/bcrypt/
        /// old method: FormsAuthentication.HashPasswordForStoringInConfigFile(userName + clearPassword, "md5");
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        /// <param name="useOld">Set to true if you want it to use the old method.</param>
        /// <returns></returns>
        public static string ToDb(string pass, string salt, bool useOld = false)
        {
            if(useOld)
                return ToDbOld(pass, salt);

            return BCrypt.Net.BCrypt.HashPassword(pass ?? string.Empty);
        }

        #endregion

        #region GUESS WHICH WAY BASED ON DBPASS

        /// <summary>
        /// Check the length of the hashed dbPass to detemine if it's the old or the new.
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="dbPass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static bool MatchesDbDetermine(string pass, string dbPass, string salt)
        {
            if (CanSkipPassCheck())
                return true;

            if (dbPass.Length == 60)
                return BCrypt.Net.BCrypt.Verify(pass, dbPass); //the new way

            //else the pass length was 32 (length of the legacy password
            return MatchesDbOld(pass, dbPass, salt);
        }

        #endregion

        #region OLD WAY

        /// <summary>
        /// Had to keep the old way since we can't required them to change the password.
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static string ToDbOld(string pass, string salt)
        {
            //old method: FormsAuthentication.HashPasswordForStoringInConfigFile(userName + clearPassword, "md5");
            //all old passwords are stored as 32 chars
            //all new passwords are stored as 60 chars
            if (string.IsNullOrEmpty(pass))
                return pass;

            return FormsAuthentication.HashPasswordForStoringInConfigFile(salt + pass, "md5");
        }
        private static bool MatchesDbOld(string pass, string dbPass, string salt)
        {
            if (CanSkipPassCheck())
                return true;

            var passwordToCheck = ToDbOld(pass, salt);
            var result = passwordToCheck == dbPass;
            return result;
            //xxxhashxxx 
            //yyyhashyyy
        }

        #endregion

    }

}

