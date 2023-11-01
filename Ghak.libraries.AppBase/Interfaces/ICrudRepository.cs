using Ghak.libraries.AppBase.DTO;
using Ghak.libraries.AppBase.Models;
using Ghak.libraries.AppBase.Utils;

namespace Ghak.libraries.AppBase.Interfaces;

public interface ICrudRepository<TKey, TModel, TDto, in TModifyModel, TToModifyModel> 
    : IPrepareData<TModel> 
    where TToModifyModel : BaseToModifyDto<TModifyModel>
{
    Task<ApiResponse<PaginationList<TDto>>> Pagination(PaginationListArgs request);

    Task<ApiResponse<List<ListItem<TKey>>>> List(string? searchQuery = null, Dictionary<string, object>? args = null);
    Task<ApiResponse<TDto>> Get(TKey id);
    Task ModifyValidation(TModel record, TModifyModel request);
    Task<ApiResponse<TToModifyModel>> PrepareModification(TKey? id);
    Task<ApiResponse<TDto>> Create(TModifyModel request);
    Task<ApiResponse<TDto>> Update(TModifyModel request);
    Task<ApiResponse<bool>> UpdateActivation(TKey id);
    Task<ApiResponse<bool>> Delete(TKey id);
    Task<bool> SaveDbChange();
}