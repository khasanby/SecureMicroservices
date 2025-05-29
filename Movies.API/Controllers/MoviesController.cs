using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.API.DataContexts;
using Movies.API.Entities;
using Movies.API.Models;

namespace Movies.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize("ClientIdPolicy")]
public class MoviesController : ControllerBase
{
    private readonly MoviesDbContext _context;
    private readonly IMapper _mapper;

    public MoviesController(MoviesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<GetMovieResponse[]>> GetMoviesAsync()
    {
        Movie[] movies = await _context.Movies.ToArrayAsync();
        var response = _mapper.Map<GetMovieResponse[]>(movies);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetMovieResponse>> GetMovieAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<GetMovieResponse>(movie));
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovieAsync(int id, CreateMovieRequest movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }
        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("Concurrency exception occurred while updating the movie.");
            if (!IsMovieExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> AddMovieAsync(CreateMovieRequest movie)
    {
        var newMovieDb = _mapper.Map<Movie>(movie);
        _context.Movies.Add(newMovieDb);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovieAsync", new { id = newMovieDb.Id }, movie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        movie.IsDeleted = true;
        await _context.SaveChangesAsync();

        return Ok();
    }

    private bool IsMovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}