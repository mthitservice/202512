using System;
using System;
using System.ComponentModel.DataAnnotations;

namespace kiconsoleapp.models
{

    public class cls_disciplin
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public virtual List<cls_event> Events { get; set; }


        public cls_disciplin(int id, string name)
        {
            this.id = id;
            this.name = name;


        }


    }


}
