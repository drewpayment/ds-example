using System;
using System.Collections.Generic;

using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Forms;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Forms
{
    public class FormType : Entity<FormType>, IHasModifiedData
    {
        public virtual int             FormTypeId            { get; set; }
        public virtual SystemFormType? SystemFormType        { get; set; }
        public virtual LocalityType?   LocalityType          { get; set; }
        public virtual int?            LocalityId            { get; set; }
        public virtual State           State                 { get; set; }
        public virtual string          Description           { get; set; }
                                                             
        public virtual Boolean         OnboardingForm        { get; set; }
        public virtual int             ModifiedBy            { get; set; }
        public virtual DateTime        Modified              { get; set; }
        public virtual bool            NoDefinitionRequired  { get; set; }

        
        public virtual ICollection<FormDefinition> FormDefinitions { get; set; }

    }
}