
using Ghak.libraries.AppBase.DTO;
using Ghak.libraries.AppBase.Exceptions;
using Ghak.libraries.AppBase.Interfaces;
using Ghak.libraries.AppBase.Models;
using Ghak.libraries.AppBase.Utils;

namespace Ghak.libraries.AppBase.Interfaces;

public interface ICrudRepository<TKey, TModel, TDto, TModifyModel, TToModifyModel> 
    : IPrepareData<TModel> 
    where TToModifyModel : BaseToModifyDto<TModifyModel>
{
    Task<ApiResponse<PaginationList<TDto>>> Pagination(PaginationListArgs request,
        CancellationToken cancellationToken);

    Task<ApiResponse<List<ListItem<TKey>>>> List(string? searchQuery = null, Dictionary<string, object>? args = null);
    Task<ApiResponse<TDto>> Get(TKey id);
    Task ModifyValidation(TModel record, TModifyModel request);
    Task<ApiResponse<TModifyModel>> Create(TModifyModel request);
    Task<ApiResponse<TModifyModel>> Update(TModifyModel request);
    Task<ApiResponse<TDto>> UpdateActivation(TKey id);
    Task<ApiResponse<TToModifyModel>> InitRecordModification(TKey? id);
    Task<ApiResponse<bool>> Delete(TKey id);
    Task<bool> SaveDbChange();
}