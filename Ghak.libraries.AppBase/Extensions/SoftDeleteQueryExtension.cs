using System.Linq.Expressions;
using System.Reflection;
using Ghak.libraries.AppBase.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ghak.libraries.AppBase.Extensions;

public static class SoftDeleteQueryExtension
{
    public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData)
    {
        var methodToCall = typeof(SoftDeleteQueryExtension)
            .GetMethod(nameof(GetSoftDeleteFilter),
                BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(entityData.ClrType);
        var filter = methodToCall.Invoke(null, new object[] { });
        entityData.SetQueryFilter((LambdaExpression) filter!);
        entityData.AddIndex(entityData.FindProperty(nameof(ISoftDelete.DeletedAt))!);
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : class, ISoftDelete
    {
        Expression<Func<TEntity, bool>> filter = x => x.DeletedAt == null;
        return filter;
    }
}