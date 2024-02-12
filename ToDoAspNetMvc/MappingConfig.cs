using AutoMapper;
using ToDoAspNetMvc.ViewModels;
using ToDoListLibrary;

namespace ToDoAspNetMvc;

public class MappingConfig
{
    public static  MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ToDoEntryViewModel, ToDoEntry>().ReverseMap();
            config.CreateMap<CustomFieldViewModel, CustomField>().ReverseMap();            
        });
        return mappingConfig;
    }
}
