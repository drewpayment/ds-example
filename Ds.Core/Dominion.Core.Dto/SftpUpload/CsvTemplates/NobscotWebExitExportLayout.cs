using System;
using Dominion.Utility.Csv;

namespace Dominion.Core.Services.SftpUpload.CsvTemplates
{
    public class NobscotWebExitExportLayout
    {
        public object fieldName;

        // Directly mapped properties
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CellPhoneNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? LastPerformanceRatingDecimal { get; set; }
        public string DepartmentRaw { get; set; }
        public string ClientCode { get; set; }
        public string ClientContactEmail { get; set; }

        // Calculated properties
        public string Rebounder
        {
            get
            {
                return "Yes";
            }
        }
        public string UserName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)) return "NA";
                else return FirstName + " " + LastName;
            }
        }
        public string EmailAndPhone
        {
            get
            {
                var phone = (this.CellPhoneNumber ?? this.HomePhoneNumber);
                var emailAddress = !string.IsNullOrWhiteSpace(EmailAddress) ? EmailAddress : "";
                var separator = !string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(emailAddress) ? "/" : "";
                var combined = emailAddress + separator + phone;
                return !string.IsNullOrWhiteSpace(combined) ? combined : "NA"; ;
            }
        }
        public string AgeRange
        {
            get
            {
                string range = "NA";

                if (this.BirthDate.HasValue)
                {
                    DateTime birthdate = this.BirthDate.Value;
                    var today = DateTime.Now;
                    var age = today.Year - birthdate.Year;


                    if (birthdate.Date > today.AddYears(-age)) age--;

                    if (age <= 17) { range = "17 or younger"; }
                    else if (age <= 22) { range = "18-22"; }
                    else if (age <= 30) { range = "23-30"; }
                    else if (age <= 40) { range = "31-40"; }
                    else if (age <= 50) { range = "41-50"; }
                    else { range = "51 or older"; }
                }

                return range;

            }
        }
        public string LengthOfService
        {
            get
            {
                var lengthOfService = "NA";

                if (this.HireDate.HasValue)
                {
                    DateTime hiredate = this.HireDate.Value;
                    var today = DateTime.Now;
                    var tenure = today.Year - hiredate.Year;

                    if (hiredate.Date > today.AddYears(-tenure)) tenure--;

                    if (tenure < 1) { lengthOfService = "Less than 1 year"; }
                    else if (tenure < 3) { lengthOfService = "1-3 years"; }
                    else if (tenure < 5) { lengthOfService = "3-5 years"; }
                    else if (tenure < 10) { lengthOfService = "5-10 years"; }
                    else if (tenure < 15) { lengthOfService = "10-15 years"; }
                    else if (tenure < 20) { lengthOfService = "15-20 years"; }
                    else { lengthOfService = "20+ years"; }
                }

                return lengthOfService;
            }
        }
        public string HrContact
        {
            get
            {
                var hrContact = ClientContactEmail;

                switch (ClientCode)
                {
                    // STS-1382 Shane updated all emails to point to redico's hr mail at AH's request.
                    case "AHP1": //American House
                    case "AH21": //American House
                    case "AH31": //American House
                    case "DMZ1": //Redico
                    case "DN21": //Continuum
                        hrContact = "HR@redico.com";
                        break;
                }

                return hrContact;
            }
        }
        public string LastPerformanceRating
        {
            get
            {
                if (LastPerformanceRatingDecimal.HasValue) return LastPerformanceRating.ToString();
                else return "NA";
            }
        }

        public string Department
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DepartmentRaw)) return DepartmentRaw;
                else return "NA";
            }
        }

    }
}

