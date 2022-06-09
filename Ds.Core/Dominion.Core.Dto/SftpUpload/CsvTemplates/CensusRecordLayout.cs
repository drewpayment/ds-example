using System;
using Dominion.Utility.Csv;

namespace Dominion.Core.Services.SftpUpload.CsvTemplates
{
    public class CensusRecordLayout
    {
        public string ee_ssn { get; set; }
        public string id => employee_number;
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string country_code { get; set; }
        public DateTime? birth_date { get; set; }
        public string gender { get; set; }
        public DateTime? hire_date { get; set; }
        public DateTime? term_date { get; set; }
        public string employment_status { get; set; }
        public string employment_status_indicator { get; set; }
        public string company { get; set; }
        public string company_desc { get; set; }
        public string division_desc { get; set; }
        public string location_desc { get; set; }
        public string department { get; set; }
        public string department_desc { get; set; }
        public string pay_periods { get; set; }
        public string hourly_rate { get; set; }
        public double? annual_income { get; set; }
        public string hourly { get; set; }
        public string exempt { get; set; }
        public string email_address { get; set; }
        public string title { get; set; }
        public DateTime? rehire_date { get; set; }
        public string benefit_package { get; set; }
        public string benefit_eligible { get; set; }
        public DateTime? benefit_eligibility_date { get; set; }

        public string employee_number { get; set; }
        public string client_code { get; set; }
        public string pay_frequency { get; set; }
        public string cell_phone { get; set; }
        public string eeoc_job_category { get; set; }
        public string employment_status_active_or_inactive { get; set; }
        public string termination_reason { get; set; }
    }
}
