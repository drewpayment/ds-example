using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientDuplicateOptionsDto
    {
        public bool CopyBenefitAdminInfo  { get; set; }
        public bool CopyDepartments       { get; set; }
        public bool CopyCostCenters       { get; set; }
        public bool CopyCompanyContacts   { get; set; }
        public bool CopyGeneralLedger     { get; set; }
        public bool CopyCompanyOptions    { get; set; }
        public bool CopyCompanySecurity   { get; set; }
        public bool CopyCompanyAccruals   { get; set; }
        public bool CopyCompanyPlans      { get; set; }
        public bool CopyCompanyTaxes      { get; set; }
        public bool CopyReportScheduling  { get; set; }
        public bool CopyVendors           { get; set; }
        public bool CopyCompanySchedules  { get; set; }
        public bool CopyTimePolicies      { get; set; }
        public bool CopyApplicantTracking { get; set; }
    }
}
