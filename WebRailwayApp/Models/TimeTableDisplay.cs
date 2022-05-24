using System.Collections.Generic;

namespace WebRailwayApp.Models
{
    public class TimeTableDisplay
    {
        public TimeTable timeTable { get; set; }
        public List<Train> trains { get; set; }
        public List<Route> routes { get; set; }
        public List<Cities> cities { get; set; }

        public bool ErrorDate { get; set; }
        public bool ErrorDateDepartureAlreadyExist { get; set; }
        public bool ErrorDateArrivalAlreadyExist { get; set; }

    }
}
