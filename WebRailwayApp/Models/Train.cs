using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebRailwayApp.Models
{
    public partial class Train
    {
        public Train()
        {
            
        }

        [Key]
        public int ID_Train { get; set; }
        public int ID_TypeOfTrain { get; set; }
        public int NumberOfTrain { get; set; }

        
    }
}
