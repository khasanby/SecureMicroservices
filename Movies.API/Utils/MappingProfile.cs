using AutoMapper;
using Movies.API.Entities;
using Movies.API.Models;

namespace Movies.API.Utilsl;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, GetMovieResponse>().ReverseMap();
        CreateMap<CreateMovieRequest, Movie>().ReverseMap();
    }
}
