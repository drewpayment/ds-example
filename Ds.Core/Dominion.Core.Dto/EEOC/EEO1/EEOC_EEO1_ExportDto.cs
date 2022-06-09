using System;
using System.Collections.Generic;
using Dominion.Core.Dto.EEOC.EEO1;
using Dominion.Core.Dto.EEOC.Enums;
using Dominion.Core.Dto.Onboarding;

namespace Dominion.Core.Dto.EEOC
{
    public class EEO1ExportDto
    {
        public ExportClientEEOCDto ClientEEOC { get; set; }
        public ClientDto Client { get; set; }

        public IEnumerable<EEOCPayBandDto> EeocPayBandDtos { get; set; }

        public class ClientDto
        {
            public IEnumerable<EmployeeDto>               Employees              { get; set; }
            public IEnumerable<LocationDto>               Locations              { get; set; }
            public IEnumerable<EmployeeW2TotalsDto>       EmployeeW2Totals       { get; set; }
            public IEnumerable<PaycheckEarningHistoryDto> PaycheckEarningHistory { get; set; }
            public string FEIN { get; set; }
        }

        public class EmployeeDto
        {
            public int      EmployeeId  { get; set; }
            public int?     LocationId  { get; set; }
            public int?     JobCategory { get; set; }
            public int?     Race        { get; set; }
            public string   Gender      { get; set; }
            public int?     PayBand     { get; set; }
            public int?  TotalHours  { get; set; }

            //we don't need this because the current value will not matter since this is done on a selected 
            //pay period and the current value may not be the same from that period
            //we'll have to make a query to find what employees we want to include
            //public EmployeeStatusType EmployeePayStatus { get; set; }
        }

        public class EmployeeW2TotalsDto
        {
            public int?     EmployeeId     { get; set; }
            public int?     ClientId       { get; set; }
            public int?     W2Year         { get; set; }
            public decimal? GrossPayEndYtd { get; set; }
        }

        public class PaycheckEarningHistoryDto
        {
            public int       EmployeeID       { get; set; }
            public int       ClientID         { get; set; }
            public DateTime? PayrollCheckDate { get; set; }
            public double?   Hours            { get; set; }

        }

        public class LocationDto
        {
            public int    EeocLocationId { get; set; }

            public bool   IsActive { get; set; }

            public bool   IsHeadquarters { get; set; }
            
            public string UnitName { get; set; }

            public string UnitNumber { get; set; }

            public string UnitAddressLine1 { get; set; }

            public string UnitAddressLine2 { get; set; }

            public string CityName { get; set; }

            public string CountyFipsName { get; set; }

            public string StateAbbreviation { get; set; }

            public string ZipCode { get; set; }

            public int EmployeeCount { get; set; }
        }

        public class EEO1Record
        {
            public string           CompanyNumber         { get; set; }
            public EEO1RecordStatus StatusCode            { get; set; }
            public string           UnitName              { get; set; }
            public string           UnitNumber            { get; set; }
            public string           UnitAddressLine1      { get; set; }
            public string           UnitAddressLine2      { get; set; }
            public string           CityName              { get; set; }
            public string           CountyFipsName        { get; set; }   //need to get this from the db 
            public string           StateAbbreviation     { get; set; }
            public string           ZipCode               { get; set; }
            public int              QuestionB2C           { get; set; }
            public int              QuestionC1            { get; set; }
            public int              QuestionC2            { get; set; }
            public int              QuestionC3            { get; set; }
            public string           QuestionD1            { get; set; }   
            public string           DunBradstreetNumber   { get; set; }
            public string           NaicsCode             { get; set; }
            public string           Title                 { get; set; }
            public string           Name                  { get; set; }
            public string           Phone                 { get; set; }
            public string           EmailAddress          { get; set; }
            public string           EIN                   { get; set; }   
            public List<RecordLine> MatrixData            { get; set; }    //there should be 11 of these
            public List<RecordLine> MatrixDataHours       { get; set; }

            public EEO1Record()
            {
                MatrixData      = new List<RecordLine>();
                MatrixDataHours = new List<RecordLine>();
            }
        }

        public class EEO1Comp2Record
        {
            /// <summary>
            /// Required | AlphaNumeric (exactly 8)
            /// Unique 8-Digit Identifier for entire company provided By NORC To Employer
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// Required | Number
            /// Indicates type of report as indicated in part A of EEOC Standard Form 100:
            /// <see cref="EEO1RecordStatus"/>
            /// </summary>
            public EEO1RecordStatus StatusCode { get; set; }

            /// <summary>
            /// Optional | AlphaNumeric (max 7)
            /// Should be reported except for new status code 8, 8 and all status code 9 records
            /// </summary>
            public string UnitNumber { get; set; }

            /// <summary>
            /// Required | AlphaNumeric (max 35)
            /// Establishment Name
            /// </summary>
            public string UnitName { get; set; }

            /// <summary>
            /// Required | AlphaNumeric (max 46)
            /// Establishment Address
            /// </summary>
            public string UnitAddress { get; set; }

