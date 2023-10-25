
using Ghak.libraries.AppBase.Exceptions;
using Ghak.libraries.AppBase.Interfaces;
using Ghak.libraries.AppBase.Models;
using Ghak.libraries.AppBase.Utils;

namespace Ghak.libraries.AppBase.Interfaces;

public interface ICrudRepository<TModel, TDto, TModifyModel> : IPrepareData<TModel>
{
    Task<ApiResponse<PaginationList<TDto>>> Pagination(PaginationListArgs request,
        CancellationToken cancellationToken);

    Task<ApiResponse<List<ListItem>>> List(string? searchQuery = null, Dictionary<string, object>? args = null);
    Task<ApiResponse<TDto>> Get(string id);
    Task ModifyValidation(TModel record, TModifyModel request);
    Task<ApiResponse<TModifyModel>> Create(TModifyModel request);
    Task<ApiResponse<TModifyModel>> Update(TModifyModel request);
    Task<ApiResponse<TDto>> UpdateActivation(string id);
    Task<ApiResponse<TModifyModel>> GetModifyRecord(string id);
    Task<ApiResponse<bool>> Delete(string id);
    Task<bool> SaveDbChange();

    
}