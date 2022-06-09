using Dominion.Core.Dto.EEOC.Enums;

namespace Dominion.Core.Dto.EEOC.EEO1.Comp1
{
    public class EEO1_Comp1_Flattened_Record
    {
        public string CompanyNumber { get; set; }
        public EEO1RecordStatus StatusCode { get; set; }
        public string UnitNumber { get; set; }
        public string UnitName { get; set; }
        public string UnitAddress1 { get; set; }
        public string UnitAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public bool IsHeadquarters { get; set; }
        public byte QuestionB2C { get; set; }
        public byte QuestionC1 { get; set; }
        public byte QuestionC2 { get; set; }
        public byte QuestionC3 { get; set; }
        public string DunAndBradstreetNumber { get; set; }
        public string County { get; set; }
        public int? QuestionD1 { get; set; }
        public int NaicsCode { get; set; }
        public string CertifyingOfficialTitle { get; set; }
        public string CertifyingOfficialName { get; set; }
        public string CertifyingOfficialPhone { get; set; }
        public string CertifyingOfficialEmail { get; set; }
        public int ExecutiveLevel_Hispanic_Male_Count { get; set; }
        public int ExecutiveLevel_Hispanic_Female_Count { get; set; }
        public int ExecutiveLevel_White_Male_Count { get; set; }
        public int ExecutiveLevel_Black_Male_Count { get; set; }
        public int ExecutiveLevel_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int ExecutiveLevel_Asian_Male_Count { get; set; }
        public int ExecutiveLevel_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int ExecutiveLevel_TwoOrMoreRaces_Male_Count { get; set; }
        public int ExecutiveLevel_White_Female_Count { get; set; }
        public int ExecutiveLevel_Black_Female_Count { get; set; }
        public int ExecutiveLevel_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int ExecutiveLevel_Asian_Female_Count { get; set; }
        public int ExecutiveLevel_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int ExecutiveLevel_TwoOrMoreRaces_Female_Count { get; set; }
        public int ExecutiveLevel_Total { get; set; }
        public int MidLevel_Hispanic_Male_Count { get; set; }
        public int MidLevel_Hispanic_Female_Count { get; set; }
        public int MidLevel_White_Male_Count { get; set; }
        public int MidLevel_Black_Male_Count { get; set; }
        public int MidLevel_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int MidLevel_Asian_Male_Count { get; set; }
        public int MidLevel_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int MidLevel_TwoOrMoreRaces_Male_Count { get; set; }
        public int MidLevel_White_Female_Count { get; set; }
        public int MidLevel_Black_Female_Count { get; set; }
        public int MidLevel_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int MidLevel_Asian_Female_Count { get; set; }
        public int MidLevel_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int MidLevel_TwoOrMoreRaces_Female_Count { get; set; }
        public int MidLevel_Total { get; set; }
        public int Professionals_Hispanic_Male_Count { get; set; }
        public int Professionals_Hispanic_Female_Count { get; set; }
        public int Professionals_White_Male_Count { get; set; }
        public int Professionals_Black_Male_Count { get; set; }
        public int Professionals_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int Professionals_Asian_Male_Count { get; set; }
        public int Professionals_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int Professionals_TwoOrMoreRaces_Male_Count { get; set; }
        public int Professionals_White_Female_Count { get; set; }
        public int Professionals_Black_Female_Count { get; set; }
        public int Professionals_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int Professionals_Asian_Female_Count { get; set; }
        public int Professionals_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int Professionals_TwoOrMoreRaces_Female_Count { get; set; }
        public int Professionals_Total { get; set; }
        public int Technicians_Hispanic_Male_Count { get; set; }
        public int Technicians_Hispanic_Female_Count { get; set; }
        public int Technicians_White_Male_Count { get; set; }
        public int Technicians_Black_Male_Count { get; set; }
        public int Technicians_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int Technicians_Asian_Male_Count { get; set; }
        public int Technicians_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int Technicians_TwoOrMoreRaces_Male_Count { get; set; }
        public int Technicians_White_Female_Count { get; set; }
        public int Technicians_Black_Female_Count { get; set; }
        public int Technicians_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int Technicians_Asian_Female_Count { get; set; }
        public int Technicians_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int Technicians_TwoOrMoreRaces_Female_Count { get; set; }
        public int Technicians_Total { get; set; }
        public int SalesWorkers_Hispanic_Male_Count { get; set; }
        public int SalesWorkers_Hispanic_Female_Count { get; set; }
        public int SalesWorkers_White_Male_Count { get; set; }
        public int SalesWorkers_Black_Male_Count { get; set; }
        public int SalesWorkers_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int SalesWorkers_Asian_Male_Count { get; set; }
        public int SalesWorkers_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int SalesWorkers_TwoOrMoreRaces_Male_Count { get; set; }
        public int SalesWorkers_White_Female_Count { get; set; }
        public int SalesWorkers_Black_Female_Count { get; set; }
        public int SalesWorkers_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int SalesWorkers_Asian_Female_Count { get; set; }
        public int SalesWorkers_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int SalesWorkers_TwoOrMoreRaces_Female_Count { get; set; }
        public int SalesWorkers_Total { get; set; }
        public int AdministrativeSupport_Hispanic_Male_Count { get; set; }
        public int AdministrativeSupport_Hispanic_Female_Count { get; set; }
        public int AdministrativeSupport_White_Male_Count { get; set; }
        public int AdministrativeSupport_Black_Male_Count { get; set; }
        public int AdministrativeSupport_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int AdministrativeSupport_Asian_Male_Count { get; set; }
        public int AdministrativeSupport_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int AdministrativeSupport_TwoOrMoreRaces_Male_Count { get; set; }
        public int AdministrativeSupport_White_Female_Count { get; set; }
        public int AdministrativeSupport_Black_Female_Count { get; set; }
        public int AdministrativeSupport_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int AdministrativeSupport_Asian_Female_Count { get; set; }
        public int AdministrativeSupport_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int AdministrativeSupport_TwoOrMoreRaces_Female_Count { get; set; }
        public int AdministrativeSupport_Total { get; set; }
        public int CraftWorkers_Hispanic_Male_Count { get; set; }
        public int CraftWorkers_Hispanic_Female_Count { get; set; }
        public int CraftWorkers_White_Male_Count { get; set; }
        public int CraftWorkers_Black_Male_Count { get; set; }
        public int CraftWorkers_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int CraftWorkers_Asian_Male_Count { get; set; }
        public int CraftWorkers_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int CraftWorkers_TwoOrMoreRaces_Male_Count { get; set; }
        public int CraftWorkers_White_Female_Count { get; set; }
        public int CraftWorkers_Black_Female_Count { get; set; }
        public int CraftWorkers_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int CraftWorkers_Asian_Female_Count { get; set; }
        public int CraftWorkers_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int CraftWorkers_TwoOrMoreRaces_Female_Count { get; set; }
        public int CraftWorkers_Total { get; set; }
        public int Operatives_Hispanic_Male_Count { get; set; }
        public int Operatives_Hispanic_Female_Count { get; set; }
        public int Operatives_White_Male_Count { get; set; }
        public int Operatives_Black_Male_Count { get; set; }
        public int Operatives_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int Operatives_Asian_Male_Count { get; set; }
        public int Operatives_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int Operatives_TwoOrMoreRaces_Male_Count { get; set; }
        public int Operatives_White_Female_Count { get; set; }
        public int Operatives_Black_Female_Count { get; set; }
        public int Operatives_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int Operatives_Asian_Female_Count { get; set; }
        public int Operatives_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int Operatives_TwoOrMoreRaces_Female_Count { get; set; }
        public int Operatives_Total { get; set; }
        public int LaborersAndHelpers_Hispanic_Male_Count { get; set; }
        public int LaborersAndHelpers_Hispanic_Female_Count { get; set; }
        public int LaborersAndHelpers_White_Male_Count { get; set; }
        public int LaborersAndHelpers_Black_Male_Count { get; set; }
        public int LaborersAndHelpers_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int LaborersAndHelpers_Asian_Male_Count { get; set; }
        public int LaborersAndHelpers_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int LaborersAndHelpers_TwoOrMoreRaces_Male_Count { get; set; }
        public int LaborersAndHelpers_White_Female_Count { get; set; }
        public int LaborersAndHelpers_Black_Female_Count { get; set; }
        public int LaborersAndHelpers_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int LaborersAndHelpers_Asian_Female_Count { get; set; }
        public int LaborersAndHelpers_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int LaborersAndHelpers_TwoOrMoreRaces_Female_Count { get; set; }
        public int LaborersAndHelpers_Total { get; set; }
        public int ServiceWorkers_Hispanic_Male_Count { get; set; }
        public int ServiceWorkers_Hispanic_Female_Count { get; set; }
        public int ServiceWorkers_White_Male_Count { get; set; }
        public int ServiceWorkers_Black_Male_Count { get; set; }
        public int ServiceWorkers_NativeHawaiian_OtherPacificIslander_Male_Count { get; set; }
        public int ServiceWorkers_Asian_Male_Count { get; set; }
        public int ServiceWorkers_AmericanIndian_Or_AlaskaNative_Male_Count { get; set; }
        public int ServiceWorkers_TwoOrMoreRaces_Male_Count { get; set; }
        public int ServiceWorkers_White_Female_Count { get; set; }
        public int ServiceWorkers_Black_Female_Count { get; set; }
        public int ServiceWorkers_NativeHawaiian_OtherPacificIslander_Female_Count { get; set; }
        public int ServiceWorkers_Asian_Female_Count { get; set; }
        public int ServiceWorkers_AmericanIndian_Or_AlaskaNative_Female_Count { get; set; }
        public int ServiceWorkers_TwoOrMoreRaces_Female_Count { get; set; }
        public int ServiceWorkers_Total { get; set; }
        public int Hispanic_Male_Total  { get; set; }
        public int Hispanic_Female_Total { get; set; }
        public int White_Male_Total { get; set; }
        public int Black_Male_Total { get; set; }
        public int NativeHawaiian_OtherPacificIslander_Male_Total { get; set; }
        public int Asian_Male_Total { get; set; }
        public int AmericanIndian_Or_AlaskaNative_Male_Total { get; set; }
        public int TwoOrMoreRaces_Male_Total { get; set; }
        public int White_Female_Total { get; set; }
        public int Black_Female_Total { get; set; }
        public int NativeHawaiian_OtherPacificIslander_Female_Total { get; set; }
        public int Asian_Female_Total { get; set; }
        public int AmericanIndian_Or_AlaskaNative_Female_Total { get; set; }
        public int TwoOrMoreRaces_Female_Total { get; set; }
        public int LocationEmployeeTotal { get; set; }
        public int GrandTotal { get; set; }
        public int Ein { get; set; }

