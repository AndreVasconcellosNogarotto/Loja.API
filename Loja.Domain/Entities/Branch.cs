namespace Loja.Domain.Entities
{
    public class Branch : EntityBase
    {
        public string ExternalId { get; private set; }
        public string Name { get; private set; }
        public string Location { get; private set; }

        // Para o ORM
        protected Branch() { }

        public Branch(string externalId, string name, string location)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                throw new ArgumentException("ExternalId cannot be empty", nameof(externalId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            ExternalId = externalId;
            Name = name;
            Location = location;
        }

        public void Update(string name, string location)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(location))
                Location = location;

            SetUpdatedAt();
        }
    }
}
