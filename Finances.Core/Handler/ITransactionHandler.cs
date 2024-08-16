using Finances.Core.Models;
using Finances.Core.Requests.Transactions;
using Finances.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Handler
{
    public interface ITransactionHandler
    {
        Task<Response<Transactions?>> CreateAsync(CreateTransactionRequest request);
        Task<Response<Transactions?>> UpdateAsync(UpdateTransactionRequest request);
        Task<Response<Transactions?>> DeleteAsync(DeleteTransactionRequest request);
        Task<Response<Transactions?>> GetByIdAsync(GetTransactionByIdRequest request);
        Task<PagedResponse<List<Transactions?>>> GetByPeriodAsync(GetTransactionByPeriodRequest request);
    }
}