        public int CalculateHispanicMaleTotal()
        {
            return ExecutiveLevel_Hispanic_Male_Count        +
                   MidLevel_Hispanic_Male_Count              +
                   Professionals_Hispanic_Male_Count         +
                   Technicians_Hispanic_Male_Count           +
                   SalesWorkers_Hispanic_Male_Count          +
                   AdministrativeSupport_Hispanic_Male_Count +
                   CraftWorkers_Hispanic_Male_Count          +
                   Operatives_Hispanic_Male_Count            +
                   LaborersAndHelpers_Hispanic_Male_Count    +
                   ServiceWorkers_Hispanic_Male_Count;

        }

        public int CalculateHispanicFemaleTotal()
        {
            return ExecutiveLevel_Hispanic_Female_Count        +
                   MidLevel_Hispanic_Female_Count              +
                   Professionals_Hispanic_Female_Count         +
                   Technicians_Hispanic_Female_Count           +
                   SalesWorkers_Hispanic_Female_Count          +
                   AdministrativeSupport_Hispanic_Female_Count +
                   CraftWorkers_Hispanic_Female_Count          +
                   Operatives_Hispanic_Female_Count            +
                   LaborersAndHelpers_Hispanic_Female_Count    +
                   ServiceWorkers_Hispanic_Female_Count;
        }

