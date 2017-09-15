using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieManager;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;
using MovieManager.API.Models;

namespace MovieManager.Tests.Controllers
{
    [TestClass]
    public class UtilTests
    {
        [TestMethod]
        [TestCategory("Constructive")]
        public void BuildDataBase()
        {
            string[] main_genres = new[] { "Action","Adventure","Animation","Comedy","Crime","Documentary","Drama","Family","Fantasy","History","Horror","Music","Mystery","Romance","Science Fictions","TV Movie","Thriller","War","Westerns"};
            string[] sub_genres = new[] { "Absurd", "Parody","Animal","Biker","Cavalry"};
            Director director;
            Genre main_genre;
            using (var db = new MovieContext())
            {
               
                // build out genres
                foreach (string genre in main_genres)
                {
                    var newGenre = new Genre()
                    {
                        Name = genre,
                    };
                    db.Genres.Add(newGenre);
                }
                db.SaveChanges();

                // build out sub genres
                foreach (string genre in sub_genres)
                {
                    var newGenre = new SubGenre()
                    {
                        Name = genre,
                    };
                    db.SubGenres.Add(newGenre);
                }
                db.SaveChanges();
                // start at 550 and walk up to 50.. see if it makes the right data
                for (int i = 550; i < 565; i++)
                {
                    HttpWebResponse movieResponse;
                    try
                    {
                        // Some movie id's throw out a 404, in that case just continue.
                        HttpWebRequest movie_request = (HttpWebRequest)WebRequest.Create($"https://api.themoviedb.org/3/movie/{i}?api_key=8c7809c57b6a451fe3227721fdae6efd");
                        movie_request.Method = "GET";
                        movieResponse = (HttpWebResponse)movie_request.GetResponse();
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    // Get the cast data for this movie 
                    HttpWebRequest cast_request = (HttpWebRequest)WebRequest.Create($"https://api.themoviedb.org/3/movie/{i}/credits?api_key=8c7809c57b6a451fe3227721fdae6efd");
                    cast_request.Method = "GET";
                    HttpWebResponse castResponse = (HttpWebResponse)cast_request.GetResponse();

                    // pull out the director from cast list
                    using (StreamReader sr = new StreamReader(castResponse.GetResponseStream()))
                    {
                        var castJSON = JObject.Parse(sr.ReadToEnd());
                        var name = (string)((JArray)castJSON["crew"]).First(crw => ((string)crw["job"]).Equals("Director", StringComparison.OrdinalIgnoreCase))["name"];
                        if (db.Directors.Any(dr => dr.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                            director = db.Directors.First(dr => dr.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                        else
                        {
                            director = new Director()
                            {
                                Name = name
                            };
                            director = db.Directors.Add(director);
                            db.SaveChanges();
                        }
                    }

                    using (StreamReader sr = new StreamReader(movieResponse.GetResponseStream()))
                    {
                        var movieJSON = JObject.Parse(sr.ReadToEnd());

                        var genre_name = (string)movieJSON["genres"][0]["name"];
                        if (db.Genres.Where(gn => gn.Name.Equals(genre_name, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                            main_genre = db.Genres.First(gn => gn.Name.Equals(genre_name, StringComparison.OrdinalIgnoreCase));
                        else
                        {
                            var newGenre = new Genre()
                            {
                                Name = genre_name
                            };
                            main_genre = db.Genres.Add(newGenre);
                        }

                        var mv = new Movie()
                        {
                            Title = (string)movieJSON["title"],
                            Director = director,
                            // Here im just using the first genre in the collection, they are all main genres coming from this endpoint
                            MainGenre = main_genre,
                            SubGenres = db.SubGenres.ToList(),
                            ReleaseDate = DateTime.Parse((string)movieJSON["release_date"]),
                            Length = int.Parse((string)movieJSON["runtime"]),
                            Description = (string)movieJSON["overview"]
                        };
                        db.Movies.Add(mv);
                    }

                    movieResponse.Close();
                    castResponse.Close();
                    Thread.Sleep(1500); // dont want to kill their api
                }
                db.SaveChanges();
            }
        }

        public HttpWebResponse SendRequest(string url, string method, string requestBody = null, string contentType = null)
        {
            // make a web request to get the base url 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;

            if (!string.IsNullOrEmpty(requestBody))
            {
                byte[] body = System.Text.Encoding.UTF8.GetBytes(requestBody);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(body, 0, requestBody.Length);
                dataStream.Close();
            }
            return (HttpWebResponse)request.GetResponse();
        }
    }
}
