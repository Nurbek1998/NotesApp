using AutoMapper;
using NoteApp.Api.Domain.Dtos.NoteDtos;
using NoteApp.Api.Domain.Entities;

namespace NoteApp.Api.Mapper;
public class NoteProfile : Profile
{
	public NoteProfile()
	{
		CreateMap<Note, NoteForCreationDto>().ReverseMap();
		CreateMap<Note, NoteForResultDto>();
		CreateMap<NoteForUpdateDto, Note>();
	}
}
