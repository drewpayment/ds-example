using Dominion.Core.Dto.Contact.Legacy;
using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.Utility.Extensions;
using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Employee
{
    /// <summary>
    /// Represents a full property set entity to dto mapping.
    /// </summary>
    public class EmployeeEmergencyContactDto : ContactNameDto, IPhoneNumbersDto
    {
        public int EmployeeEmergencyContactId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }

        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }

        public string Relation { get; set; }
        public string EmailAddress { get; set; }

        public byte InsertApproved { get; set; }

        public override void Normalize()
        {
            this.NormalizePhoneNumbers();
            base.Normalize();
        }
    }

    /// <summary>
    /// A DTO representing a list of emergency contacts in a last, first name fashion.
    /// Id is included for correlation.
    /// </summary>
    /// todo: jay: remove: this is no longer needed. Look for usages and remove.
    public class EmployeeEmergencyContactLastFirstNameIdDto : DtoObject
    {
        public int EmployeeEmergencyContactId { get; set; }
        public string PersonNameLastFirst { get; set; }
    }

    public class EmployeeEmergencyContactFullNameDto : DtoObject
    {
        public int EmployeeEmergencyContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeEmergencyContactRequestDto: EmployeeEmergencyContactDto
    {
        public bool requested { get; set; }
    }
}