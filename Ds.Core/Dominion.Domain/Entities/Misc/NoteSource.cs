using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominion.Domain.Entities.Misc
{
    /// <summary>
    /// Note Source entity providing all note sources
    /// </summary>
    [Table("NoteSource")]
    public class NoteSourceEntity : Entity<NoteSourceEntity>
    {
        [Key]
        public virtual int    NoteSourceId { get; set; }
        public virtual string NoteSource   { get; set; }
        public NoteSourceEntity()
        {
        }
    }
}