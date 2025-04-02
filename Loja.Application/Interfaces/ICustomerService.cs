using Loja.Application.DTOs;

namespace Loja.Application.Interfaces
{
    public interface ICustomerService : IApplicationServiceBase<CustomerDto, CustomerDto, CustomerDto>
    {
        Task<CustomerDto> GetByExternalIdAsync(string externalId);
    }
}