        public int CalculateWhiteMaleTotal()
        {
            return ExecutiveLevel_White_Male_Count        +
                   MidLevel_White_Male_Count              +
                   Professionals_White_Male_Count         +
                   Technicians_White_Male_Count           +
                   SalesWorkers_White_Male_Count          +
                   AdministrativeSupport_White_Male_Count +
                   CraftWorkers_White_Male_Count          +
                   Operatives_White_Male_Count            +
                   LaborersAndHelpers_White_Male_Count    +
                   ServiceWorkers_White_Male_Count;
        }

        public int CalculateBlackMaleTotal()
        {
            return ExecutiveLevel_Black_Male_Count        +
                   MidLevel_Black_Male_Count              +
                   Professionals_Black_Male_Count         +
                   Technicians_Black_Male_Count           +
                   SalesWorkers_Black_Male_Count          +
                   AdministrativeSupport_Black_Male_Count +
                   CraftWorkers_Black_Male_Count          +
                   Operatives_Black_Male_Count            +
                   LaborersAndHelpers_Black_Male_Count    +
                   ServiceWorkers_Black_Male_Count;
        }

        public int CalculateNativeHawaiianOrOtherPacificIslanderMaleTotal()
        {
            return ExecutiveLevel_NativeHawaiian_OtherPacificIslander_Male_Count        +
                   MidLevel_NativeHawaiian_OtherPacificIslander_Male_Count              +
                   Professionals_NativeHawaiian_OtherPacificIslander_Male_Count         +
                   Technicians_NativeHawaiian_OtherPacificIslander_Male_Count           +
                   SalesWorkers_NativeHawaiian_OtherPacificIslander_Male_Count          +
                   AdministrativeSupport_NativeHawaiian_OtherPacificIslander_Male_Count +
                   CraftWorkers_NativeHawaiian_OtherPacificIslander_Male_Count          +
                   Operatives_NativeHawaiian_OtherPacificIslander_Male_Count            +
                   LaborersAndHelpers_NativeHawaiian_OtherPacificIslander_Male_Count    +
                   ServiceWorkers_NativeHawaiian_OtherPacificIslander_Male_Count;
        }

