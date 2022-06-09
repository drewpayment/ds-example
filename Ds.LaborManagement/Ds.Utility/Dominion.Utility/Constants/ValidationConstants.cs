using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Constants
{
    public class ValidationConstants
    {
        #region A Bunch Of Validation Consts

        // ----------------------------------
        // COMPANY CODE
        // ----------------------------------
        public const string CO_CODE_EER_REQ_MSG = "Company code is required.";
        public const string CO_CODE_ERR_LEN_MSG = "Company code must be between 1 and 4 characters";
        public const string CO_CODE_ERR_FMT_MSG = "Invalid company code format.";
        public const int    CO_CODE_MIN_LEN     = 1;
        public const int    CO_CODE_MAX_LEN     = 4;
        public const string CO_CODE_REG_MATCH   = "([a-zA-Z0-9 .&'-]+)";


        // ----------------------------------
        // SOCIAL SECURITY NUMBER
        // ----------------------------------
        public const string SSN_EER_REQ_MSG        = "Social Security Number is required.";
        public const string SSN_ERR_FMT_MSG        = "Invalid Social Security Number format.";
        public const string SSN_REG_MATCH          = @"^\d{9}|\d{3}-\d{2}-\d{4}$";
        public const string SSN_REG_MATCH_ALW_WSP  = @"^(\s*|\d{3}-\d{2}-\d{4})$"; //ALLOWS WHITE SPACE ??? WHY
        public const string SSN_REG_MASK           = @"(\d{3})-(\d{2})-(\d{4})";


        // ----------------------------------
        // PASSWORD
        // ----------------------------------
        //public const string PASS_DISPLAY            = "Password";
        //public const string PASS_EER_MATCH_MSG      = "Passwords do not match.";
        //public const string PASS_EER_REQ_MSG        = "Password is required.";
        //public const string PASS_ERR_LEN_MSG        = "Password must be 8-50 characters";
        //public const string PASS_ERR_FMT_MSG        = "Password must contain an uppercase and lowercase letter, one number, and be 8-50 characters.";
        //public const string PASS_ERR_FMT_EASY_MSG   = "Password must contain at least one character, no spaces, and numbers and letters only.";
        //public const int    UN_SAFE_PASS_MIN_LEN    = 1;
        //public const int    PASS_MIN_LEN            = 8;
        //public const int    PASS_MAX_LEN            = 50;
        ////public const string PASS_REG_MATCH        = @"(?=.*\d)(?=.*[A-Za-z]).{8,}";
        ////"^.*(?=.{8,})(?=.*\d)(?=.*[a-zA-Z]).*$"



        // ----------------------------------
        // EMPLOYEE NUMBER
        // ----------------------------------
        public const string EE_NUM_EER_REQ_MSG = "Employee Number is required.";
        public const string EE_NUM_ERR_LEN_MSG = "Employee Number must be between 1 and 10 characters";
        public const string EE_NUM_ERR_FMT_MSG = "Invalid Employee Number format.";
        public const int    EE_NUM_MIN_LEN     = 1;
        public const int    EE_NUM_MAX_LEN     = 10;
        public const string EE_NUM_REG_MATCH   = @"([^\s]\d)";


        // ----------------------------------
        // EMAIL
        // ----------------------------------
        public const string EMAIL_DISPLAY     = "Email Address";
        public const string EMAIL_ERR_LNG_MSG = "Email address must only be 50 characters long.";
        public const string EMAIL_ERR_FMT_MSG = "Email address is invalid.";
        public const int    EMAIL_MIN_LEN     = 1;
        public const int    EMAIL_MAX_LEN     = 50;


        // ----------------------------------
        // USER NAME
        // ----------------------------------
        public const string USR_NAME_DISPLAY = "Username.";
        public const string USR_NAME_EER_TAKEN_MSG = "Username already exists. Please enter a different username.";
        public const string USR_NAME_EER_REQ_MSG = "Username is required.";
        public const string USR_NAME_ERR_LEN_MSG = "Username must be between 4 and 15 characters";
        public const string USR_NAME_ERR_FMT_MSG = "Invalid Username format.";
        public const int    USR_NAME_MIN_LEN     = 4;
        public const int    USR_NAME_MAX_LEN     = 15;
        public const string USR_NAME_REG_MATCH   = @"([^\s]\d)";


        public const string APPLICATION_EXCEPTION = "An error occurred has occured. Please contact your administrator.";
        public const string USR_ERROR_ADDING = "An error occurred while trying to add the user.";


        public const string PHONE_10_DIGIT_ERR_MSG = "Phone format: XXX-XXX-XXXX";


        // ----------------------------------
        // SECURITY QUESTIONS
        // Answers cannot be duplicated
        // ----------------------------------
        public const string SEC_QUEST_ERR_DUP_MSG = "Answers must be unique.";
        public const string SEC_QUEST_ERR_REQ_MSG = "Question Required.";
        public const string SEC_QUEST_ANSWER_ERR_LEN_MSG = "3-50 Characters.";
        public const int SEC_QUEST_ANSWER_MIN_LEN = 3;
        public const int SEC_QUEST_ANSWER_MAX_LEN = 50;


        //// ----------------------------------
        //// 2-FACTOR AUTH 
        //// ----------------------------------
        //public const string ERR_CODE_NUM_ERR_MSG = "Invalid Format";

        // ----------------------------------
        // GENERIC
        // ----------------------------------
        //public const string ERR_REQ_MSG = "Required.";
        //public const string ERR_FRMT_MSG = "Invalid Format.";

        #endregion


        #region REVIEW THESE FOR DELETION - CHECK ALL PROJECTS AND EVEN THE DYNAMICALLY CREATED HTML PAGES

        ///////// <summary>
        ///////// Must be between 1-50 chars.
        ///////// ^[\w]{1,50}$
        ///////// ^ asserts position at start of a line
        ///////// Match a single character present in the list below [\w]{1,50}
        ///////// {1,50} Quantifier — Matches between 1 and 50 times, as many times as possible, giving back as needed (greedy)
        ///////// \w matches any word character (equal to [a-zA-Z0-9_])
        ///////// $ asserts position at the end of a line
        ///////// </summary>
        ////////public const string UNSAFE_PASS_REG_MATCH = @"^[\w]{1,50}$";


        /// <summary>
        /// 8-50 chars.
        /// Any characters.
        /// </summary>
        //public const string UNSAFE_PASS_REG_MATCH = @"^(?=.*).{1,50}$";


        /// <summary>
        /// 8-50 chars.
        /// Must have 1 upper case letter
        /// Must have 1 lower case letter
        /// Must have 1 number
        /// Once the 4 above requirements are met; the user can use any char
        /// </summary>
        //public const string PASS_REG_MATCH = @"^.*(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,50}$";
        //public const string PASS_REG_MATCH = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*";

        #endregion

    }

    public class ValidConsts
    {
        public class Generic
        {
            public const string RequiredMsg = "Required.";
            public const string InvalidFrmtMsg = "Invalid Format.";
        }

        public class User
        {
            #region User Name RegEx and Messages

            public class UserName
            {
                public class RegEx
                {
                    public const string AllowedChars = @"[a-zA-Z0-9_.!]"; //https://regex101.com/r/sQv0jK/1 //original allowed chars: [a-zA-Z0-9_'.\s]
                    public const string Complete = @"^[a-zA-Z0-9_.!]{4,15}$";
                    public const string Length = @"^.{4,15}$";
                }

                public class Msg
                {
                    public const string ValidCharsList = "numbers, letters, (_), and (.)";
                    public const string ProfilePage = "Username must be 4-15 characters, and only contain: " + ValidCharsList + ""; //used on the Profile.aspx page. It's used in a summary so it has to be more specific
                    public const string MustContain = "Must contain: " + ValidCharsList;
                    public const string Length = "Must be 4-15 characters";
                }
            }
            

            #endregion

            public const string PassMustMatchMsg        = "Passwords must match.";
            public const string PassFrmtMsg             = "Password must contain an uppercase and lowercase letter, one number, and have 8-50 characters. Special characters are allowed.";
            public const string UnSafePassFrmtMsg       = "Password must have 1-50 characters.";
            public const int PassMinUnSafe              = 1;
            public const int PassMin                    = 8;
            public const int PassMax                    = 50;

            public class MinMsg
            {
                public const string PassLen = "8-50 characters.";
                public const string PassOneUpperCase = @"Must contain one upper case.";
                public const string PassOneLowerCase = @"Must contain one lower case.";
                public const string PassOneNumber = @"Must contain one number.";
                public const string PassNoSpecialChars = @"No special characters, or spaces.";

                //review: jay: will have to add context to 'recent' specifying what recent means.
                public const string PassNoRecentPasswords = @"Recent passwords not allowed.";

                public const string UnSafePassLen = @"1-50 characters.";
                public const string UnSafePassCharsOnly = @"Numbers and letters only.";
            }


            #region Password Reg Ex (verified good)

            public const string RegExPassOneNumber = @"[\d]";
            public const string RegExPassOneUpperCase = @"[A-Z]";
            public const string RegExPassOneLowerCase = @"[a-z]";

            /// <summary>
            /// https://regex101.com/
            /// </summary>
            public const string UnSafePassFrmt = @"^(?=.*).{1,50}$";
            public const string PassFrmt = @"^.*(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,50}$";

            #endregion

        }

        public class Name
        {
            public const string MidInitialFrmtMsg = "Must be a single letter.";
        }

        public class Address
        {
            public const string Zip9FrmtMsg = "xxxxx or xxxxx-xxxx";
        }
    }
}

