using System.Reflection;

using Domain.Core.Models;

namespace Infrastructure.Core.Extend;

public static   class EFEntityInfo
{
    public static IEnumerable<Type> GetEntityTypes(Assembly assembly)
    {
        var efEntities = assembly.GetTypes()
            .Where(m => m.FullName != null 
            && Array.Exists(m.GetInterfaces(), t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEntity<>))
            && !m.IsAbstract && !m.IsInterface)
            .ToArray();

        return efEntities;
    }
}
