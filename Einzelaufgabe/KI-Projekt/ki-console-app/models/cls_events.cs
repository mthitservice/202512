using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace kiconsoleapp.models
{

    public class cls_event
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }

        public cls_disciplin disciplin { get; set; }
        public float elevation { get; set; }
        public DateTime event_day { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }

        public virtual List<cls_record> Records { get; set; }
        public cls_event(string name, string location, float elevation, DateTime event_day, float longitude, float latitude)
        {
            this.name = name;
            this.location = location;

            this.elevation = elevation;
            this.event_day = event_day;
            this.longitude = longitude;
            this.latitude = latitude;

        }


    }


}