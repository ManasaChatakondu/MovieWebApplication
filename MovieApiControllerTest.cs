using System;
using System.Net.Http;
using System.Web;
using Moq;
using MovieWebApplication.Controllers;
using NUnit.Framework;

namespace MovieWebApplication.Tests
{
    [TestFixture]
    public class MovieApiControllerTest
    {
        [Test]
        public void GetMoviesTest()
        {
            // Assign:
            var stubHttpClient = new Mock<HttpClient>();
            var factory = new Mock<IHttpClientFactory>();
            factory.Setup(x => x.GetInstance()).Returns(stubHttpClient.Object);
            var movieController = new MovieApiController(factory.Object);

            // Act:
            var result = movieController.GetMovies(string.Empty);

            // Assert:
            Assert.IsNotNull(result);
        }
    }
}
