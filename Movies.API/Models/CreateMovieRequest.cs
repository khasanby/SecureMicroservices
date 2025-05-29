namespace Movies.API.Models;

public sealed class CreateMovieRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the movie.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the movie.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the movie.
    /// </summary>
    public string Genre { get; set; }

    /// <summary>
    /// Gets or sets the rating of the movie.
    /// </summary>
    public string Rating { get; set; }

    /// <summary>
    /// Gets or sets the release date of the movie.
    /// </summary>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the URL of the movie's image.
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the owner of the movie.
    /// </summary>
    public string Owner { get; set; }
}