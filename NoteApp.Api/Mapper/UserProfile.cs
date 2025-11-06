using AutoMapper;
using NoteApp.Api.Domain.Dtos.AuthDtos;
using NoteApp.Api.Domain.Dtos.UserDtos;
using NoteApp.Api.Domain.Entities;

namespace NoteApp.Api.Mapper;
public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<User, UserForCreationDto>().ReverseMap();
		CreateMap<User, UserForResultDto>().ReverseMap();
		CreateMap<User, UserForUpdateDto>().ReverseMap();
		CreateMap<User, LoginRequest>().ReverseMap();
		CreateMap<RegisterRequest, User>().ReverseMap();
	}
}
