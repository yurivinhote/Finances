using Finances.Core.Handler;
using Finances.Core.Models;
using Finances.Core.Requests.Categories;
using Finances.Core.Responses;
using Microsoft.EntityFrameworkCore;
using NMemory.Linq;

namespace Finances.api.Data.Handlers
{
    public class CategoryHandler(AppDbContext context) : ICategoryHandler
    {
        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };

            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erro ao incluir em banco de dados.");
                Console.WriteLine(ex.Message);
                return new Response<Category?>(null, 500, "Não foi possível criar categoria.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response<Category?>(null, 500, "Não foi possível criar categoria.");
            }

            return new Response<Category?>(category, 201, "Categoria criada com sucesso.");
        } //Feito

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (category is null)
                    return new Response<Category?>(null, 404, "Categoria não encontrada");

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(category, message: " Categoria excluída com sucesso.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erro ao incluir em banco de dados.");
                Console.WriteLine(ex.Message);
                return new Response<Category?>(null, 500, "Não foi possível deletar a categoria.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response<Category?>(null, 500, "Não foi possível deletar a categoria.");
            }
        } //Feito

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            try
            {
                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return category is null
                    ? new Response<Category?>(null, 404, "Categoria não encontrada")
                    : new Response<Category?>(category);
            }
            catch
            {
                return new Response<Category?>(null,500,"Erro ao buscar categoria.");
            }
        }//Feotp

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (category is null)
                    return new Response<Category?>(null, 404, "Categoria não encontrada");

                category.Title = request.Title;
                category.Description = request.Description;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(category,message:" Categoria atualizada com sucesso.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erro ao incluir em banco de dados.");
                Console.WriteLine(ex.Message);
                return new Response<Category?>(null, 500, "Não foi possível atualizar categoria.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response<Category?>(null, 500, "Não foi possível atualizar categoria.");
            }
        } //Feito

        public async Task<PagedResponse<List<Category?>>> GetAllAsync(GetAllCategoriesRequest request)
        {
            try { 
            var query =
                context.Categories.
                AsNoTracking().
                Where(x => x.UserId == request.UserId).
                OrderBy(x => x.Title);

            var categories = await query
                 .Skip((request.PageNumber - 1) * request.PageSize) //0
                 .Take(request.PageSize) //25
                 .ToListAsync();
            
            var count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new PagedResponse<List<Category>?>(null, 0);
            }
        }
    }
}
