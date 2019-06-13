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
    public class SongController : Controller
    {
        private SoundLibDbContext context;

        public SongController(SoundLibDbContext _context)
        {
            this.context = _context;
        }

        public IActionResult Index()
        {

            var songs = this.context.Songs.Include(g => g.Album).Include(a => a.Artist).AsQueryable();

            return View(songs.ToList());

        }

        [HttpPost]
        public ActionResult Search(Song filter)
        {
            var songQuery = this.context.Songs.Include(a => a.Album).Include(p => p.Artist).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                songQuery = songQuery.Where(p => p.Title.Contains(filter.Title.ToLower()));

            var model = songQuery.ToList();

            return PartialView("_SongList", model);
        }

        public IActionResult Details(int id)
        {
            var song = context.Songs
                .Include(g => g.Album)
                .Include(a => a.Artist)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return View(song);
        }

        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Song model)
        {
            if (ModelState.IsValid)
            {
                
                this.context.Songs.Add(model);
                this.context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();
            return View(model);
        }

        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
            this.FillDropdownValues();
            return View(this.context.Songs.FirstOrDefault(p => p.Id == id));
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            var song = this.context.Songs.FirstOrDefault(p => p.Id == id);

            var ok = await this.TryUpdateModelAsync(song);

            if (ok)
            {
                this.context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();

            return View(song);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var deletedSong = context.Songs.FirstOrDefault(x => x.Id == id);
            context.Songs.Remove(deletedSong);
            context.SaveChanges();

            return RedirectToAction("Index");
        }


        private void FillDropdownValues()
        {
            var albums = new List<SelectListItem>();
            var artists = new List<SelectListItem>();

            //Dropdown za albume
            var listItem = new SelectListItem();
            listItem.Text = "- Album -";
            listItem.Value = "";
            albums.Add(listItem);

            foreach (var album in context.Albums)
            {
                albums.Add(new SelectListItem()
                {
                    Value = "" + album.Id,
                    Text = album.Title
                });
            }

            //Dropdown za izvodace
            var listItemArtist = new SelectListItem();
            listItemArtist.Text = "- Artist -";
            listItemArtist.Value = "";
            artists.Add(listItemArtist);

            foreach (var artist in context.Artists)
            {
                artists.Add(new SelectListItem()
                {
                    Value = "" + artist.Id,
                    Text = artist.FullName
                });
            }

            ViewBag.PossibleAlbums = albums;
            ViewBag.PossibleArtists = artists;
        }
    }
}