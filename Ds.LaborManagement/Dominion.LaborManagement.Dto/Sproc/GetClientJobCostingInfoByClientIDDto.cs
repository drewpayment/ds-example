using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClientJobCostingInfoByClientIDDto
    {
        //    public DateTime Modified { get; set; }
        //    public Boolean isEnabled { get; set; }
        //    public Boolean HideOnScreen { get; set; }
        //    public Boolean IsPostBack { get; set; }
        //    public Boolean IsRequired { get; set; }
        //    public string DescriptionClientJobCosting { get; set; }
        //    public string DescriptionJobCost { get; set; }
        //    public string DescriptionJobCostType { get; set; }

        //    public int ClientID { get; set; }
        //    public int JobCostingTypeID { get; set; }
        //    public int Sequence { get; set; }
        //    public int ModifiedBy { get; set; }
        //    public int Width { get; set; }
        //    public int Level { get; set; }
        //    public int ID { get; set; }
        //    public int JobCostID { get; set; }
        //    public int ForeignKeyID { get; set; }
        //    public int JobCostingTypeIDType { get; set; }

        public class table1
        {
            public int ClientJobCostingID { get; set; }
            public int ClientID { get; set; }
            public string Description { get; set; }
            public int JobCostingTypeID { get; set; }
            public int Sequence { get; set; }
            public int ModifiedBy { get; set; }
            public DateTime Modified { get; set; }
            public Boolean isEnabled { get; set; }
            public Boolean HideOnScreen { get; set; }
            public Boolean IsPostBack { get; set; }
            public int Width { get; set; }
            public int Level { get; set; }
            public Boolean IsRequired { get; set; }

        }

        public class table2
        {
            public int ID { get; set; }
            public int JobCostID { get; set; }
            public string Description { get; set; }
            public int? ForeignKeyID { get; set; }

        }


        public class table3
        {

            public int ID { get; set; }
            public string Description { get; set; }
            public int JobCostingTypeID { get; set; }
        }

        //ClientJobCostingID(int)
        //                    ClientID(int)
        //                    Description(string)
        //                    JobCostingTypeID(int)
        //                    Sequence(int)
        //                    ModifiedBy(int)
        //                    Modified(date)
        //                    isEnabled(bool)
        //                    HideOnScreen(bool)
        //                    IsPostBack(bool)
        //                    Width(int)
        //                    Level(int)
        //                    IsRequired(bool)
        //                Table(1)
        //                    ID(int)
        //                    JobCostID(int)
        //                    Description(string)
        //                    ForeignKeyID(int)
        //                Table(2)
        //                    ID(int)
        //                    Description(string)
        //                    JobCostingTypeID(int)
    }
}
