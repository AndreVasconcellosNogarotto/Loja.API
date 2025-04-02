namespace Loja.Application.DTOs
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
    }
}
