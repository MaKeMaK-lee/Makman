
using Makman.Middle.Core;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makman.Middle.Entities
{
    [Table("Units")]
    public class Unit : ObservableObject
    {
        [Key]
        [Required]
        [Column("UnitId")]
        public required Guid Id { get; set; }

        [Required]
        [Column("UnitFullFileName")]
        [MaxLength(5000)]
        public required string FullFileName { get; set; }

        [Required]
        [Column("UnitFileSize")]
        public required long FileSize { get; set; }

        [Required]
        [Column("UnitFileName")]
        [MaxLength(1000)]
        public required string FileName { get; set; }

        [Required]
        [Column("UnitTags")]
        public required ObservableCollection<Tag> Tags { get; set; }

        [Column("UnitBunchId")]
        public Guid? BunchId { get; set; }

        [Column("UnitBunch")]
        public Bunch? Bunch { get; set; }

        [Column("UnitParentUnitId")]
        public Guid? ParentUnitId { get; set; }

        [Column("UnitParentUnit")]
        public Unit? ParentUnit { get; set; }

        [Required]
        [Column("UnitChildUnits")]
        public required ObservableCollection<Unit> ChildUnits { get; set; }

        [NotMapped] 
        public ObservableCollection<Unit>? BunchedUnits { get => Bunch?.Units; }


        public bool Equals(Unit unit2)
        {
            if (unit2 == this)
            {
                return true;
            }
            return this.FullFileName == unit2.FullFileName;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((obj as Unit)!);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


    }
}
