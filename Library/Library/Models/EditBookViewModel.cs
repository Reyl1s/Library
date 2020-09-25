using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class EditBookViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public string ImgPath { get; set; }
    }
}
