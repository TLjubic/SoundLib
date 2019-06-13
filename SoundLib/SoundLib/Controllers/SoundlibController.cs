using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SoundLib.Controllers
{
    public class SoundlibController : Controller
    {
        private SoundLibDbContext context;

        public SoundlibController(SoundLibDbContext _context)
        {
            this.context = _context;
        }

        public IActionResult Index()
        {

            var albums = this.context.Albums.Include(a => a.Artist).Include(s => s.Songs).Include(g => g.Genre).AsQueryable();

            return View(albums.ToList());

        }
    }
}