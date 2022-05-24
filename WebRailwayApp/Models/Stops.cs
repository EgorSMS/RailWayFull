using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebRailwayApp.Models
{
    public partial class Stops
    {
        [Key]
        public int ID_Stop { get; set; }
        public int ID_City { get; set; }
        public int ID_TimeTable { get; set; }
        public DateTime TimeOfStop { get; set; }      
        public int Platform { get; set; }

    }
}
