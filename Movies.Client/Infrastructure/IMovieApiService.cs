using Movies.Client.Models;

namespace Movies.Client.Infrastructure;

public interface IMovieApiService
{
    Task<IEnumerable<Movie>> GetMoviesAsync();
    
    Task<Movie> GetMovieAsync(string id);
    
    Task<Movie> CreateMovieAsync(Movie movie);
    
    Task<Movie> UpdateMovieAsync(Movie movie);
    
    Task DeleteMovieAsync(int id);
    
    Task<UserInfoViewModel> GetUserInfoAsync();
}