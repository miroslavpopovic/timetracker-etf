using System.ComponentModel.DataAnnotations;

namespace TimeTrackerEtf.Client.Models
{
    public class ClientInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
