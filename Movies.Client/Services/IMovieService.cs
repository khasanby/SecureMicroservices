using Movies.Client.Models;

namespace Movies.Client.Services;

public interface IMovieService
{
    public Task<IEnumerable<Movie>> GetMoviesAsync();

    public Task<Movie> GetMovieAsync(string id);
    
    public Task<Movie> CreateMovieAsync(Movie movie);
    
    public Task<Movie> UpdateMovieAsync(Movie movie);
    
    public Task DeleteMovieAsync(int id);
    
    public Task<UserInfoViewModel> GetUserInfoAsync();
}