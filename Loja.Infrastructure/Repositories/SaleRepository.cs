using Microsoft.EntityFrameworkCore;
using Loja.Domain.Entities;
using Loja.Domain.Interfaces;
using Loja.Infrastructure.Data;

namespace Loja.Infrastructure.Repositories
{
    public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
    {
        public SaleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Sale> GetSaleWithDetailsAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetSalesByCustomerAsync(Guid customerId)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Where(s => s.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetSalesByBranchAsync(Guid branchId)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Where(s => s.BranchId == branchId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();
        }

        public async Task<string> GenerateSaleNumberAsync()
        {
            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var lastSaleToday = await _context.Sales
                .Where(s => s.SaleNumber.StartsWith(today))
                .OrderByDescending(s => s.SaleNumber)
                .FirstOrDefaultAsync();

            int sequence = 1;
            if (lastSaleToday != null)
            {
                var lastSequenceStr = lastSaleToday.SaleNumber.Substring(today.Length);
                if (int.TryParse(lastSequenceStr, out int lastSequence))
                {
                    sequence = lastSequence + 1;
                }
            }

            return $"{today}{sequence:D6}";
        }
    }
}
