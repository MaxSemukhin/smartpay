using AutoMapper;

namespace SmartPay.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {			
        // CreateMap<ToDoTask, DbToDoTask>()
        //     .ForMember(t => t.Id, opt => opt.Ignore())
        //     .ForMember(t => t.User, opt => opt.Ignore());
        //
        // CreateMap<DbToDoTask, ToDoTask>();
        //
        // CreateMap<ToDoHistoryEntry, DbToDoHistoryEntry>()
        //     .ForMember(t => t.User, opt => opt.Ignore());
        //
        // CreateMap<DbToDoHistoryEntry, ToDoHistoryEntry>();
    }
}