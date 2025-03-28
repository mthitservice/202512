using System;
using System.ComponentModel.DataAnnotations;
namespace kiconsoleapp.models
{

    public class cls_record
    {
        [Key]
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public DateTime record_day { get; set; }

        public virtual cls_event sportsevent { get; set; }
        public string club { get; set; }
        public string gender { get; set; }
        public float Distance { get; set; }
        public int YearOfBirth { get; set; }
        public int timeinSecond { get; set; }
    }
}