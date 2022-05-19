using System;
using System.Collections.Generic;

#nullable disable

namespace WebRailwayApp.Models
{
    public partial class TimeTable
    {
        public int IdTimeTable { get; set; }
        public DateTime DateTimeArrived { get; set; }
        public DateTime DateTimeDeparted { get; set; }
        public int IdTrain { get; set; }
        public int IdRoute { get; set; }

        
    }
}
