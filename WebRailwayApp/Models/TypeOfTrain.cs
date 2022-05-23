using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebRailwayApp.Models
{
    public partial class TypeOfTrain
    {
        public TypeOfTrain()
        {
        }

        [Key]
        public int ID_TypeOfTrain { get; set; }
        public string Name { get; set; }
        public string MaxSpeed { get; set; }
        public int Capacity { get; set; }

    }
}
