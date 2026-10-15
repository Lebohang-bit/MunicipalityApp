using System;

namespace MunicipalityApp
{
    public class Event
    {
        public string EventID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int Priority { get; set; }

        public Event()
        {
            EventID = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Priority = 3;
        }

        public Event(string title, string category, string location, string description, DateTime eventDate, int priority)
        {
            EventID = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Title = title;
            Category = category;
            Location = location;
            Description = description;
            EventDate = eventDate;
            Priority = priority;
        }

        public override string ToString()
        {
            return $"{EventDate:yyyy-MM-dd} | {Category} | {Title}";
        }
    }
}