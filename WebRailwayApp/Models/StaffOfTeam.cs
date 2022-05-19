using System;
using System.Collections.Generic;

#nullable disable

namespace WebRailwayApp.Models
{
    public partial class StaffOfTeam
    {
        public int IdSot { get; set; }
        public int IdStaff1 { get; set; }
        public int IdStaff2 { get; set; }
        public int IdTrain { get; set; }
    }
}
