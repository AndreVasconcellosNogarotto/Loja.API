namespace Loja.Application.Interfaces
{
    public interface IApplicationServiceBase<TDto, TCreateRequest, TUpdateRequest>
        where TDto : class
        where TCreateRequest : class
        where TUpdateRequest : class
    {
        Task<TDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> AddAsync(TCreateRequest createRequest);
        Task<TDto> UpdateAsync(TUpdateRequest updateRequest);
        Task<bool> RemoveAsync(Guid id);
    }
}
