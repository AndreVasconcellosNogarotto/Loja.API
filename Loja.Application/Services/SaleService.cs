using AutoMapper;
using Loja.Application.DTOs;
using Loja.Application.DTOs.Request;
using Loja.Application.Interfaces;
using Loja.Domain.Entities;
using Loja.Domain.Events;
using Loja.Domain.Interfaces;
using Loja.Domain.ValueObject;
using Loja.Infrastructure.EventPublisher;
using Microsoft.Extensions.Logging;

namespace Loja.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;

        public SaleService(
            ISaleRepository saleRepository,
            ICustomerRepository customerRepository,
            IBranchRepository branchRepository,
            IProductRepository productRepository,
            IEventPublisher eventPublisher,
            IMapper mapper,
            ILogger<SaleService> logger)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SaleDto> GetByIdAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<IEnumerable<SaleDto>> GetAllAsync()
        {
            var sales = await _saleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<SaleDto> GetSaleWithDetailsAsync(Guid id)
        {
            var sale = await _saleRepository.GetSaleWithDetailsAsync(id);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByCustomerAsync(Guid customerId)
        {
            var sales = await _saleRepository.GetSalesByCustomerAsync(customerId);
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByBranchAsync(Guid branchId)
        {
            var sales = await _saleRepository.GetSalesByBranchAsync(branchId);
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleRepository.GetSalesByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<SaleDto> AddAsync(CreateSaleRequest request)
        {
            try
            {
                // Validar e obter Customer
                var customer = await _customerRepository.GetByExternalIdAsync(request.CustomerExternalId);
                if (customer == null)
                    throw new ArgumentException($"Customer with external ID {request.CustomerExternalId} not found");

                // Validar e obter Branch
                var branch = await _branchRepository.GetByExternalIdAsync(request.BranchExternalId);
                if (branch == null)
                    throw new ArgumentException($"Branch with external ID {request.BranchExternalId} not found");

                // Gerar número da venda
                var saleNumber = await _saleRepository.GenerateSaleNumberAsync();

                // Criar a venda
                var sale = new Sale(saleNumber, customer, branch);

                // Adicionar itens
                foreach (var itemRequest in request.Items)
                {
                    var product = await _productRepository.GetByExternalIdAsync(itemRequest.ProductExternalId);
                    if (product == null)
                        throw new ArgumentException($"Product with external ID {itemRequest.ProductExternalId} not found");

                    var unitPrice = new Money(itemRequest.UnitPrice);
                    sale.AddItem(product, itemRequest.Quantity, unitPrice);
                }

                // Persistir a venda
                await _saleRepository.AddAsync(sale);
                await _saleRepository.SaveChangesAsync();

                // Publicar evento
                await _eventPublisher.PublishAsync(new SaleCreatedEvent(sale));

                // Retornar DTO da venda criada
                return _mapper.Map<SaleDto>(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sale");
                throw;
            }
        }

        public async Task<SaleDto> UpdateAsync(UpdateSaleRequest request)
        {
            try
            {
                // Obter a venda com itens
                var sale = await _saleRepository.GetSaleWithDetailsAsync(request.SaleId);
                if (sale == null)
                    throw new ArgumentException($"Sale with ID {request.SaleId} not found");

                if (sale.Cancelled)
                    throw new InvalidOperationException("Cannot update a cancelled sale");

                // Atualizar itens
                foreach (var itemUpdate in request.Items)
                {
                    sale.UpdateItem(itemUpdate.ItemId, itemUpdate.Quantity);
                }

                // Persistir as mudanças
                await _saleRepository.UpdateAsync(sale);
                await _saleRepository.SaveChangesAsync();

                // Publicar evento
                await _eventPublisher.PublishAsync(new SaleModifiedEvent(sale));

                // Retornar DTO da venda atualizada
                return _mapper.Map<SaleDto>(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sale");
                throw;
            }
        }

        public async Task<bool> CancelSaleAsync(CancelSaleRequest request)
        {
            try
            {
                // Obter a venda
                var sale = await _saleRepository.GetSaleWithDetailsAsync(request.SaleId);
                if (sale == null)
                    throw new ArgumentException($"Sale with ID {request.SaleId} not found");

                if (sale.Cancelled)
                    return true; // Já está cancelada

                // Cancelar a venda
                sale.Cancel();

                // Persistir as mudanças
                await _saleRepository.UpdateAsync(sale);
                await _saleRepository.SaveChangesAsync();

                // Publicar evento
                await _eventPublisher.PublishAsync(new SaleCancelledEvent(sale, request.Reason));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling sale");
                throw;
            }
        }

        public async Task<bool> CancelSaleItemAsync(CancelSaleItemRequest request)
        {
            try
            {
                // Obter a venda com itens
                var sale = await _saleRepository.GetSaleWithDetailsAsync(request.SaleId);
                if (sale == null)
                    throw new ArgumentException($"Sale with ID {request.SaleId} not found");

                if (sale.Cancelled)
                    throw new InvalidOperationException("Cannot cancel an item from an already cancelled sale");

                // Obter o item
                var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId);
                if (item == null)
                    throw new ArgumentException($"Item with ID {request.ItemId} not found in sale {request.SaleId}");

                if (item.Cancelled)
                    return true; // Já está cancelado

                // Cancelar o item
                sale.CancelItem(request.ItemId);

                // Persistir as mudanças
                await _saleRepository.UpdateAsync(sale);
                await _saleRepository.SaveChangesAsync();

                // Publicar evento
                await _eventPublisher.PublishAsync(new ItemCancelledEvent(sale, item, request.Reason));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling sale item");
                throw;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            try
            {
                // Em vez de remover, vamos cancelar a venda
                var sale = await _saleRepository.GetByIdAsync(id);
                if (sale == null)
                    return false;

                if (sale.Cancelled)
                    return true; // Já está cancelada

                sale.Cancel();
                await _saleRepository.UpdateAsync(sale);
                await _saleRepository.SaveChangesAsync();

                // Publicar evento
                await _eventPublisher.PublishAsync(new SaleCancelledEvent(sale, "Sale removed from system"));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing sale");
                throw;
            }
        }
    }
}
