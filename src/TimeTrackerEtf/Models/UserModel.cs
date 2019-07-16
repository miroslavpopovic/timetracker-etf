using TimeTrackerEtf.Domain;

namespace TimeTrackerEtf.Models
{
    /// <summary>
    /// Represents one time tracker user.
    /// </summary>
    public class UserModel
    {
        private UserModel()
        {
        }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets how much the user will be paid per hour.
        /// </summary>
        public decimal HourRate { get; set; }

        public static UserModel FromUser(User user)
        {
            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                HourRate = user.HourRate
            };
        }
    }
}
