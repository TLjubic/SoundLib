using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Model;

namespace Model.DTO
{
    public class ArtistDTO
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }


        public static Expression<Func<Artist, ArtistDTO>> SelectorExpression { get; } = p => new ArtistDTO()
        {
            Id = p.Id,
            FirstName = p.FirstName,
            Address = p.Address,
            City = p.City
        };
    }
}
