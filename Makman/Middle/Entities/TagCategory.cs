
using Makman.Middle.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makman.Middle.Entities
{
    [Table("TagCategories")]
    public class TagCategory : ObservableObject
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

        [NotMapped]
        public string ChangableColor
        {
            get => Color;
            set
            {
                Color = value;
                OnPropertyChanged(Color);
                OnPropertyChanged(ChangableColor);
            }
        }

        public TagCategory() 
        {

        }

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

        public override string ToString()
        {
            return Name;
        }
    }
}
