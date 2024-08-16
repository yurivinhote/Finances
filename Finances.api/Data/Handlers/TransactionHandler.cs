using Finances.api.Data;
using Finances.Core.Common;
using Finances.Core.Handler;
using Finances.Core.Models;
using Finances.Core.Requests.Transactions;
using Finances.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Finances.Core.Handlers
{
    public class TransactionHandler(AppDbContext dbContext) : ITransactionHandler
    {
        public async Task<Response<Transactions?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: Finances.Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                var transaction = new Transactions
                {
                    UserId = request.UserId,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.Now,
                    Amount = request.Amount,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    Title = request.Title,
                    Type = request.Type
                };

                await dbContext.Transactions.AddAsync(transaction);
                await dbContext.SaveChangesAsync();

                return new Response<Transactions?>(transaction, 201, "Transação criada com sucesso!");
            }
            catch
            {
                return new Response<Transactions?>(null, 500, "Não foi possível criar sua transação");
            }
        }

        public async Task<Response<Transactions?>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is { Type: Finances.Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                var transaction = await dbContext
                    .Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transactions?>(null, 404, "Transação não encontrada");

                transaction.CategoryId = request.CategoryId;
                transaction.Amount = request.Amount;
                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

                dbContext.Transactions.Update(transaction);
                await dbContext.SaveChangesAsync();

                return new Response<Transactions?>(transaction);
            }
            catch
            {
                return new Response<Transactions?>(null, 500, "Não foi possível recuperar sua transação");
            }
        }

        public async Task<Response<Transactions?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await dbContext
                    .Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transactions?>(null, 404, "Transação não encontrada");

                dbContext.Transactions.Remove(transaction);
                await dbContext.SaveChangesAsync();

                return new Response<Transactions?>(transaction);
            }
            catch
            {
                return new Response<Transactions?>(null, 500, "Não foi possível recuperar sua transação");
            }
        }

        public async Task<Response<Transactions?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await dbContext
                    .Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return transaction is null
                    ? new Response<Transactions?>(null, 404, "Transação não encontrada")
                    : new Response<Transactions?>(transaction);
            }
            catch
            {
                return new Response<Transactions?>(null, 500, "Não foi possível recuperar sua transação");
            }
        }

        public async Task<PagedResponse<List<Transactions>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDay();
                request.EndDate ??= DateTime.Now.GetLastDay();
            }
            catch
            {
                return new PagedResponse<List<Transactions>?>(null, 500);
            }

            try
            {
                var query = dbContext
                    .Transactions
                    .AsNoTracking()
                    .Where(x =>
                        x.PaidOrReceivedAt >= request.StartDate &&
                        x.PaidOrReceivedAt <= request.EndDate &&
                        x.UserId == request.UserId)
                    .OrderBy(x => x.PaidOrReceivedAt);

                var transactions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query.CountAsync();

                return new PagedResponse<List<Transactions>?>(
                    transactions,
                    count,
                    request.PageNumber,
                    request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Transactions>?>(null, 500);
            }
        }
    }
}
