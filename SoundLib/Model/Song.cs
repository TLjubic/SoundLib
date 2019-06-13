using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Song
    {

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public decimal Rating { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
        public decimal Duration { get; set; }

        [Required]
        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        [ForeignKey(nameof(Album))]
        public int AlbumId { get; set; }
        public Album Album { get; set; }

    }
}
