
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makman.Middle.Entities
{
    [Table("TagCategories")]
    public class TagCategory : CollectionDatabaseEntity
    {
        [Key]
        [Required]
        [Column("TagCategoryId")]
        public Guid Id { get; set; }

        [Required]
        [Column("TagCategoryName")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Column("TagCategoryColor")]
        [MaxLength(15)]
        public string Color { get; set; }

        public TagCategory(string name, string color = "")
        {
            init();
            Name = name;
            Color = color;
        }
        private void init()
        {
            Id = Guid.NewGuid();
        }

    }
}
