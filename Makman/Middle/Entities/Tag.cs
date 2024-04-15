
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makman.Middle.Entities
{
    [Table("Tags")]
    public class Tag : CollectionDatabaseEntity
    {
        [Key]
        [Required]
        [Column("TagId")]
        public Guid Id { get; set; }

        [Required]
        [Column("TagName")]
        [MaxLength(1000)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Column("TagColor")]
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
                OnPropertyChanged(SummaryColor);
            }
        }

        [NotMapped]
        public string SummaryColor
        {
            get
            {
                if (Color != "" || Category == null)
                    return Color;
                return Category.Color;
            }
        }

        [NotMapped]
        private TagCategory? category;

        [NotMapped]
        public TagCategory? Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
                OnPropertyChanged(nameof(SummaryColor));
            }
        }

        [Column("TagParentTag")]
        public Tag? ParentTag { get; set; }

        [Required]
        [Column("TagUnits")]
        public ICollection<Unit> Units { get; set; }

        public Tag()
        {

        }
        public Tag(string name, string color = "")
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
