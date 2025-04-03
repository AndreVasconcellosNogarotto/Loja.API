using AutoMapper;
using Loja.Application.DTOs;
using Loja.Domain.Entities;
using Loja.Domain.ValueObject;

namespace Loja.Application.AutoMapperConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Branch, BranchDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.BasePrice.Currency));

            CreateMap<SaleItem, SaleItemDto>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.UnitPrice.Currency))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice.Value));

            CreateMap<Sale, SaleDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TotalAmount.Currency))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CustomerDto, Customer>()
                .ForCtorParam("externalId", opt => opt.MapFrom(src => src.ExternalId))
                .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("email", opt => opt.MapFrom(src => src.Email))
                .ForCtorParam("document", opt => opt.MapFrom(src => src.Document));

            CreateMap<BranchDto, Branch>()
                .ForCtorParam("externalId", opt => opt.MapFrom(src => src.ExternalId))
                .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("location", opt => opt.MapFrom(src => src.Location));

            CreateMap<ProductDto, Product>()
                .ForCtorParam("externalId", opt => opt.MapFrom(src => src.ExternalId))
                .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("description", opt => opt.MapFrom(src => src.Description))
                .ForCtorParam("basePrice", opt => opt.MapFrom(src => new Money(src.BasePrice, src.Currency)));
        }
    }
}
