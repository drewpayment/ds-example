using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.ComPsychExport
{
    public class ComPsychExportLayout
    {
        /*
        BETWEEN Field #'s (26	Z	Work Phone Extension) and (27	AA	Business Unit)
        "The following 10 fields allow you to segment employees into groups.  These groups can be used to drive the leave administration processes, assist a STD vendor partner's disability administration, specify letter and/or report distribution, and can be included on AbsencePro reports distributed back to the Company.  The first two fields have fixed field names and should be used if you organize employees by Business Unit or Organization.  These two fields are the main Employee Group identifiers that the Company may extract segmented leave reports from the AbsencePro database.

        The Company can assign employee statuses and other unique identifiers to the eight Employee Group fields.  If the Employee Group fields are being utilized for something other than the AbsencePro recommended field definition, the field names must be defined in account setup discussions with the designated AbsencePro Implementation Expert.  Each of these 10 fields can have unique values in the eligibility file to assign employees to your specific business structures or groups."																																													
        */


        /// <summary>
        /// "n": 1,
        /// "#": "A",
        /// "Field (Header) Name": "Account Number (KEY DATA ELEMENT)",
        /// "Field Requirement": "Required",
        /// "Field Type": "Number",
        /// "Size (Max)": 7,
        /// "Valid / Preferred Format": "nnnnnnn",
        /// "Example(s)": 9999999,
        /// "Field Notes": "AbsencePro's unique identifier for the Company account(s). The designated AbsencePro Implementation Expert will provide account number. The provided number needs to be included for each employee record on the file.",
        /// "Overwriting Fields": "Special:  If left blank, file will be rejected",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is the same for all employees. Just put this number in this column every time for every employee.
        /// </remarks>
        [Description("Account Number")]
        public string Column_01_A_Account_Number_KEY_DATA_ELEMENT { get; set; }

        /// <summary>
        /// "n": 2,
        /// "#": "B",
        /// "Field (Header) Name": "Account Name",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "Company Name",
        /// "Example(s)": "ABC Company",
        /// "Field Notes": "Text identifier of the company name.  (i.e. legal company name or recognizable abbreviation of the company name) ",
        /// "Overwriting Fields": "Special:  If left blank, file will be rejected",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is the same for all employees. Just put this number in this column every time for every employee.
        /// </remarks>
        [Description("Account Name")]
        public string Column_02_B_Account_Name { get; set; }

        /// <summary>
        /// "n": 3,
        /// "#": "C",
        /// "Field (Header) Name": "Employee ID # (KEY DATA ELEMENT)",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "numeric or alpha-numeric",
        /// "Example(s)": "99999,or T99999",
        /// "Field Notes": "This is the unique identifier the company assign to each employee.  Value must meet all the 'Conditional' requirements.  The field is flagged as a Key Data Element explained in 1.4 Data Consistency section of the 'File Format Instructions'.",
        /// "Overwriting Fields": "Special: If left blank, employee record may be rejected",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Employee Number")]
        public string Column_03_C_Employee_ID__Number_KEY_DATA_ELEMENT { get; set; }

        /// <summary>
        /// "n": 4,
        /// "#": "D",
        /// "Field (Header) Name": "Employee SSN# (KEY DATA ELEMENT)",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Number",
        /// "Size (Max)": 11,
        /// "Valid / Preferred Format": "nnn-nn-nnnn",
        /// "Example(s)": "999-99-9999",
        /// "Field Notes": "This is an additional unique identifier of the employee record.   The field is flagged as a Key Data Element explained in 1.4 Data Consistency section of the 'File Format Instructions'.",
        /// "Overwriting Fields": "Special: If left blank, employee record may be rejected",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Employee Social Security Number")]
        public string Column_04_D_Employee_SSN_Number_KEY_DATA_ELEMENT { get; set; }

        /// <summary>
        /// "n": 5,
        /// "#": "E",
        /// "Field (Header) Name": "First Name",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "First letter of the name capitalized ",
        /// "Example(s)": "Jane",
        /// "Field Notes": "",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("First Name")]
        public string Column_05_E_First_Name { get; set; }

        /// <summary>
        /// "n": 6,
        /// "#": "F",
        /// "Field (Header) Name": "Middle Initial",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 1,
        /// "Valid / Preferred Format": "One Alpha Character",
        /// "Example(s)": "T",
        /// "Field Notes": "",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Middle Initial")]
        public string Column_06_F_Middle_Initial { get; set; }

        /// <summary>
        /// "n": 7,
        /// "#": "G",
        /// "Field (Header) Name": "Last Name",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "First letter of the name capitalized ",
        /// "Example(s)": "Doe",
        /// "Field Notes": "",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Last Name")]
        public string Column_07_G_Last_Name { get; set; }

        /// <summary>
        /// "n": 8,
        /// "#": "H",
        /// "Field (Header) Name": "Home Address Line #1",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "123 Michigan Ave",
        /// "Field Notes": "",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Home Address 1")]
        public string Column_08_H_Home_Address_Line_Number_1 { get; set; }

        /// <summary>
        /// "n": 9,
        /// "#": "I",
        /// "Field (Header) Name": "Home Address Line #2",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 65,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "Apt 12",
        /// "Field Notes": "If Company is unable to pull 'Home Address' as two separate fields, the 'Home Address Line #1' and the 'Home Address Line #2' fields may be combined in the 'Home Address Line 1' field (e.g. 123 Michigan Ave, Suite 13) as long as it does not exceed the character size maximum.",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// N/A
        /// </remarks>
        [Description("Home Address 2")]
        public string Column_09_I_Home_Address_Line_Number_2 { get; set; }

        /// <summary>
        /// "n": 10,
        /// "#": "J",
        /// "Field (Header) Name": "Home City",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 65,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "Chicago",
        /// "Field Notes": "",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Home City")]
        public string Column_10_J_Home_City { get; set; }

        /// <summary>
        /// "n": 11,
        /// "#": "K",
        /// "Field (Header) Name": "Home State / Providence",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 2,
        /// "Valid / Preferred Format": "State / Providence Abbreviation",
        /// "Example(s)": "IL",
        /// "Field Notes": "The value must match the official ISO list of state and province abbreviations",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Home State / Providence")]
        public string Column_11_K_Home_State__Providence { get; set; }

        /// <summary>
        /// "n": 12,
        /// "#": "L",
        /// "Field (Header) Name": "Home Zip / Postal Code",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 20,
        /// "Valid / Preferred Format": "Five-digit zip code or zip plus four including a hyphen",
        /// "Example(s)": "60606,or 60606-1234",
        /// "Field Notes": "Canadian postal codes is an acceptable value as long as the 'Home Country' field is completed.  ANA NAN where “A” represents an alphabetic character and “N” represents a numeric character w/ space in middle. (e.g. E4M 2X9)",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Home Zip / Postal Code")]
        public string Column_12_L_Home_Zip__Postal_Code { get; set; }

        /// <summary>
        /// "n": 13,
        /// "#": "M",
        /// "Field (Header) Name": "Home Country",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 3,
        /// "Valid / Preferred Format": "Three alphabetic characters",
        /// "Example(s)": "USA",
        /// "Field Notes": "The value must match the official ISO list of country abbreviations",
        /// "Overwriting Fields": "Special:  If left blank, overwrites USA",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is the same for all employees. Just put this number in this column every time for every employee.
        /// </remarks>
        [Description("Home Country")]
        public string Column_13_M_Home_Country { get; set; }

        /// <summary>
        /// "n": 14,
        /// "#": "N",
        /// "Field (Header) Name": "Home (or Primary) Phone #",
        /// "Field Requirement": "*Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 12,
        /// "Valid / Preferred Format": "nnn-nnn-nnnn",
        /// "Example(s)": "312-555-1234",
        /// "Field Notes": "Do not provide the leading “1” (ITU Country Calling Code) within the Home Phone #",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Home Phone Number")]
        public string Column_14_N_Home_or_Primary_Phone_Number { get; set; }

        /// <summary>
        /// "n": 15,
        /// "#": "O",
        /// "Field (Header) Name": "Gender",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 1,
        /// "Valid / Preferred Format": "F, M",
        /// "Example(s)": "F",
        /// "Field Notes": "",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Gender")]
        public string Column_15_O_Gender { get; set; }

        /// <summary>
        /// "n": 16,
        /// "#": "P",
        /// "Field (Header) Name": "Date of Birth",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Date",
        /// "Size (Max)": 10,
        /// "Valid / Preferred Format": "mm/dd/yyyy",
        /// "Example(s)": "1/8/1980",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Date of Birth")]
        public DateTime? Column_16_P_Date_of_Birth { get; set; }

        /// <summary>
        /// "n": 17,
        /// "#": "Q",
        /// "Field (Header) Name": "Primary Language",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 7,
        /// "Valid / Preferred Format": "Include the full language name (English, French, or Spanish)",
        /// "Example(s)": "English",
        /// "Field Notes": "The AbsencePro system will use Primary Language to determine the correct letters and attachments for employees.  We currently support only these three languages.",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Primary Language")]
        public string Column_17_Q_Primary_Language { get; set; }

        /// <summary>
        /// "n": 18,
        /// "#": "R",
        /// "Field (Header) Name": "Work Address ID",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 150,
        /// "Valid / Preferred Format": "A unique number or code which identifies the work location of the employee record",
        /// "Example(s)": "MA001554",
        /// "Field Notes": "If your payroll or HR system assigns a unique identifier for your work locations, please provide it to help maintain data integrity.",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work Address ID")]
        public string Column_18_R_Work_Address_ID { get; set; }

        /// <summary>
        /// "n": 19,
        /// "#": "S",
        /// "Field (Header) Name": "Work Address Line #1",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "1200 North Blvd",
        /// "Field Notes": "",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work Address 1")]
        public string Column_19_S_Work_Address_Line_Number_1 { get; set; }

        /// <summary>
        /// "n": 20,
        /// "#": "T",
        /// "Field (Header) Name": "Work Address Line #2",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 65,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "Suite 440",
        /// "Field Notes": "If Company is unable to pull 'Work Address' as two separate fields, the 'Work Address Line #1' and the 'Work Address Line #2' fields may be combined in the 'Work Address Line 1' field (e.g. 123 Michigan Ave, Suite 13) as long as it does not exceed the character size maximum.",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work Address 2")]
        public string Column_20_T_Work_Address_Line_Number_2 { get; set; }

        /// <summary>
        /// "n": 21,
        /// "#": "U",
        /// "Field (Header) Name": "Work City",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 65,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "Evanston",
        /// "Field Notes": "",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is the same for all employees. Just put this number in this column every time for every employee.
        /// </remarks>
        [Description("Work City")]
        public string Column_21_U_Work_City { get; set; }

        /// <summary>
        /// "n": 22,
        /// "#": "V",
        /// "Field (Header) Name": "Work State / Providence",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 2,
        /// "Valid / Preferred Format": "State / Providence Abbreviation",
        /// "Example(s)": "IL",
        /// "Field Notes": "The value must match the official ISO list of state and province abbreviations",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is the same for all employees. Just put this number in this column every time for every employee.
        /// </remarks>
        [Description("Work State / Providence")]
        public string Column_22_V_Work_State_Providence { get; set; }

        /// <summary>
        /// "n": 23,
        /// "#": "W",
        /// "Field (Header) Name": "Work Zip",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 20,
        /// "Valid / Preferred Format": "Five-digit zip code or zip plus four including a hyphen",
        /// "Example(s)": "60011,or 60011-1234",
        /// "Field Notes": "Canadian postal codes is an acceptable value as long as the 'Home Country' field is completed.  ANA NAN where “A” represents an alphabetic character and “N” represents a numeric character w/ space in middle. (e.g. E4M 2X9)",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work Zip / Postal Code")]
        public string Column_23_W_Work_Zip { get; set; }

        /// <summary>
        /// "n": 24,
        /// "#": "X",
        /// "Field (Header) Name": "Work Country",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 3,
        /// "Valid / Preferred Format": "Three alphabetic characters",
        /// "Example(s)": "USA",
        /// "Field Notes": "The value must match the official ISO list of country abbreviations",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work Country")]
        public string Column_24_X_Work_Country { get; set; }

        /// <summary>
        /// "n": 25,
        /// "#": "Y",
        /// "Field (Header) Name": "Work Phone #",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 12,
        /// "Valid / Preferred Format": "nnn-nnn-nnnn",
        /// "Example(s)": "877-855-1000",
        /// "Field Notes": "Do not provide the leading “1” (ITU Country Calling Code) within the Work Phone #",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work Phone Number")]
        public string Column_25_Y_Work_Phone_Number { get; set; }

        /// <summary>
        /// "n": 26,
        /// "#": "Z",
        /// "Field (Header) Name": "Work Phone Extension",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 20,
        /// "Valid / Preferred Format": "nnnn",
        /// "Example(s)": 2563,
        /// "Field Notes": "",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work Phone Extension")]
        public string Column_26_Z_Work_Phone_Extension { get; set; }

        /// <summary>
        /// "n": 27,
        /// "#": "AA",
        /// "Field (Header) Name": "Business Unit",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "Sales",
        /// "Field Notes": "The field is typically used as the employee's \"department\".  The Business Unit is a field that the Company will be able to extract leave reports from the AbsencePro system per unique value provided.  The field value may also be  used to distribute leave info and establish document parameters for leave letters.  ",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is the HOME COST CENTER Description in Dominion
        /// </remarks>
        [Description("Business Unit")]
        public string Column_27_AA_Business_Unit { get; set; }

        /// <summary>
        /// "n": 28,
        /// "#": "AB",
        /// "Field (Header) Name": "Organization",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "Midwest Region",
        /// "Field Notes": "The field is typically used as the employee's location or region.  The Organization is a field that the Company will be able to extract leave reports from the AbsencePro system per unique value provided.  The field value may also be used to distribute leave info and establish document parameters for leave letters.  ",
        /// "Overwriting Fields": "*N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Organization")]
        public string Column_28_AB_Organization { get; set; }

        /// <summary>
        /// "n": 29,
        /// "#": "AC",
        /// "Field (Header) Name": "Employee Group #1",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 1")]
        public string Column_29_AC_Employee_Group_Number_1 { get; set; }

        /// <summary>
        /// "n": 30,
        /// "#": "AD",
        /// "Field (Header) Name": "Employee Group #2",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 2")]
        public string Column_30_AD_Employee_Group_Number_2 { get; set; }

        /// <summary>
        /// "n": 31,
        /// "#": "AE",
        /// "Field (Header) Name": "Employee Group #3",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 3")]
        public string Column_31_AE_Employee_Group_Number_3 { get; set; }

        /// <summary>
        /// "n": 32,
        /// "#": "AF",
        /// "Field (Header) Name": "Employee Group #4",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "The field will be defined by the Company, AbsencePro, and/or STD Disability Vendor Partner.",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 4")]
        public string Column_32_AF_Employee_Group_Number_4 { get; set; }

        /// <summary>
        /// "n": 33,
        /// "#": "AG",
        /// "Field (Header) Name": "Employee Group #5",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "The field will be defined by the Company, AbsencePro, and/or STD Disability Vendor Partner.",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 5")]
        public string Column_33_AG_Employee_Group_Number_5 { get; set; }

        /// <summary>
        /// "n": 34,
        /// "#": "AH",
        /// "Field (Header) Name": "Employee Group #6",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "The field will be defined by the Company, AbsencePro, and/or STD Disability Vendor Partner.",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 6")]
        public string Column_34_AH_Employee_Group_Number_6 { get; set; }

        /// <summary>
        /// "n": 35,
        /// "#": "AI",
        /// "Field (Header) Name": "Employee Group #7",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "The field will be defined by the Company, AbsencePro, and/or STD Disability Vendor Partner.",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 7")]
        public string Column_35_AI_Employee_Group_Number_7 { get; set; }

        /// <summary>
        /// "n": 36,
        /// "#": "AJ",
        /// "Field (Header) Name": "Employee Group #8",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 100,
        /// "Valid / Preferred Format": "{Defined by Company}",
        /// "Example(s)": "The field will be defined by the Company, AbsencePro, and/or STD Disability Vendor Partner.",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employee Group 8")]
        public string Column_36_AJ_Employee_Group_Number_8 { get; set; }

        /// <summary>
        /// "n": 37,
        /// "#": "AK",
        /// "Field (Header) Name": "Full Time / Part Time Status",
        /// "Field Requirement": "Required",
        /// "Field Type": "Text",
        /// "Size (Max)": 1,
        /// "Valid / Preferred Format": "F, P",
        /// "Example(s)": "F",
        /// "Field Notes": "Although the AbsencePro database only houses only a F or P status, AbsencePro understands the Company may have other statuses (i.e. Temp, Per Diem, etc.).  If the actual status is necessary to the leave administration, it may be placed in one of the Employee Group fields above.",
        /// "Overwriting Fields": "Special: if left blank, overwrites “F”",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Full Time / Part-Time Status")]
        public string Column_37_AK_Full_Time__Part_Time_Status { get; set; }

        /// <summary>
        /// "n": 38,
        /// "#": "AL",
        /// "Field (Header) Name": "Hire Date",
        /// "Field Requirement": "Required",
        /// "Field Type": "Date",
        /// "Size (Max)": 10,
        /// "Valid / Preferred Format": "mm/dd/yyyy",
        /// "Example(s)": "9/15/2004",
        /// "Field Notes": "Fields 38 &amp; 39 assist in determining eligibility for leave and is dependent upon the data the Company houses in their HRIS / Payroll system.  For the most accurate leave eligibility determination, AbsencePro prefers to receive both fields if the Company has record of an employees possible break in employment (Original Hire Date (#38), Rehire Date (#39)).  PLEASE NOTE:  These fields are utilized to determine months/years of service with the Company.  By no means is it to provide record of the employees status (position or location) changes.",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Hire Date")]
        public DateTime? Column_38_AL_Hire_Date { get; set; }

        /// <summary>
        /// "n": 39,
        /// "#": "AM",
        /// "Field (Header) Name": "Adjusted Employment Date",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Date",
        /// "Size (Max)": 10,
        /// "Valid / Preferred Format": "mm/dd/yyyy",
        /// "Example(s)": "4/6/2010",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Adjusted Employment Date")]
        public DateTime? Column_39_AM_Adjusted_Employment_Date { get; set; }

        /// <summary>
        /// "n": 40,
        /// "#": "AN",
        /// "Field (Header) Name": "Anniversary Date",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Date",
        /// "Size (Max)": 10,
        /// "Valid / Preferred Format": "mm/dd/yyyy",
        /// "Example(s)": "9/15/2005",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Anniversary Date")]
        public DateTime? Column_40_AN_Anniversary_Date { get; set; }

        /// <summary>
        /// "n": 41,
        /// "#": "AO",
        /// "Field (Header) Name": "Termination Date",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Date",
        /// "Size (Max)": 10,
        /// "Valid / Preferred Format": "mm/dd/yyyy",
        /// "Example(s)": "7/10/2013",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion. If they don't have one, leave it blank
        /// </remarks>
        [Description("Termination Date")]
        public DateTime? Column_41_AO_Termination_Date { get; set; }

        /// <summary>
        /// "n": 42,
        /// "#": "AP",
        /// "Field (Header) Name": "Hours Wkd in the Previous 12 Months",
        /// "Field Requirement": "*Required",
        /// "Field Type": "Number",
        /// "Size (Max)": 11,
        /// "Valid / Preferred Format": "nnnn.nn,or nnnn",
        /// "Example(s)": "1624.00 or 1624",
        /// "Field Notes": "A 12 month rolling look-back of actual hours worked by the employee.  This field is vital to determining eligibility for leave.  Please be informed that AbsencePro cannot be held responsible for eligibility determinations that are made based on out-of-date, incomplete and/or inaccurate data.",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// For this field, in my custom report, I have to set my limiting clause to be 
        /// <c>AND CHECKDATE &gt;= [check date that was 52 weeks ago] AND CHECKDATE &lt;= CURRENT</c>, 
        /// and I use a custom field called "Worked Hours" by combining REG and OVT 
        /// (and holiday worked and doubletime if they have those earnings)
        /// </remarks>
        [Description("Hours Worked Previous 12 Months")]
        public double? Column_42_AP_Hours_Wkd_in_the_Previous_12_Months { get; set; }

        /// <summary>
        /// "n": 43,
        /// "#": "AQ",
        /// "Field (Header) Name": "Key Employee",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 3,
        /// "Valid / Preferred Format": "Yes\", \"No",
        /// "Example(s)": "Yes",
        /// "Field Notes": "A ‘‘key employee’’ is a salaried FMLA-eligible employee who is among the highest paid 10 percent of all the employees employed by the employer within 75 miles of the employee’s worksite. If the Company reviews \"key employees\" protection rights for each leave, this is preferred field.",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Key Employee")]
        public string Column_43_AQ_Key_Employee { get; set; }

        /// <summary>
        /// "n": 44,
        /// "#": "AR",
        /// "Field (Header) Name": "Job Title",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "Sales Executive",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// From field in Dominion
        /// </remarks>
        [Description("Job Title")]
        public string Column_44_AR_Job_Title { get; set; }

        /// <summary>
        /// "n": 45,
        /// "#": "AS",
        /// "Field (Header) Name": "Work E-mail Address",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 80,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "j.doe@abccompany.com",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Work E-mail Address")]
        public string Column_45_AS_Work_Email_Address { get; set; }

        /// <summary>
        /// "n": 46,
        /// "#": "AT",
        /// "Field (Header) Name": "Scheduled Hours per Week",
        /// "Field Requirement": "Required",
        /// "Field Type": "Number",
        /// "Size (Max)": 6,
        /// "Valid / Preferred Format": "nnn.nn or nnn",
        /// "Example(s)": 37.5,
        /// "Field Notes": "AbsencePro's standard operating procedure is to confirm the employee's schedule with the employee when requesting a leave.",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Look at column AK. If they are FT, put 40, if PT, put 24.
        /// </remarks>
        [Description("Scheduled Hours per Week")]
        public double Column_46_AT_Scheduled_Hours_per_Week { get; set; }

        /// <summary>
        /// "n": 47,
        /// "#": "AU",
        /// "Field (Header) Name": "Scheduled # of Days per Week",
        /// "Field Requirement": "Required",
        /// "Field Type": "Number",
        /// "Size (Max)": 1,
        /// "Valid / Preferred Format": "n",
        /// "Example(s)": 5,
        /// "Field Notes": "AbsencePro's standard operating procedure is to confirm the employee's schedule with the employee when requesting a leave.",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Look at column AK. If they are FT, put 5, if PT, put 3.
        /// </remarks>
        [Description("Scheduled Number of Days per Week")]
        public int Column_47_AU_Scheduled__Number_of_Days_per_Week { get; set; }

        /// <summary>
        /// "n": 48,
        /// "#": "AV",
        /// "Field (Header) Name": "Additional Letter Recipient E-mail",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 200,
        /// "Valid / Preferred Format": "",
        /// "Example(s)": "johndoe@tpa.com",
        /// "Field Notes": "This field may be populated if the employee's leave letters needs to be sent to a third party, such as the employee's lawyer.  Most companies will not have this data in their HRIS / Payroll system.  Therefore,  it may be requested to be added to the employee's account on a case by case basis.",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Additional Letter Recipient E-mail Addresses")]
        public string Column_48_AV_Additional_Letter_Recipient_Email { get; set; }

        /// <summary>
        /// "n": 49,
        /// "#": "AW",
        /// "Field (Header) Name": "Spouse Employed ",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 3,
        /// "Valid / Preferred Format": "Yes, No",
        /// "Example(s)": "Yes",
        /// "Field Notes": "Fields 49 & 50 assist in combining FMLA entitlement.  If both the employee and their spouse works for the Company and the Company combines their entitlement for situations defined by the DOL, this is preferred data to link the two employee records / entitlement.  Commonly unable to be provided therefore, Company may address relationship on a case by case basis.",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Spouse Employed at the Employee's Account")]
        public string Column_49_AW_Spouse_Employed_ { get; set; }

        /// <summary>
        /// "n": 50,
        /// "#": "AX",
        /// "Field (Header) Name": "Spouse Employee ID",
        /// "Field Requirement": "Optional",
        /// "Field Type": "Text",
        /// "Size (Max)": 50,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "77777,or T77777",
        /// "Field Notes": "",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "N"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Spouse Employee ID")]
        public string Column_50_AX_Spouse_Employee_ID { get; set; }

        /// <summary>
        /// "n": 51,
        /// "#": "AY",
        /// "Field (Header) Name": "Employer Contact 1 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 1 IDS")]
        public string Column_51_AY_Employer_Contact_1_IDS { get; set; }

        /// <summary>
        /// "n": 52,
        /// "#": "AZ",
        /// "Field (Header) Name": "STD Earnings",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Number",
        /// "Size (Max)": 8,
        /// "Valid / Preferred Format": "nnnnnn.nn,or nnnnnn",
        /// "Example(s)": 55000,
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// For this field, in my custom report, I have to set my limiting clause to be 
        /// <c>AND CHECKDATE &gt;= [check date that was 52 weeks ago] AND CHECKDATE &lt;= CURRENT</c>, 
        /// and I use the <c>GROSS PAY</c> field to sum up their gross pay for the last year.
        /// </remarks>
        [Description("STD Earnings")]
        public decimal? Column_52_AZ_STD_Earnings { get; set; }

        /// <summary>
        /// "n": 53,
        /// "#": "BA",
        /// "Field (Header) Name": "STD Eligibility Date",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Date",
        /// "Size (Max)": 10,
        /// "Valid / Preferred Format": "mm/dd/yyyy",
        /// "Example(s)": "9/15/2004",
        /// "Field Notes": "The field provides an indicator to AbsencePro if the employee has a STD benefit.",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is a formula I wrote that simply pulls the EXACT 3 month date from their hire date
        /// </remarks>
        [Description("STD Eligibility Date")]
        public DateTime? Column_53_BA_STD_Eligibility_Date => _Column_38_AL_Hire_Date_PlusThreeMonths;

        /// <summary>
        /// "n": 54,
        /// "#": "BB",
        /// "Field (Header) Name": "LTD Eligibility Date",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Date",
        /// "Size (Max)": 10,
        /// "Valid / Preferred Format": "mm/dd/yyyy",
        /// "Example(s)": "9/15/2004",
        /// "Field Notes": "",
        /// "Overwriting Fields": "N",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// This is a formula I wrote that simply pulls the EXACT 3 month date from their hire date
        /// </remarks>
        [Description("LTD Eligibility Date")]
        public DateTime? Column_54_BB_LTD_Eligibility_Date => _Column_38_AL_Hire_Date_PlusThreeMonths;

        /// <summary>
        /// "n": 55,
        /// "#": "BC",
        /// "Field (Header) Name": "Employer Contact 2 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 2 IDS")]
        public string Column_55_BC_Employer_Contact_2_IDS { get; set; }

        /// <summary>
        /// "n": 56,
        /// "#": "BD",
        /// "Field (Header) Name": "Employer Contact 3 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 3 IDS")]
        public string Column_56_BD_Employer_Contact_3_IDS { get; set; }

        /// <summary>
        /// "n": 57,
        /// "#": "BE",
        /// "Field (Header) Name": "Employer Contact 4 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 4 IDS")]
        public string Column_57_BE_Employer_Contact_4_IDS { get; set; }

        /// <summary>
        /// "n": 58,
        /// "#": "BF",
        /// "Field (Header) Name": "Employer Contact 5 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 5 IDS")]
        public string Column_58_BF_Employer_Contact_5_IDS { get; set; }

        /// <summary>
        /// "n": 59,
        /// "#": "BG",
        /// "Field (Header) Name": "Employer Contact 6 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 6 IDS")]
        public string Column_59_BG_Employer_Contact_6_IDS { get; set; }

        /// <summary>
        /// "n": 60,
        /// "#": "BH",
        /// "Field (Header) Name": "Employer Contact 7 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 7 IDS")]
        public string Column_60_BH_Employer_Contact_7_IDS { get; set; }

        /// <summary>
        /// "n": 61,
        /// "#": "BI",
        /// "Field (Header) Name": "Employer Contact 8 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 8 IDS")]
        public string Column_61_BI_Employer_Contact_8_IDS { get; set; }

        /// <summary>
        /// "n": 62,
        /// "#": "BJ",
        /// "Field (Header) Name": "Employer Contact 9 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 9 IDS")]
        public string Column_62_BJ_Employer_Contact_9_IDS { get; set; }

        /// <summary>
        /// "n": 63,
        /// "#": "BK",
        /// "Field (Header) Name": "Employer Contact 10 IDS",
        /// "Field Requirement": "Conditional",
        /// "Field Type": "Text",
        /// "Size (Max)": 250,
        /// "Valid / Preferred Format": "numeric, or alpha-numeric",
        /// "Example(s)": "88888,or T88888",
        /// "Field Notes": "EmployeeIDs if multiple separate with ;",
        /// "Overwriting Fields": "Y",
        /// "STD Intake Fields": "Y"
        /// </summary>
        /// <remarks>
        /// David's notes:
        /// Leave this blank - it is optional, and we can't pull this from Dominion
        /// </remarks>
        [Description("Employer Contact 10 IDS")]
        public string Column_63_BK_Employer_Contact_10_IDS { get; set; }




        private DateTime? _Column_38_AL_Hire_Date_PlusThreeMonths => Column_38_AL_Hire_Date?.AddMonths(3);
    }
}


