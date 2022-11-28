﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.EFDataModel
{
    [Table("mdevice")]
    public class EFMeasureDevice
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("name")] // IP address
        [Required(ErrorMessage = "IP address is required")]
        public string Name { get; set; }

        [Column("mdintarval")]
        [Required(ErrorMessage = "Measure intarvall is requiered")]
        public int Interval { get; set; }


        public EFMeasureDevice()
        {
            Id = -1;
            Name = string.Empty;
            Interval = -1;
        }

        public EFMeasureDevice(string name, int interval)
        {
            Name = name;
            Interval = interval;
        }

        public EFMeasureDevice(int id, string name, int interval)
        {
            Id = id;
            Name = name;
            Interval = interval;
        }
    }
}
