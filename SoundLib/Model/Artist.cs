using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using Model.DTO;

namespace Model
{
    public class Artist
    {

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }

        public string Description { get; set; }

        public IEnumerable<Album> Albums { get; set; }

        public string FullName => $"{FirstName} {LastName}";

    }
}
