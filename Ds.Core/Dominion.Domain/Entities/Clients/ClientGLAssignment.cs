using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientGLAssignment : Entity<ClientGLAssignment>
    {
        public virtual int      ClientGLAssignmentId  { get; set; }
        public virtual int      GeneralLedgerTypeId   { get; set; }
        public virtual int?     ClientCostCenterId    { get; set; }
        public virtual int?     ClientGeneralLedgerId { get; set; }
        public virtual int?     ForeignKeyId          { get; set; }
        public virtual string   Description           { get; set; }
        public virtual int      ClientId              { get; set; }
        public virtual DateTime Modified              { get; set; }
        public virtual string   ModifiedBy            { get; set; }
        public virtual bool     IsAccrued             { get; set; }
        public virtual bool     IsDetail              { get; set; }
        public virtual int?     SequenceNum           { get; set; }
        public virtual int?     ClientDepartmentId    { get; set;}
        public virtual bool     IsOffset              { get; set; }
        public virtual string   Project               { get; set; }
        public virtual int?     ClientDivisionId      { get; set; }
        public virtual int?     ClientGroupId         { get; set; }
        public virtual int?     ClientGLClassGroupId  { get; set; }

        public virtual ClientCostCenter ClientCostCenter { get; set; }
        public virtual ClientDepartment ClientDepartment { get; set; }
        public virtual ClientDivision ClientDivision { get; set; }
        public virtual ClientGroup ClientGroup { get; set; }
    }
}
