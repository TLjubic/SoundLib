using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Model.DTO;

namespace Model.DTO
{
    public class SongDTO
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Rating { get; set; }

        public decimal Duration { get; set; }

        public ArtistDTO Artist { get; set; }

        public AlbumDTO Album { get; set; }


        public static Expression<Func<Song, SongDTO>> SelectorExpression { get; } = p => new SongDTO()
        {
            Id = p.Id,
            Title = p.Title,
            Rating = p.Rating,
            Duration = p.Duration,
            Artist = new ArtistDTO()
            {
                Id = p.Artist.Id,
                FirstName = p.Artist.FirstName,
                Address = p.Artist.Address,
                City = p.Artist.City
            },
            Album = new AlbumDTO()
            {
                Id = p.Album.Id,
                Title = p.Album.Title,
                Rating = p.Album.Rating
            }

        };

    }
}
