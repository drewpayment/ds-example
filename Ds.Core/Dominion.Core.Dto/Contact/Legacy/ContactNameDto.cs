using System;
using System.ComponentModel.DataAnnotations;

using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.Utility.Extensions;
using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Contact.Legacy
{
    [Serializable]
    [Obsolete("No longer using DtoObject pattern")]
    public class ContactNameDto : DtoObject, IContactNameDto
    {
        /// <summary>
        /// The first name of the contact.
        /// </summary>
        [Required(ErrorMessage = "First Name must be specified.")]
        public string FirstName { get; set; }

        /// <summary>
        /// The middle name of the contact.
        /// </summary>
        public string MiddleInitial { get; set; }

        /// <summary>
        /// The last name of the contact.
        /// </summary>
        [Required(ErrorMessage = "Last Name must be specified.")]
        public string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Normalize()
        {
            this.NormalizeContactName();
        }
    }
}