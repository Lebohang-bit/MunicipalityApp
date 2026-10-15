using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalityApp
{
    public class EventRepository
    {
        public Stack<Event> RecentActivityStack { get; private set; }
        public Queue<Event> NewlyAddedQueue { get; private set; }
        public PriorityQueue<Event, int> PriorityQueue { get; private set; }
        public Dictionary<string, List<Event>> EventsByCategory { get; private set; }
        public SortedDictionary<DateTime, List<Event>> EventsByDate { get; private set; }
        public HashSet<string> UniqueCategories { get; private set; }
        public List<Event> AllEvents { get; private set; }

        public EventRepository()
        {
            RecentActivityStack = new Stack<Event>();
            NewlyAddedQueue = new Queue<Event>();
            PriorityQueue = new PriorityQueue<Event, int>();
            EventsByCategory = new Dictionary<string, List<Event>>();
            EventsByDate = new SortedDictionary<DateTime, List<Event>>();
            UniqueCategories = new HashSet<string>();
            AllEvents = new List<Event>();
        }

        public void AddEvent(Event evt)
        {
            AllEvents.Add(evt);

            RecentActivityStack.Push(evt);
            NewlyAddedQueue.Enqueue(evt);

            int priorityValue = 4 - Math.Max(1, Math.Min(3, evt.Priority));
            PriorityQueue.Enqueue(evt, priorityValue);

            if (!EventsByCategory.ContainsKey(evt.Category))
            {
                EventsByCategory[evt.Category] = new List<Event>();
            }
            EventsByCategory[evt.Category].Add(evt);

            DateTime day = evt.EventDate.Date;
            if (!EventsByDate.ContainsKey(day))
            {
                EventsByDate[day] = new List<Event>();
            }
            EventsByDate[day].Add(evt);

            UniqueCategories.Add(evt.Category);
        }

        public List<Event> GetMostRecent(int count)
        {
            return RecentActivityStack.Take(count).ToList();
        }

        public List<Event> GetUpcoming(int count)
        {
            return AllEvents
                .Where(e => e.EventDate >= DateTime.Today)
                .OrderBy(e => e.EventDate)
                .Take(count)
                .ToList();
        }

        public List<Event> GetHighestPriority(int count)
        {
            List<Event> result = new List<Event>();
            PriorityQueue<Event, int> temp = new PriorityQueue<Event, int>();

            while (PriorityQueue.Count > 0 && result.Count < count)
            {
                Event e = PriorityQueue.Dequeue();
                result.Add(e);
            }

            return result;
        }

        public List<Event> Search(string category, DateTime? fromDate, DateTime? toDate)
        {
            IEnumerable<Event> query = AllEvents;

            if (!string.IsNullOrWhiteSpace(category) && category != "All Categories")
            {
                query = query.Where(e => e.Category == category);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(e => e.EventDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(e => e.EventDate <= toDate.Value);
            }

            return query.OrderBy(e => e.EventDate).ToList();
        }

        public void LoadSampleData()
        {
            DateTime today = DateTime.Today;

            AddEvent(new Event("Water Outage in Soweto", "Utilities", "Soweto", "Scheduled water maintenance for Zone 7.", today.AddDays(3), 1));
            AddEvent(new Event("Road Resurfacing - Main Reef Road", "Roads", "Roodepoort", "Lane closures expected. Use alternate routes.", today.AddDays(5), 1));
            AddEvent(new Event("Community Clean-Up Drive", "Sanitation", "Alexandra", "Join us for the monthly clean-up initiative.", today.AddDays(7), 3));
            AddEvent(new Event("Street Light Repair - Ward 42", "Street Lighting", "Sandton", "Faulty street lights will be repaired.", today.AddDays(2), 2));
            AddEvent(new Event("New Bus Route Launch", "Public Transport", "Johannesburg CBD", "Route 88 now connects Soweto to Sandton.", today.AddDays(10), 3));
            AddEvent(new Event("Park Restoration Project", "Parks and Recreation", "Emmarentia", "Tree planting and playground upgrades.", today.AddDays(14), 3));
            AddEvent(new Event("Waste Collection Schedule Update", "Waste Management", "Randburg", "New collection days effective next month.", today.AddDays(1), 2));
            AddEvent(new Event("Electricity Tariff Public Meeting", "Utilities", "Midrand", "Public consultation on proposed tariff changes.", today.AddDays(6), 1));
            AddEvent(new Event("Pothole Repair Program", "Roads", "Krugersdorp", "Targeted pothole repairs across the ward.", today.AddDays(4), 2));
            AddEvent(new Event("Library Reading Program", "Parks and Recreation", "Braamfontein", "Free reading sessions for children aged 5-12.", today.AddDays(9), 3));
            AddEvent(new Event("Fire Hydrant Maintenance", "Utilities", "Lenasia", "Routine hydrant inspections.", today.AddDays(8), 2));
            AddEvent(new Event("Community Safety Meeting", "Public Transport", "Soweto", "Monthly community policing forum.", today.AddDays(12), 3));
        }
    }
}