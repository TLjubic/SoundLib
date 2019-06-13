using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Album
    {

        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }
        public decimal Rating { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public string Description { get; set; }

        public IEnumerable<Song> Songs { get; set; }

    }
}