// ----------------------------------
// USER NAME
// ----------------------------------
//https://regex101.com/r/GwKUfq/1 or https://regex101.com/r/sQv0jK/1
//Googles Username Rules:
//https://support.google.com/a/answer/33386?hl=en
//public const string UserNameValidCharsList = "(A-Z), (a-z), (0-9), (-), (_), ('), ( ), and (.)";
//Usernames can contain letters(a-z), numbers(0-9), dashes , underscores(_), apostrophes('), and periods (.).
//Usernames can't contain an ampersand (&), equal sign (=), brackets (<,>), plus sign , comma (,), or more than one period (.) in a row.
//Usernames can begin or end with non-alphanumeric characters except periods (.), with a maximum of 64 characters.
//public const string UserNameRegEx = @"^(?=\S)(?!.*\.\.)([a-zA-Z0-9_'.\s]*)(\S$)"; //https://regex101.com/r/GwKUfq/1
//public const string UserNameRegEx1 = @"^(?=\S)([a-zA-Z0-9_'.\s]*)(\S$)"; //https://regex101.com/r/sQv0jK/1
//public const string UserNameRegEx1 = @"^(?=\S)([a-zA-Z0-9_.]{4,15})(\S$)"; //this one checks the lengths //https://regex101.com/r/pTSGm0/1
//public const string UserNameCharCheck = @"^[a-zA-Z0-9_.]+$"; //https://regex101.com/r/sQv0jK/1
//public const string UserNameRegEx3 = @"^(?=\S).*(\S$)"; //https://regex101.com/r/sQv0jK/1

//public const string UserNameMsg2 = "User name can only contain: " + UserNameValidCharsList; //used on the Profile.aspx page. It's used in a summary so it has to be more specific

//public const string UserNameSpacesMsg1 = "Can not start or end with a space.";
//public const string UserNameSpacesMsg2 = "Username can not start or end with a space."; //used on the Profile.aspx page. It's used in a summary so it has to be more specific

//public const int UserNameMin = 4;
//public const int UserNameMax = 15;