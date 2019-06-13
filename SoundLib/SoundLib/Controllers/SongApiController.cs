using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
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
    public class SongApiController : Controller
    {

        private SoundLibDbContext context;

        public SongApiController(SoundLibDbContext _context)
        {
            this.context = _context;
        }

        public ActionResult<List<SongDTO>> Get()
        {
            var songs = this.context.Songs
                .Select(SongDTO.SelectorExpression)
                .ToList();

            return songs;
        }

        [Route("{id:int}")]
        public ActionResult<SongDTO> Get(int id)
        {
            var song = this.context.Songs
                .Where(s => s.Id == id)
                .Select(SongDTO.SelectorExpression)
                .FirstOrDefault();

            return song;
        }


        /*
        [Route("")]
        [HttpPost]
        public ActionResult<SongDTO> Post(Song s)
        {
            if (ModelState.IsValid)
            {

                this.context.Songs.Add(s);
                this.context.SaveChanges();

                return Get(s.Id);
            }

            return BadRequest(ModelState);
        }

    */


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Song song)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Songs.Add(song);
            await context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = song.Id }, song);

        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<ActionResult<SongDTO>> Put(int id, [FromBody] JObject model)
        {
            var valueProvider = new ObjectSourceValueProvider(model);

            var existing = context.Songs.FirstOrDefault(p => p.Id == id);
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

        private Task<bool> TryUpdateModelAsync(Song existing, string v, ObjectSourceValueProvider valueProvider)
        {
            throw new NotImplementedException();
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var existing = context.Songs.FirstOrDefault(p => p.Id == id);
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


    
}


/*


[HttpPut("{id}")]
public async Task<IActionResult> PutClient([FromRoute] int id, [FromBody] Client client)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    if (id != client.ID)
    {
        return BadRequest();
    }

    _context.Entry(client).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!ClientExists(id))
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
public async Task<IActionResult> PostClient([FromBody] Client client)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Clients.Add(client);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetClient", new { id = client.ID }, client);

}

    */