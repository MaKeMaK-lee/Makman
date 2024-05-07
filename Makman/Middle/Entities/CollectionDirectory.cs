
using Makman.Middle.Core;
using Makman.Middle.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makman.Middle.Entities
{
    [Table("CollectionDirectories")]
    public class CollectionDirectory : ObservableObject
    {


        [Key]
        [Required]
        [Column("CollectionDirectoryId")]
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Column("CollectionDirectoryPath")]
        [MaxLength(5000)]
        public string Path { get; set; }

        [Column("CollectionDirectorySpecialTag")]
        [MaxLength(1000)]
        public string? SpecialTag { get; set; }

        [Required]
        [Column("CollectionDirectoryAutoScanning", TypeName = "TINYINT(1)")]
        public bool AutoScanning { get; set; }

        [Required]
        [Column("CollectionDirectorySynchronizingWithCloud", TypeName = "TINYINT(1)")]
        public bool SynchronizingWithCloud { get; set; }

        //[NotMapped]
        //public object PickDirectoryCommand { get; set; }
        //public void PickDirectoryCommandAction(object? o)
        //{
        //    var newPath = 
        //}
        public CollectionDirectory()
        {

        }
        public CollectionDirectory(string path, bool autoscanning = true, bool synchronizingwithcloud = false)
        {
            init();
            Path = path;
            AutoScanning = autoscanning;
            SynchronizingWithCloud = synchronizingwithcloud;
        }
        private void init()
        {
            Id = Guid.NewGuid();
        }

    }
}
