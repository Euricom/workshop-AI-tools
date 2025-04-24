using AutoMapper;

namespace MyApp.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}

public interface IMapTo<T>
{
    void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
}

public abstract class MappingProfile : Profile
{
    protected MappingProfile()
    {
        ApplyMappingsFromAssembly(typeof(IMapFrom<>).Assembly);
    }

    private void ApplyMappingsFromAssembly(System.Reflection.Assembly assembly)
    {
        var mapFromTypes = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && 
                    (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) ||
                     i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
            .ToList();

        foreach (var type in mapFromTypes)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping") 
                ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping")
                ?? type.GetInterface("IMapTo`1")?.GetMethod("Mapping");
            
            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}