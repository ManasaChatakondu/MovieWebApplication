﻿using System;
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
        public string ID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
    public class CinemaFilmVM
    {
        public string Title { get; set; }
        public string Poster { get; set; }
    }
    public class MovieDescModel
    {
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Price { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Rating { get; set; }

    }
}