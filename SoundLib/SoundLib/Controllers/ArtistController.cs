using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;

namespace SoundLib.Controllers
{
    public class ArtistController : Controller
    {
        private SoundLibDbContext context;

        public ArtistController(SoundLibDbContext _context)
        {
            this.context = _context;
        }

        public IActionResult Index()
        {

            var artists = this.context.Artists.AsQueryable();

            return View(artists.ToList());

        }

        public IActionResult Details(int id)
        {
            var artist = context.Artists
                .Include(a => a.Albums)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return View(artist);
        }

        public IActionResult CreateArtist()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateArtist(Artist model)
        {
            if (ModelState.IsValid)
            {

                this.context.Artists.Add(model);
                this.context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}