﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using Movies.Client.Infrastructure;
using Movies.Client.Models;
using Microsoft.AspNetCore.Authentication;

namespace Movies.Client.Controllers;

[Authorize]
public class MoviesController : Controller
{
    private readonly IMovieApiService _movieApiService;

    public MoviesController(IMovieApiService movieApiService)
    {
        _movieApiService = movieApiService ?? throw new ArgumentNullException(nameof(movieApiService));
    }

    // GET: Movies
    public async Task<IActionResult> Index()
    {
        await LogTokenAndClaims();
        return View(await _movieApiService.GetMoviesAsync());
    }
    public async Task LogTokenAndClaims()
    {
        var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

        Debug.WriteLine($"Identity token: {identityToken}");

        foreach (var claim in User.Claims)
        {
            Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
        }
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> OnlyAdmin()
    {
        var userInfo = await _movieApiService.GetUserInfoAsync();
        return View(userInfo);
    }


    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        return View();

        //if (id == null)
        //{
        //    return NotFound();
        //}

        //var movie = await _context.Movie
        //    .FirstOrDefaultAsync(m => m.Id == id);
        //if (movie == null)
        //{
        //    return NotFound();
        //}

        //return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Movies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Genre,ReleaseDate,Owner")] Movie movie)
    {
        return View();

        //if (ModelState.IsValid)
        //{
        //    _context.Add(movie);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //return View(movie);
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        return View();

        //if (id == null)
        //{
        //    return NotFound();
        //}

        //var movie = await _context.Movie.FindAsync(id);
        //if (movie == null)
        //{
        //    return NotFound();
        //}
        //return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,ReleaseDate,Owner")] Movie movie)
    {
        return View();

        //if (id != movie.Id)
        //{
        //    return NotFound();
        //}

        //if (ModelState.IsValid)
        //{
        //    try
        //    {
        //        _context.Update(movie);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MovieExists(movie.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        //return View(movie);
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        return View();

        //if (id == null)
        //{
        //    return NotFound();
        //}

        //var movie = await _context.Movie
        //    .FirstOrDefaultAsync(m => m.Id == id);
        //if (movie == null)
        //{
        //    return NotFound();
        //}

        //return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        return View();

        //var movie = await _context.Movie.FindAsync(id);
        //_context.Movie.Remove(movie);
        //await _context.SaveChangesAsync();
        //return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return true;

        //return _context.Movie.Any(e => e.Id == id);
    }

    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }

}