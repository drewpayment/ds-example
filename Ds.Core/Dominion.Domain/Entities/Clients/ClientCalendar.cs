using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients.Calendar;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// Entity representation of the dbo.ClientCalendar table.
    /// </summary>
    public partial class ClientCalendar : Entity<ClientCalendar>
    {
        public virtual int       ClientId                       { get; set; } 
        public virtual byte      CalendarTypeId                 { get; set; } 
        public virtual byte?     CalendarDateId                 { get; set; } 
        public virtual byte?     CalendarAlt1Id                 { get; set; } 
        public virtual byte?     CalendarAlt2Id                 { get; set; } 
        public virtual DateTime? FiscalStart                    { get; set; } 
        public virtual DateTime? FiscalEnd                      { get; set; } 
        public virtual byte?     CalendarPickupId               { get; set; } 
        public virtual byte?     DeliveryMethodId               { get; set; } 
        public virtual byte?     CalendarDeliveryTimeId         { get; set; } 
        public virtual byte?     CalendarTaxTypeId              { get; set; } 
        public virtual byte?     CalendarChecksId               { get; set; } 
        public virtual byte?     CalendarTaxFileId              { get; set; } 
        public virtual byte?     CalendarPcDataId               { get; set; } 
        public virtual byte?     CalendarDebitId                { get; set; } 
        public virtual byte?     CalendarBillingId              { get; set; } 
        public virtual byte?     CalendarFrequencyWeeklyId      { get; set; } 
        public virtual byte?     CalendarFrequencyBiWeeklyId    { get; set; } 
        public virtual byte?     CalendarFrequencyAltWeekId     { get; set; } 
        public virtual byte?     CalendarFrequencySemiMonthlyId { get; set; } 
        public virtual byte?     CalendarFrequencyMonthlyId     { get; set; } 
        public virtual byte?     CalendarFrequencyQuarterlyId   { get; set; } 
        public virtual byte?     DayOfWeekPe                    { get; set; } 
        public virtual byte?     DayOfWeekChecks                { get; set; } 
        public virtual byte?     DayOfWeekAlt1                  { get; set; } 
        public virtual byte?     DayOfWeekAlt2                  { get; set; } 
        public virtual byte?     DayOfWeekPickup                { get; set; } 
        public virtual byte?     DayOfWeekDelivery              { get; set; } 
        public virtual byte?     FederalTaxFrequencyId          { get; set; } 
        public virtual DateTime? Modified                       { get; set; } 
        public virtual int?      ModifiedBy                     { get; set; } 
        public virtual byte      TaxManagementAchBankId         { get; set; } 
        public virtual byte?     WeeklyWeekStartDay             { get; set; } 
        public virtual byte?     BiWeeklyWeekStartDay           { get; set; } 
        public virtual byte?     AltBiWeeklyWeekStartDay        { get; set; } 
        public virtual byte?     SemiMonthlyWeekStartDay        { get; set; } 
        public virtual byte?     MonthlyWeekStartDay            { get; set; } 
        public virtual byte?     QuarterlyWeekStartDay          { get; set; } 

        //FOREIGN KEYS
        public virtual Client         Client         { get; set; }
        public virtual DeliveryMethod DeliveryMethod { get; set; } 
    }
}
