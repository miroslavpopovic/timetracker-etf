using System;
using TimeTrackerEtf.Domain;

namespace TimeTrackerEtf.Models
{
    public class TimeEntryInputModel
    {
        public long ProjectId { get; set; }

        public long UserId { get; set; }

        public DateTime EntryDate { get; set; }

        public int Hours { get; set; }

        public string Description { get; set; }

        public void MapTo(TimeEntry timeEntry)
        {
            timeEntry.EntryDate = EntryDate;
            timeEntry.Hours = Hours;
            timeEntry.Description = Description;
        }
    }
}
