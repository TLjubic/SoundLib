using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;

namespace SoundLib.Controllers
{
    public class AlbumController : Controller
    {
        private SoundLibDbContext context;

        public AlbumController(SoundLibDbContext _context)
        {
            this.context = _context;
        }

        public IActionResult Index()
        {

            var albums = this.context.Albums.Include(a => a.Artist).Include(s => s.Songs).Include(g => g.Genre).AsQueryable();

            return View(albums.ToList());

        }

        public IActionResult Details(int id)
        {
            var album = context.Albums
                .Include(a => a.Artist)
                .Include(s => s.Songs)
                .Include(g => g.Genre)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return View(album);
        }

        [ActionName("CreateAlbum")]
        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }

        [HttpPost, ActionName("CreateAlbum")]
        public IActionResult Create(Album model)
        {
            if (ModelState.IsValid)
            {
               

                this.context.Albums.Add(model);
                this.context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();
            return View(model);
        }

        public IActionResult Search(string id)
        {
            var albums = this.context.Albums.Include(a => a.Artist).Include(s => s.Songs)
                .Where(p => p.Genre.Title.Contains(id));
            return View("Index", albums.ToList());

        }

        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
            this.FillDropdownValues();
            return View(this.context.Albums.FirstOrDefault(p => p.Id == id));
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            var album = this.context.Albums.FirstOrDefault(p => p.Id == id);

            var ok = await this.TryUpdateModelAsync(album);

            if (ok)
            {
                this.context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();

            return View(album);
        }


        private void FillDropdownValues()
        {
            var artists = new List<SelectListItem>();
            var genres = new List<SelectListItem>();

            //Dropdown za izvodace
            var listItem = new SelectListItem();
            listItem.Text = "- Artist -";
            listItem.Value = "";
            artists.Add(listItem);

            foreach (var artist in context.Artists)
            {
                artists.Add(new SelectListItem()
                {
                    Value = "" + artist.Id,
                    Text = artist.FullName
                });
            }

            //Dropdown za zanr
            var listItemGenres = new SelectListItem();
            listItemGenres.Text = "- Genre -";
            listItemGenres.Value = "";
            genres.Add(listItemGenres);

            foreach (var genre in context.Genres)
            {
                genres.Add(new SelectListItem()
                {
                    Value = "" + genre.Id,
                    Text = genre.Title
                });
            }

            ViewBag.PossibleArtists = artists;
            ViewBag.PossibleGenres = genres;
        }
    }
}