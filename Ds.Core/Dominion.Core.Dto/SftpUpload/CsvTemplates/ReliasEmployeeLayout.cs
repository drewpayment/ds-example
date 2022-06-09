using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    public class ReliasEmployeeLayout
    {
        //public int user00_employeeId { get; set; }

        /// <summary>
        /// USER:00 USER         char(4)
        /// </summary>
        public string user00_user { get; set; }
        /// <summary>
        /// USER:01 5532
        /// </summary>
        public string user01_orgId { get; set; }
        /// <summary>
        /// USER:02 ASD5532   varchar(6)
        /// </summary>
        public string user02_orgCode { get; set; }
        /// <summary>
        /// USER:03 Y        nvarchar(50)
        /// </summary>
        public string user03_lastName { get; set; }
        /// <summary>
        /// USER:04 Y       nvarchar(50)
        /// </summary>
        public string user04_firstName { get; set; }
        /// <summary>
        /// USER:05 first initial of first name first initial of last name and last four of social (Ex. MD1234)  nvarchar(50)
        /// </summary>
        public string user05_userName { get; set; }
        /// <summary>
        /// USER:06 password     nvarchar(50)
        /// </summary>
        public string user06_password { get; set; }
        /// <summary>
        /// USER:07 N- Blank         nvarchar(50)
        /// </summary>
        public string user07_guid { get; set; }
        /// <summary>
        /// USER:08 N- Blank   nvarchar(50)
        /// </summary>
        public string user08_employeeId { get; set; }
        /// <summary>
        /// USER:09 Y – most recent  smalldatetime
        /// </summary>
        public DateTime? user09_hireDate { get; set; }
        /// <summary>
        /// USER:10 Y         smalldatetime
        /// </summary>
        public DateTime? user10_terminationDate { get; set; }
        /// <summary>
        /// USER:11 Y               nvarchar(150)
        /// </summary>
        public string user11_email { get; set; }
        /// <summary>
        /// USER:12 Y            nvarchar(100)
        /// </summary>
        public string user12_jobTitle { get; set; }
        /// <summary>
        /// USER:13 Y          nvarchar(100)
        /// </summary>
        public string user13_department { get; set; }
        /// <summary>
        /// USER:14 Y – AHP1 (codes) nvarchar(110)
        /// </summary>
        public string user14_organization { get; set; }
        /// <summary>
        /// USER:15 Y - Division nvarchar(100)
        /// </summary>
        public string user15_location { get; set; }
        /// <summary>
        /// USER:16 N - Blank   nvarchar(25)
        /// </summary>
        public string user16_workPhone { get; set; }
        /// <summary>
        /// USER:17 N - Blank nvarchar(25)
        /// </summary>
        public string user17_credentials { get; set; }
        /// <summary>
        /// USER:18 N - Blank         nvarchar(25)
        /// </summary>
        public string user18_fax { get; set; }
        /// <summary>
        /// USER:19 N - Blank     nvarchar(255)
        /// </summary>
        public string user19_address { get; set; }
        /// <summary>
        /// USER:20 N - Blank    nvarchar(255)
        /// </summary>
        public string user20_address2 { get; set; }
        /// <summary>
        /// USER:21 N - Blank        nvarchar(255)
        /// </summary>
        public string user21_city { get; set; }
        /// <summary>
        /// USER:22 N - Blank       char(2)
        /// </summary>
        public string user22_state { get; set; }
        /// <summary>
        /// USER:23 N - Blank     nvarchar(255)
        /// </summary>
        public string user23_country { get; set; }
        /// <summary>
        /// USER:24 N - Blank  nvarchar(10)
        /// </summary>
        public string user24_postalCode { get; set; }
        /// <summary>
        /// USER:25 Y (0, 1, or 2)   tinyint
        /// 
        /// 0=inactive, 1=active, 2= onleave Note: Users cannot be
        /// created with a status of 0 or 2. They must first be added
        /// as active(1) and then have the status adjusted to
        /// inactive or onleave.
        /// </summary>
        public byte user25_active { get; set; }
        /// <summary>
        /// USER:26 Y – Please see additional documents
        /// int 
        /// </summary>
        public string user26_hierarchyId { get; set; }
        /// <summary>
        /// USER:27 N - Blank
        /// int?
        /// </summary>
        public string user27_udf1 { get; set; }
        /// <summary>
        /// USER:28 N - Blank
        /// int?
        /// </summary>
        public string user28_udf2 { get; set; }
        /// <summary>
        /// USER:29 N - Blank
        /// int?
        /// </summary>
        public string user29_udf3 { get; set; }
        /// <summary>
        /// USER:30 N - 0    tinyint
        /// </summary>
        public byte user30_restricted { get; set; }
        /// <summary>
        /// USER:31 N - Blank  nvarchar(255)
        /// </summary>
        public string user31_userCategories { get; set; }
        /// <summary>
        /// USER:32 N - Blank
        /// int?
        /// </summary>
        public string user32_hideFromMaster { get; set; }
        /// <summary>
        /// USER:33 Y- 1
        /// </summary>
        public int user33_canElect { get; set; }
        /// <summary>
        /// USER:34 Blank
        /// int?
        /// </summary>
        public string user34_selfCompletionMode { get; set; }
        /// <summary>
        /// USER:35 Blank
        /// int?
        /// </summary>
        public string user35_externalRequestMode { get; set; }
        /// <summary>
        /// USER:36 N - Blank
        /// char?
        /// </summary>
        public string user36_gender { get; set; }
        /// <summary>
        /// USER:37 N – Blank   nvarchar(50)
        /// </summary>
        public string user37_ethnicity { get; set; }
        /// <summary>
        /// USER:38 N - Blank nvarchar(100)
        /// </summary>
        public string user38_employmentTypes { get; set; }
        /// <summary>
        /// USER:39 Blank
        /// </summary>
        public DateTime? user39_dateOfBirth { get; set; }
        /// <summary>
        /// USER:40 Blank  nvarchar(50)
        /// </summary>
        public string user40_middleName { get; set; }
        /// <summary>
        /// USER:41 Blank
        /// int?
        /// </summary>
        public string user41_administrator { get; set; }
        /// <summary>
        /// USER:42 Blank
        /// int?
        /// </summary>
        public string user42_instructor { get; set; }
        /// <summary>
        /// USER:43 Blank
        /// char?
        /// </summary>
        public string user43_supervisor { get; set; }
        /// <summary>
        /// USER:44 Blank
        /// char?
        /// </summary>
        public string user44_observer { get; set; }
        /// <summary>
        /// USER:45 Blank
        /// int?
        /// </summary>
        public string user45_performanceOptimizerReporter { get; set; }
        /// <summary>
        /// USER:46 1
        /// </summary>
        public string user46_requirePasswordChangeOnNextLogin { get; set; }
        /// <summary>
        /// USER:47 Blank
        /// int?
        /// </summary>
        public string user47_exemptStatus { get; set; }
        /// <summary>
        /// USER:48 Blank  varchar(255)
        /// </summary>
        public string user48_additionalHierarchyIds { get; set; }
        /// <summary>
        /// USER:49 Blank
        /// int?
        /// </summary>
        public string user49_comparisonGroup { get; set; }
    }
}
