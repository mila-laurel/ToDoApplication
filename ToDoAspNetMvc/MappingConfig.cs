using AutoMapper;
using ToDoAspNetMvc.Models;
using ToDoListLibrary;

namespace ToDoAspNetMvc;

public class MappingConfig
{
    public static  MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ToDoEntryViewModel, ToDoEntry>().ForMember(e => e.Fields, opt => opt.Ignore());
            config.CreateMap<ToDoEntry, ToDoEntryViewModel>();
            config.CreateMap<CustomFieldViewModel, CustomField>().ReverseMap();            
        });
        return mappingConfig;
    }
}
