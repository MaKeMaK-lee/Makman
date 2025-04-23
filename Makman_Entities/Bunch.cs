
using Makman_Core;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Makman_Entities
{
    [Table("Bunchs")]
    public class Bunch: ObservableObject
    {
        [Key]
        [Required]
        [Column("BunchId")]
        public Guid Id { get; set; }

        [Required]
        [Column("BunchUnits")]
        public ObservableCollection<Unit> Units { get; set; }

        public Bunch()
        {
            init();
        }

        private void init()
        {
            Id = Guid.NewGuid();
        }
    }
}
