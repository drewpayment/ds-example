using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public partial class AddApplicantDto
    {
        public int    ApplicantId    { get; set; }
        public string FirstName      { get; set; }
        public string MiddleInitial  { get; set; }
        public string LastName       { get; set; }
        public string Address1       { get; set; }
        public string Address2       { get; set; }
        public string City           { get; set; }
        public int    State          { get; set; }
        public string ZipCode        { get; set; }
        public string Phone          { get; set; }
        public string CellPhone      { get; set; }
        public bool? IsTextEnabled   { get; set; }
        public string Email          { get; set; }
        public string DOB            { get; set; }
        public string UserName       { get; set; }
        public string Password       { get; set; }
        public int    SecretQuestion { get; set; }
        public string SecretAnswer   { get; set; }
        public int    ClientId       { get; set; }
        public string WorkPhone      { get; set; }
        public string Extension      { get; set; }
        public int    CountryId      { get; set; }
        public int    DsUserId       { get; set; }
        public string Password1      { get; set; }
        public string Password2      { get; set; }
        public int DsUserType        { get; set; }
        public int EmployeeId        { get; set; }
        public int ViewEmployee      { get; set; }
        public int ViewRates         { get; set; }
        public bool SecuritySettings { get; set; }
        public bool AllowUnSafePass  { get; set; }
        public bool IsStruckOut      { get; set; }
        public bool ViewOnly         { get; set; }
        public bool EmployeeSelfServiceOnly { get; set; }
        public bool ReportingOnly           { get; set; }
        public bool BlockPayrollAccess      { get; set; }
        public bool Timeclock               { get; set; }
        public bool BlockHR                 { get; set; }
        public bool EmployeeOnly            { get; set; }
        public bool ApplicantAdmin          { get; set; }
        public bool ViewTaxPackets          { get; set; }
        public bool MustChangePWD           { get; set; }
        public DateTime? FromDate           { get; set; }
        public DateTime? ToDate             { get; set; }
        public int? Timeout                 { get; set; }
        public int? PostingId               { get; set; }
        public string PostLink              { get; set; }
        public string signInToken           { get; set; }
    }
}
