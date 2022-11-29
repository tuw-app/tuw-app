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
        [Required(ErrorMessage ="Az intervallum szükséges")]
        public string Interval { get; set; } =string.Empty;

        public MDFormInterval()
        {
            Interval= string.Empty;
        }

        public MDFormInterval(string interval)
        {
            Interval = interval;
        }
    }
}
