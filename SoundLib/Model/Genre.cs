using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Genre
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public IEnumerable<Album> Albums { get; set; }

    }
}
