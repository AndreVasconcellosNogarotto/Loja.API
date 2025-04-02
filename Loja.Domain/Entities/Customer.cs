namespace Loja.Domain.Entities
{
    public class Customer : EntityBase
    {
        public string ExternalId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; private set; }

        protected Customer() { }

        public Customer(string externalId, string name, string email, string document)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                throw new ArgumentException("ExternalId cannot be empty", nameof(externalId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            ExternalId = externalId;
            Name = name;
            Email = email;
            Document = document;
        }

        public void Update(string name, string email, string document)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(email))
                Email = email;

            if (!string.IsNullOrWhiteSpace(document))
                Document = document;

            SetUpdatedAt();
        }
    }
}
