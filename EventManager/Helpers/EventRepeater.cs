using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.Helpers
{
    public class EventRepeater
    {
        public int repeatType { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int[] dow { get; set; }
        public int repeatCount { get; set; }
    }
}