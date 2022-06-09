using System.Collections.Generic;
using Dominion.Utility.Dto;
using Dominion.Authentication.Dto;

namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// This is a container class for basic user profile data that is intended to service viewing and updating
    /// basic user profile info.
    /// </summary>
    public class BasicUserProfileDto : DtoObject
    {
        public int UserId { get; set; }

        public int AuthUserId { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }


        /// <summary>
        /// The entire list of questions with the user's answers
        /// for the questions they answered.
        /// If they didn't answer the question, there will be no answer.
        /// </summary>
        public IEnumerable<SecurityQuestionData> SecurityQuestions { get; set; }

        //public int SecretQuestionId { get; set; }
        //public string SecretQuestionAnswer { get; set; }
    }
}