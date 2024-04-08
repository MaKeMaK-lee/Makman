
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makman.Middle.Entities.Connections
{
    [Table("UnitTag")]
    public class UnitTag
    {
        [Required]
        [Column("UnitId")]
        public Guid UnitId { get; set; }
        [Required]
        [Column("TagId")]
        public Guid TagId { get; set; }
    }
}
