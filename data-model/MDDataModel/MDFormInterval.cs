using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace DataModel.MDDataModel
{
    [Serializable]
    public class MDFormInterval
    {
        [Required(ErrorMessage = "Az intervallum szükséges")]
        [Range(1, long.MaxValue, ErrorMessage = "Az intervallum nullánál nagyobb szám")]
        public long Interval { get; set; } = -1;

        public MDFormInterval()
        {
            Interval = -1;
        }

        public MDFormInterval(long interval)
        {
            Interval = interval;
        }
    }
}
