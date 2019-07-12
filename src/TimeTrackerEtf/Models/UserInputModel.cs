using System;
using TimeTrackerEtf.Domain;

namespace TimeTrackerEtf.Models
{
    public class UserInputModel
    {
        public string Name { get; set; }

        public decimal HourRate { get; set; }

        public void MapTo(User user)
        {
            user.Name = Name;
            user.HourRate = HourRate;
        }
    }
}
