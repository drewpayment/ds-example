using System;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    public class CustomBenefitField : Entity<CustomBenefitField>, IHasModifiedData
    {
        public int       CustomFieldId    { get; set; }
        public int       ClientId         { get; set; }
        public string    Name             { get; set; }
        public string    FieldKey         { get; set; }
        public FieldType FieldType        { get; set; }
        public bool      IsEmployeeField  { get; set; }
        public bool      IsDependentField { get; set; }
        public bool      IsArchived       { get; set; }
        public DateTime  Modified         { get; set; }
        public int       ModifiedBy       { get; set; }
        public Client    Client           { get; set; }
    }
}
