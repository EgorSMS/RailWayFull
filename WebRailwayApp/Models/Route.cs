using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebRailwayApp.Models
{
    public partial class Route
    {
        public Route()
        {
            
        }

        [Key]
        public int ID_Route { get; set; }
        public int ID_City_Departure { get; set; }
        public int ID_City_Arrival { get; set; }
        public int PlatformDeparture { get; set; }
        public int PlatformArrival { get; set; }

    }
}
