using Finances.Core.Models;
using Finances.Core.Requests.Categories;
using Finances.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Handler
{
    public interface ICategoryHandler
    {
        Task<Response<Category?>> CreateAsync(CreateCategoryRequest request);
        Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request);
        Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request);
        Task<Response<Category?>> GetByIdAsync( GetCategoryByIdRequest request);
        Task<PagedResponse<List<Category?>>> GetAllAsync(GetAllCategoriesRequest request);

    }
}
