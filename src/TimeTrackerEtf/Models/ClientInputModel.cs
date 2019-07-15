using TimeTrackerEtf.Domain;

namespace TimeTrackerEtf.Models
{
    public class ClientInputModel
    {
        public string Name { get; set; }

        public void MapTo(Client client)
        {
            client.Name = Name;
        }
    }
}
