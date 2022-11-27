using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeasuringServer.Model
{
    [Table("md")]
    public class EFMeasureDevice
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("name")]
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        public EFMeasureDevice()
        {
            Id = -1;
            Name = string.Empty;
        }

        public EFMeasureDevice(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