        public int CalculateAsianMaleTotal()
        {
            return ExecutiveLevel_Asian_Male_Count        +
                   MidLevel_Asian_Male_Count              +
                   Professionals_Asian_Male_Count         +
                   Technicians_Asian_Male_Count           +
                   SalesWorkers_Asian_Male_Count          +
                   AdministrativeSupport_Asian_Male_Count +
                   CraftWorkers_Asian_Male_Count          +
                   Operatives_Asian_Male_Count            +
                   LaborersAndHelpers_Asian_Male_Count    +
                   ServiceWorkers_Asian_Male_Count;
        }

        public int CalculateAmericanIndianOrNativeAlaskanMaleTotal()
        {
            return ExecutiveLevel_AmericanIndian_Or_AlaskaNative_Male_Count        +
                   MidLevel_AmericanIndian_Or_AlaskaNative_Male_Count              +
                   Professionals_AmericanIndian_Or_AlaskaNative_Male_Count         +
                   Technicians_AmericanIndian_Or_AlaskaNative_Male_Count           +
                   SalesWorkers_AmericanIndian_Or_AlaskaNative_Male_Count          +
                   AdministrativeSupport_AmericanIndian_Or_AlaskaNative_Male_Count +
                   CraftWorkers_AmericanIndian_Or_AlaskaNative_Male_Count          +
                   Operatives_AmericanIndian_Or_AlaskaNative_Male_Count            +
                   LaborersAndHelpers_AmericanIndian_Or_AlaskaNative_Male_Count    +
                   ServiceWorkers_AmericanIndian_Or_AlaskaNative_Male_Count;
        }

        public int CalculateTwoOrMoreRacesMaleTotal()
        {
            return ExecutiveLevel_TwoOrMoreRaces_Male_Count        +
                   MidLevel_TwoOrMoreRaces_Male_Count              +
                   Professionals_TwoOrMoreRaces_Male_Count         +
                   Technicians_TwoOrMoreRaces_Male_Count           +
                   SalesWorkers_TwoOrMoreRaces_Male_Count          +
                   AdministrativeSupport_TwoOrMoreRaces_Male_Count +
                   CraftWorkers_TwoOrMoreRaces_Male_Count          +
                   Operatives_TwoOrMoreRaces_Male_Count            +
                   LaborersAndHelpers_TwoOrMoreRaces_Male_Count    +
                   ServiceWorkers_TwoOrMoreRaces_Male_Count;
        }

        public int CalculateWhiteFemaleTotal()
        {
            return ExecutiveLevel_White_Female_Count        +
                   MidLevel_White_Female_Count              +
                   Professionals_White_Female_Count         +
                   Technicians_White_Female_Count           +
                   SalesWorkers_White_Female_Count          +
                   AdministrativeSupport_White_Female_Count +
                   CraftWorkers_White_Female_Count          +
                   Operatives_White_Female_Count            +
                   LaborersAndHelpers_White_Female_Count    +
                   ServiceWorkers_White_Female_Count;
        }

