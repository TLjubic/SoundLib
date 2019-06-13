using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Model.DTO
{
    public class AlbumDTO
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Rating { get; set; }

        public static Expression<Func<Album, AlbumDTO>> SelectorExpression { get; } = p => new AlbumDTO()
        {
            Id = p.Id,
            Title = p.Title,
            Rating = p.Rating
        };

    }
}
