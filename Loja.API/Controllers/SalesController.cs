using Microsoft.AspNetCore.Mvc;
using Loja.Application.DTOs;
using Loja.Application.DTOs.Request;
using Loja.Application.Interfaces;

namespace Loja.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISaleService saleService, ILogger<SalesController> logger)
        {
            _saleService = saleService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _saleService.GetAllAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sale = await _saleService.GetSaleWithDetailsAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomer(Guid customerId)
        {
            var sales = await _saleService.GetSalesByCustomerAsync(customerId);
            return Ok(sales);
        }

        [HttpGet("branch/{branchId}")]
        [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBranch(Guid branchId)
        {
            var sales = await _saleService.GetSalesByBranchAsync(branchId);
            return Ok(sales);
        }

        [HttpGet("date-range")]
        [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("Start date must be before or equal to end date");

            var sales = await _saleService.GetSalesByDateRangeAsync(startDate, endDate);
            return Ok(sales);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSaleRequest request)
        {
            try
            {
                var sale = await _saleService.AddAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sale");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the sale");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateSaleRequest request)
        {
            try
            {
                var sale = await _saleService.UpdateAsync(request);
                return Ok(sale);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sale");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the sale");
            }
        }

        [HttpPost("cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSale([FromBody] CancelSaleRequest request)
        {
            try
            {
                var result = await _saleService.CancelSaleAsync(request);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling sale");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while cancelling the sale");
            }
        }

        [HttpPost("cancel-item")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSaleItem([FromBody] CancelSaleItemRequest request)
        {
            try
            {
                var result = await _saleService.CancelSaleItemAsync(request);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling sale item");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while cancelling the sale item");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _saleService.RemoveAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sale");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the sale");
            }
        }
    }
}
