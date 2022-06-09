using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLAssignmentDto
    {
        public int      ClientGLAssignmentId  { get; set; }
        public int      GeneralLedgerTypeId   { get; set; }
        public int?     ClientCostCenterId    { get; set; }
        public int?     ClientGeneralLedgerId { get; set; }
        public int?     ForeignKeyId          { get; set; }
        public string   Description           { get; set; }
        public int      ClientId              { get; set; }
        public DateTime Modified              { get; set; }
        public string   ModifiedBy            { get; set; }
        public bool     IsAccrued             { get; set; }
        public bool     IsDetail              { get; set; }
        public int?     SequenceNum           { get; set; }
        public int?     ClientDepartmentId    { get; set;}
        public bool     IsOffset              { get; set; }
        public string   Project               { get; set; }
        public int?     ClientDivisionId      { get; set; }
        public int?     ClientGroupId         { get; set; }
        public int?     ClientGLClassGroupId  { get; set; }

        public bool CanBeDetail   { get; set; }
        public bool CanBeAccrued  { get; set; }
        public bool CanBeOffset   { get; set; }
        public double SequenceId { get; set; }

        public ClientCostCenterDto     ClientCostCenter { get; set; }
        public CoreClientDepartmentDto ClientDepartment { get; set; }
        public ClientDivisionDto       ClientDivision   { get; set; }
        public ClientGroupDto          ClientGroup      { get; set; }


    }

    public class ClientGLMappingItemDto : ClientGLAssignmentDto
    {
        public int?   ClientGLAssignmentId { get; set; }
        public int    ClassId              { get; set; }
        public string ClassCode            { get; set; }
        public string ClassDescription     { get; set; }
        public bool   IsActive             { get; set; }
        
    }
}
