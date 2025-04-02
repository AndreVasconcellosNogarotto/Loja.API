using Loja.Application.DTOs;

namespace Loja.Application.Interfaces
{
    public interface IProductService : IApplicationServiceBase<ProductDto, ProductDto, ProductDto>
    {
        Task<ProductDto> GetByExternalIdAsync(string externalId);
    }
}
