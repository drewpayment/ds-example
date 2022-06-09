using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class GenW2EmployeeHistory
    {
        public virtual string   ClientCode                     { get; set; }
        public virtual string   ClientName                     { get; set; }
        public virtual string   FederalIdNumber                { get; set; }
        public virtual string   ClientAddress1                 { get; set; }
        public virtual string   ClientAddress2                 { get; set; }
        public virtual string   ClientCity                     { get; set; }
        public virtual string   ClientStateAbbreviation        { get; set; }
        public virtual string   ClientZipCode                  { get; set; }
        public virtual int?     ClientId                       { get; set; }
        public virtual int?     EmployeeId                     { get; set; }
        public virtual string   FirstName                      { get; set; }
        public virtual string   LastName                       { get; set; }
        public virtual string   MiddleInitial                  { get; set; }
        public virtual string   AddressLine1                   { get; set; }
        public virtual string   AddressLine2                   { get; set; }
        public virtual string   City                           { get; set; }
        public virtual int?     StateId                        { get; set; }
        public virtual string   PostalCode                     { get; set; }
        public virtual string   SocialSecurityNumber           { get; set; }
        public virtual bool?    IsW2Pension                    { get; set; }
        public virtual string   EmployeeNumber                 { get; set; }
        public virtual string   TaxIdNumber                    { get; set; }
        public virtual string   StateAbbreviation              { get; set; }
        public virtual decimal? GrossPayEndYtd                 { get; set; }
        public virtual decimal? SocSecWagesEndYtd              { get; set; }
        public virtual decimal? MedicareWagesEndYtd            { get; set; }
        public virtual decimal? SocSecTaxEndYtd                { get; set; }
        public virtual decimal? MedicareTaxEndYtd              { get; set; }
        public virtual decimal? FederalTaxEndYtd               { get; set; }
        public virtual decimal? TpsFedTaxEndYtd                { get; set; }
        public virtual decimal? DedDependentCareEndYtd         { get; set; }
        public virtual decimal? DedNonQualifiedPlanEndYtd      { get; set; }
        public virtual decimal? EicEndYtd                      { get; set; }
        public virtual decimal? DedPensionEndYtd               { get; set; }
        public virtual decimal? ThirdPartyPayEndYtd            { get; set; }
        public virtual decimal? TipsEndYtd                     { get; set; }
        public virtual bool?    IsStatutoryEmployee            { get; set; }
        public virtual int?     StateTaxStateId                { get; set; }
        public virtual decimal? StateTaxTaxAmountYtd           { get; set; }
        public virtual string   StateTaxAbbreviation           { get; set; }
        public virtual decimal? StateTaxTaxableWagesYtd        { get; set; }
        public virtual string   LocalTax1Desc                  { get; set; }
        public virtual decimal? LocalTax1Wages                 { get; set; }
        public virtual decimal? LocalTax1Amount                { get; set; }
        public virtual string   LocalTax1Code                  { get; set; }
        public virtual string   LocalTax2Desc                  { get; set; }
        public virtual decimal? LocalTax2Wages                 { get; set; }
        public virtual decimal? LocalTax2Amount                { get; set; }
        public virtual string   LocalTax2Code                  { get; set; }
        public virtual string   Box12ACode                     { get; set; }
        public virtual string   Box12BCode                     { get; set; }
        public virtual string   Box12CCode                     { get; set; }
        public virtual string   Box12DCode                     { get; set; }
        public virtual decimal? Box12AAmount                   { get; set; }
        public virtual decimal? Box12BAmount                   { get; set; }
        public virtual decimal? Box12CAmount                   { get; set; }
        public virtual decimal? Box12DAmount                   { get; set; }
        public virtual decimal? TotalDeferredComp              { get; set; }
        public virtual decimal? D401KAmount                    { get; set; }
        public virtual decimal? D403BAmount                    { get; set; }
        public virtual decimal? D408KAmount                    { get; set; }
        public virtual decimal? D457BAmount                    { get; set; }
        public virtual decimal? D501CAmount                    { get; set; }
        public virtual decimal? HsaAmount                      { get; set; }
        public virtual decimal? ExcessInsuranceAmount          { get; set; }
        public virtual decimal? Roth401KAmount                 { get; set; }
        public virtual decimal? Roth403BAmount                 { get; set; }
        public virtual string   OtherInfo1Desc                 { get; set; }
        public virtual decimal? OtherInfo1Amount               { get; set; }
        public virtual string   OtherInfo2Desc                 { get; set; }
        public virtual decimal? OtherInfo2Amount               { get; set; }
        public virtual string   OtherInfo3Desc                 { get; set; }
        public virtual decimal? OtherInfo3Amount               { get; set; }
        public virtual string   OtherInfo4Desc                 { get; set; }
        public virtual decimal? OtherInfo4Amount               { get; set; }
        public virtual string   OtherInfo5Desc                 { get; set; }
        public virtual decimal? OtherInfo5Amount               { get; set; }
        public virtual int?     W2Year                         { get; set; }
        public virtual bool?    IsStateTaxAdd401K              { get; set; }
        public virtual bool?    IsStateTaxFlexTaxable          { get; set; }
        public virtual string   LocalTax3Desc                  { get; set; }
        public virtual decimal? LocalTax3Wages                 { get; set; }
        public virtual decimal? LocalTax3Amount                { get; set; }
        public virtual string   LocalTax3Code                  { get; set; }
        public virtual string   LocalTax4Desc                  { get; set; }
        public virtual decimal? LocalTax4Wages                 { get; set; }
        public virtual decimal? LocalTax4Amount                { get; set; }
        public virtual string   LocalTax4Code                  { get; set; }
        public virtual bool?    IsLocalTax1Add401K             { get; set; }
        public virtual bool?    IsLocalTax2Add401K             { get; set; }
        public virtual bool?    IsLocalTax3Add401K             { get; set; }
        public virtual bool?    IsLocalTax4Add401K             { get; set; }
        public virtual bool?    IsLocalTax1FlexTaxable         { get; set; }
        public virtual bool?    IsLocalTax2FlexTaxable         { get; set; }
        public virtual bool?    IsLocalTax3FlexTaxable         { get; set; }
        public virtual bool?    IsLocalTax4FlexTaxable         { get; set; }
        public virtual string   Ew2Consent                     { get; set; }
        public virtual string   StateTaxStatePostalNumericCode { get; set; }
        public virtual string   StateTaxStateTaxIdNumber       { get; set; }
        public virtual decimal? MovingExpenseCodePAmount       { get; set; }
        public virtual decimal? HireActWagesAmount             { get; set; }
        public virtual decimal? NonTaxableSickPayAmount        { get; set; }
        public virtual decimal? YtdGrossPayOnly                { get; set; }
        public virtual string   StateTaxEmployerTId            { get; set; }
        public virtual string   StateTaxEmployerTidLocation    { get; set; }
        public virtual decimal  GroupTermLife                  { get; set; }
        public virtual decimal  SponsoredHealthIns             { get; set; }
        public virtual decimal  Roth457B                       { get; set; }
        public virtual decimal  D409AAmount                    { get; set; }
        public virtual bool     IsActive                       { get; set; }
        public virtual decimal  AdoptionExpensesT              { get; set; }
        public virtual int      GenW2EmployeeHistoryId         { get; set; }
        public virtual int      RowCountId                     { get; set; }
        public virtual Client   Client                         { get; set; }

        public GenW2EmployeeHistory()
        {
        }
    }
}
