using AutoMapper;
using Auditory.API.Domain;

namespace Auditory.API.Data.Mappers
{
    public class UserRecordHistoryProfile : Profile
    {
        public UserRecordHistoryProfile()
        {
            CreateMap<UserRecordHistory, UserRecordHistoryMongo>().ReverseMap();
        }
    }
}
