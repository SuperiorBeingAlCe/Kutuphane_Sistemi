using AutoMapper;
using kitapsin.Server.Models;
using kitapsin.Server.Dto;

namespace kitapsin.Server.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, DtoUserResponse>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.phoneNumber));
            CreateMap<DtoUserCreate, User>();
            CreateMap<Loan, DtoLoanResponse>();
            CreateMap<Penalty, DtoPenaltyResponse>();
            CreateMap<Book, DtoBookResponse>();
        }

    }
}