using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullCalendar.Models {
    public class CalendarEvent {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        public bool allDay { get; set; }
    }
}