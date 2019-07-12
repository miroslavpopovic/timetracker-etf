using TimeTrackerEtf.Domain;

namespace TimeTrackerEtf.Models
{
    public class ClientModel
    {
        private ClientModel()
        {
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public static ClientModel FromClient(Client client)
        {
            return new ClientModel
            {
                Id = client.Id,
                Name = client.Name
            };
        }
    }
}
