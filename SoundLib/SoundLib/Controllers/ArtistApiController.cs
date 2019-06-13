using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Newtonsoft.Json.Linq;

namespace SoundLib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistApiController : Controller
    {

        private SoundLibDbContext context;

        public ArtistApiController(SoundLibDbContext _context)
        {
            this.context = _context;
        }

        public ActionResult<List<ArtistDTO>> Get()
        {
            var artists = this.context.Artists
                .Select(ArtistDTO.SelectorExpression)
                .ToList();

            return artists;
        }

        [Route("{id:int}")]
        public ActionResult<ArtistDTO> Get(int id)
        {
            var artist = this.context.Artists
                .Where(a => a.Id == id)
                .Select(ArtistDTO.SelectorExpression)
                .FirstOrDefault();

            return artist;
        }

        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Artists.Add(artist);
            await context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = artist.Id }, artist);

        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<ActionResult<ArtistDTO>> Put(int id, [FromBody] JObject model)
        {
            var valueProvider = new ObjectSourceValueProvider(model);

            var existing = context.Artists.FirstOrDefault(p => p.Id == id);
            if (existing != null && ModelState.IsValid && await TryUpdateModelAsync(existing, "", valueProvider))
            {
                this.context.SaveChanges();
                return Get(id);
            }

            if (existing == null)
            {
                ModelState.AddModelError("id", "Invalid client ID");
            }

            return BadRequest(ModelState);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var existing = context.Artists.FirstOrDefault(p => p.Id == id);
            if (existing != null)
            {
                this.context.Entry(existing).State = EntityState.Deleted;
                this.context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new { error = "Unable to locate client with provided ID", providedID = id });
            }
        }
    }

    public class ObjectSourceValueProvider : IValueProvider
    {
        private JObject _x;

        public ObjectSourceValueProvider(JObject x)
        {
            this._x = x;
        }

        public bool ContainsPrefix(string prefix)
        {
            return _x.Properties().Any(p => p.Name == prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            var prop = _x.Properties().Where(p => p.Name.ToLower() == key?.ToLower()).FirstOrDefault();

            if (prop == null)
            {
                return ValueProviderResult.None;
            }
            return new ValueProviderResult(prop.Value.ToString());
        }
    }
}