        public int CalculateBlackFemaleTotal()
        {
            return ExecutiveLevel_Black_Female_Count        +
                   MidLevel_Black_Female_Count              +
                   Professionals_Black_Female_Count         +
                   Technicians_Black_Female_Count           +
                   SalesWorkers_Black_Female_Count          +
                   AdministrativeSupport_Black_Female_Count +
                   CraftWorkers_Black_Female_Count          +
                   Operatives_Black_Female_Count            +
                   LaborersAndHelpers_Black_Female_Count    +
                   ServiceWorkers_Black_Female_Count;
        }

        public int CalculateNativeHawaiianOrOtherPacificIslanderFemaleTotal()
        {
            return ExecutiveLevel_NativeHawaiian_OtherPacificIslander_Female_Count        +
                   MidLevel_NativeHawaiian_OtherPacificIslander_Female_Count              +
                   Professionals_NativeHawaiian_OtherPacificIslander_Female_Count         +
                   Technicians_NativeHawaiian_OtherPacificIslander_Female_Count           +
                   SalesWorkers_NativeHawaiian_OtherPacificIslander_Female_Count          +
                   AdministrativeSupport_NativeHawaiian_OtherPacificIslander_Female_Count +
                   CraftWorkers_NativeHawaiian_OtherPacificIslander_Female_Count          +
                   Operatives_NativeHawaiian_OtherPacificIslander_Female_Count            +
                   LaborersAndHelpers_NativeHawaiian_OtherPacificIslander_Female_Count    +
                   ServiceWorkers_NativeHawaiian_OtherPacificIslander_Female_Count;
        }

        public int CalculateAsianFemaleTotal()
        {
            return ExecutiveLevel_Asian_Female_Count        +
                   MidLevel_Asian_Female_Count              +
                   Professionals_Asian_Female_Count         +
                   Technicians_Asian_Female_Count           +
                   SalesWorkers_Asian_Female_Count          +
                   AdministrativeSupport_Asian_Female_Count +
                   CraftWorkers_Asian_Female_Count          +
                   Operatives_Asian_Female_Count            +
                   LaborersAndHelpers_Asian_Female_Count    +
                   ServiceWorkers_Asian_Female_Count;
        }

        public int CalculateAmericanIndianOrNativeAlaskanFemaleTotal()
        {
            return ExecutiveLevel_AmericanIndian_Or_AlaskaNative_Female_Count        +
                   MidLevel_AmericanIndian_Or_AlaskaNative_Female_Count              +
                   Professionals_AmericanIndian_Or_AlaskaNative_Female_Count         +
                   Technicians_AmericanIndian_Or_AlaskaNative_Female_Count           +
                   SalesWorkers_AmericanIndian_Or_AlaskaNative_Female_Count          +
                   AdministrativeSupport_AmericanIndian_Or_AlaskaNative_Female_Count +
                   CraftWorkers_AmericanIndian_Or_AlaskaNative_Female_Count          +
                   Operatives_AmericanIndian_Or_AlaskaNative_Female_Count            +
                   LaborersAndHelpers_AmericanIndian_Or_AlaskaNative_Female_Count    +
                   ServiceWorkers_AmericanIndian_Or_AlaskaNative_Female_Count;
        }

        public int CalculateTwoOrMoreRacesFemaleTotal()
        {
            return ExecutiveLevel_TwoOrMoreRaces_Female_Count        +
                   MidLevel_TwoOrMoreRaces_Female_Count              +
                   Professionals_TwoOrMoreRaces_Female_Count         +
                   Technicians_TwoOrMoreRaces_Female_Count           +
                   SalesWorkers_TwoOrMoreRaces_Female_Count          +
                   AdministrativeSupport_TwoOrMoreRaces_Female_Count +
                   CraftWorkers_TwoOrMoreRaces_Female_Count          +
                   Operatives_TwoOrMoreRaces_Female_Count            +
                   LaborersAndHelpers_TwoOrMoreRaces_Female_Count    +
                   ServiceWorkers_TwoOrMoreRaces_Female_Count;
        }

        public int CalculateLocationEmployeeTotal()
        {
            return ExecutiveLevel_Total        +
                   MidLevel_Total              +
                   Professionals_Total         +
                   Technicians_Total           +
                   SalesWorkers_Total          +
                   AdministrativeSupport_Total +
                   CraftWorkers_Total          +
                   Operatives_Total            +
                   LaborersAndHelpers_Total    +
                   ServiceWorkers_Total;
        }
    }
}
