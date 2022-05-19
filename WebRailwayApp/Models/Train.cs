using System;
using System.Collections.Generic;

#nullable disable

namespace WebRailwayApp.Models
{
    public partial class Train
    {
        public Train()
        {
            
        }

        public int IdTrain { get; set; }
        public int IdTypeOfTrain { get; set; }
        public int NumberOfTrain { get; set; }

        
    }
}