            /// <summary>
            /// Optional | AlphaNumeric (max 25)
            /// Extended Establishment Address to include Suite No., PO Box, Etc
            /// </summary>
            public string UnitAddress2 { get; set; }

            /// <summary>
            /// Required | Alpha (Max 28)
            /// City Name
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// Required | Alpha (Max 2)
            /// FIPS Pub 5-2 (Census)
            /// State Abbreviation, Valid Values are 5 states plus DC, look up Information
            /// </summary>
            public string State { get; set; }

            /// <summary>
            /// Required | Numeric (Exactly 5)
            /// U.S. Postal Service Zip Code
            /// </summary>
            public string Zipcode { get; set; }

            /// <summary>
            /// Required | AlphaNumeric (Max 28)
            /// County name
            /// </summary>
            public string CountyName { get; set; }

            /// <summary>
            /// Required | Numeric (Exactly 9)
            /// Federal EIN(Tax ID) 9 Digits
            /// </summary>
            public string FEIN { get; set; }

            /// <summary>
            /// Required | Numeric (exactly 6)
            /// NAICS Code
            /// </summary>
            public string NaicsCode { get; set; }

            /// <summary>
            /// Required | Numeric (max 1)
            /// Was an EEO-1 Report Filed for this establishment last year? 1=Yes, 2=No
            /// </summary>
            public int QuestionB2C { get; set; }

            /// <summary>
            /// Required | Numeric (Max 1)
            /// Is this Establishment:
            ///     1 - Not Exempt as provided for by 41 CFR 60-1.5
            ///     2 - A prime contractor or first-tier subcontractor
            ///     3 - Have a contract, subcontract , or purchase order amounting to $50,000 or more
            ///         Or serve as a depository of government funds in any amount
            ///         or is a financial institution which is an issuing and paying agents for U.S. Savings Bonds
            ///         and/or Savings notes
            ///     1=Yes, 2=No
            /// </summary>
            public int QuestionD2 { get; set; }

            /// <summary>
            /// Required | Numeric (max 2)
            /// <see cref="EEOCJobCategory"/>
            /// </summary>
            public EEOCJobCategory JobCategory { get; set; }

            /// <summary>
            /// Required | Alpha (Max 1)
            /// RACE/ETHNCITY/GENDER CODES ARE AS FOLLOWS:
            ///    A: HISPANIC or LATINO MALES
            ///    B: HISPANIC or LATINO FEMALES
            ///    C: WHITE MALES
            ///    D: BLACK or AFRICAN AMERICAN MALES
            ///    E: NATIVE HAWAIIAN or OTHER PACIFIC ISLANDER MALES
            ///    F: ASIAN MALES
            ///    G: NATIVE AMERICAN or ALASKA NATIVE MALES
            ///    H: TWO or MORE RACES MALES
            ///    I: WHITE FEMALES
            ///    J: BLACK or AFRICAN AMERICAN FEMALES
            ///    K: NATIVE HAWAIIAN or OTHER PACIFIC ISLANDER FEMALES
            ///    L: ASIAN FEMALES
            ///    M: NATIVE AMERICAN or ALASKA NATIVE FEMALES
            ///    N: TWO or MORE RACES FEMALES
            ///    Z: NOT APPLICABLE(TYPE 6 REPORT ONLY)
            /// </summary>
            public string RaceEthnicityGender { get; set; }

            /// <summary>
            /// Required | Numeric (Max 2)
            /// PAY BAND CATEGORY CODES ARE AS FOLLOWS:
            ///    1: $19,239 and under
            ///    2: $19,240 - $24,439
            ///    3: $24,440 - $30,679
            ///    4: $30,680 - $38,999
            ///    5: $39,000 - $49,919
            ///    6: $49,920 - $62,919
            ///    7: $62,920 - $80,079
            ///    8: $80,080 - $101,919
            ///    9: $101,920 - $128,959
            ///    10: $128,960 - $163,799
            ///    11: $163,800 - $207,999
            ///    12: $208,000 and over
            ///    99: Total Employees(TYPE 6 REPORT ONLY)
            /// </summary>
            public int AnnualSalary { get; set; }

            /// <summary>
            /// Required | Numeric (Max 13)
            /// Total Employees per Category, No Negatives
            /// Note: Filers Should NOT Report Rows Where Number of Employees = 0
            /// </summary>
            public int TotalEmployees { get; set; }

            /// <summary>
            /// Required | Numeric (Max 13)
            /// Total Hours per category, no negatives
            /// Exception: type 6 Report which will Report "-3"
            /// </summary>
            public int TotalHours { get; set; }
        }

        public class RecordLine
        {
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
            public int D { get; set; }
            public int E { get; set; }
            public int F { get; set; }
            public int G { get; set; }
            public int H { get; set; }
            public int I { get; set; }
            public int J { get; set; }
            public int K { get; set; }
            public int L { get; set; }
            public int M { get; set; }
            public int N { get; set; }
            public int O { get; set; }

            public int CalculateTotals()
            {
                return A+B+C+D+E+F+G+H+I+J+K+L+M+N;
            }
        }
    }

}
