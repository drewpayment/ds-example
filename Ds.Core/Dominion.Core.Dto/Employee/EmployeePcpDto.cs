using System;

using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Employee
{
    /// <summary>
    /// Class that defines an EmployeePcp DTO object.
    /// </summary>
    [Serializable] 
    public class EmployeePcpDto : DtoObject
    {
        public int?     EmployeeId { get; set; }
        public string   FirstName  { get; set; }
        public string   LastName   { get; set; }
        public string   Address    { get; set; }
        public string   Address2   { get; set; }
        public string   City       { get; set; }
        public int?     StateId    { get; set; }
        public string   ZipCode    { get; set; }
        public string   NpiNumber  { get; set; }
        public int      ModifiedBy { get; set; }
        public DateTime Modified   { get; set; }
        public string   StateName  { get; set; }
    }
}