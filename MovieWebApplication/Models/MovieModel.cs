using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieWebApplication.Models
{
    public class MoviesList
    {
        public List<MovieModel> Movies { get; set; }
       
    }
    public class MovieModel
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}