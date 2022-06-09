using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Accounting
{
    public partial class ClientGLInterface : Entity<ClientGLInterface>, IHasModifiedUserNameData
    {
        public virtual int ClientGLInterfaceId      { get; set; }
        public virtual int GeneralLedgerInterfaceId { get; set; }
        public virtual string Description           { get; set; }
        public virtual string FileName              { get; set; }
        public virtual string Email                 { get; set; }
        public virtual int ClientId                 { get; set; }
        public virtual int TaxFrequencyId           { get; set; }
        public virtual DateTime Modified            { get; set; }
        public virtual string ModifiedBy            { get; set; }
        public virtual int StartedTransactionId     { get; set; }
        public virtual string DefaultClass          { get; set; }
        public virtual int MemoOptionId             { get; set; }
        public virtual bool IsTabDelimited          { get; set; }
    }
}
