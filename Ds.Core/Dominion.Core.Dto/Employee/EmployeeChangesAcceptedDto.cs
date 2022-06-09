using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    // Replaces a vb class, kept original naming convention.
    [Serializable]
    public class EmployeeChangesAcceptedDto
    {
        public string m_strDominionToValue                 { get; set; }
        public string strDominionFromValue                 { get; set; }
        public int    intDominionColumnsID                 { get; set; }
        public int    intDominionTablesID                  { get; set; }
        public bool   bolIsNewRow                          { get; set; }
        public bool   bolIsNewHire                         { get; set; }
        public int    intEmployeeID                        { get; set; }
        public int    intForeignKeyID                      { get; set; }
        public string strDominionColumnName                { get; set; }
        public int    intClientMappedInterfaceAssignmentID { get; set; }
        public int    intClientMappedInterfaceID           { get; set; }
        public string strDominionTableName                 { get; set; }
        public bool   bolIsForeignKey                      { get; set; }
        public string strForeignKeyColumn                  { get; set; }
        public string strSourceToValue                     { get; set; }
        public string strParentTableColumn_Description     { get; set; }
        public int    intEmployeeBankID                    { get; set; }

        public string strDominionToValue 
        { 
            get 
            { 
                return this.m_strDominionToValue; 
            } 
            set
            { 
                if (value == "-2147483648") 
                    this.m_strDominionToValue = "null"; 
                else 
                    this.m_strDominionToValue = value; 
            } 
        }
    }
}
