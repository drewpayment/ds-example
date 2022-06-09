using System;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.Yardi
{
    /// <summary>
    /// Layout for YardiLearningExport rows.
    /// </summary>
    public class YardiLearningExportLayout
    {
        public static readonly string Delimiter = ",";
        public static readonly string DelimiterReplacement = " ";

        /// <summary>
        /// Column A: First Name
        /// </summary>
        public string Column_A_FirstName { get; set; }

        /// <summary>
        /// Column B: Last Name
        /// </summary>
        public string Column_B_LastName { get; set; }

        /// <summary>
        /// Column C: Email: (User profile email)
        /// </summary>
        public string Column_C_Email { get; set; }

        /// <summary>
        /// Column D: Username: (User profile email)
        /// </summary>
        public string Column_D_Username { get; set; }

        /// <summary>
        /// Column E: Hire Date: (Rehire Date if present else Hire Date)
        /// </summary>
        public DateTime Column_E_HireDate { get; set; }

        /// <summary>
        /// Column F: Job Title: (Job Title) 
        /// - Create a check box option to mark a job title on the job profile.
        /// - Only job titles marked will be included in the Yardi integration file.
        /// </summary>
        public string Column_F_JobTitle { get; set; }

        /// <summary>
        /// Column G: Department: (Department)
        /// </summary>
        public string Column_G_Department { get; set; }

        /// <summary>
        /// Column H: Property: (Division)
        /// </summary>
        public string Column_H_Property { get; set; }

        /// <summary>
        /// Column I: Status: (Any Dominion Active Status should come over as (Active) Any Inactive status comes over as (Inactive)
        /// </summary>
        public bool Column_I_IsActiveStatus { get; set; }

        /// <summary>
        /// Column J: Employee ID: (Employee Number)
        /// </summary>
        public string Column_J_EmployeeNumber { get; set; }

        /// <summary>
        /// Column K: Reports To Name (Direct Supervisor)
        /// </summary>
        public string Column_K_DirectSupervisorFirstNameLastName { get; set; }

        /// <summary>
        /// Column L: Reports To Email (Supervisor profile email)
        /// </summary>
        public string Column_L_DirectSupervisorEmail { get; set; }
    }
}
