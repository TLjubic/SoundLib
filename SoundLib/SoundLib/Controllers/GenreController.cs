using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SoundLib.Controllers
{
    public class GenreController : Controller
    {
        private SoundLibDbContext context;

        public GenreController(SoundLibDbContext _context)
        {
            this.context = _context;
        }

       
        

        public IActionResult Details(string id)
        {
            var albums = this.context.Albums.Include(a => a.Artist).Include(s => s.Songs)
                .Where(p => p.Genre.Title.Contains(id));
            return View(albums.ToList());

        }

    }
}