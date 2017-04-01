using FakeServer.Controllers;
using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace FakeServer.Test
{
    public class DynamicControllerTests
    {
        [Fact]
        public void GetCollections()
        {
            var filePath = UTHelpers.Up();
            var ds = new DataStore(filePath);

            var controller = new DynamicController(ds);

            var collections = controller.GetCollections();
            Assert.Equal(3, collections.Count());

            UTHelpers.Down(filePath);
        }

        [Fact]
        public void GetItems_FavouriteMoveiWithQueryString()
        {
            var filePath = UTHelpers.Up();
            var ds = new DataStore(filePath);

            var controller = new DynamicController(ds);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.QueryString = new QueryString("?parents.favouriteMovie=Predator");

            // NOTE: Can't but skip and take to querystring with tests
            var result = controller.GetItems("family", 0, 100) as OkObjectResult;
            Assert.Equal(11, ((IEnumerable<dynamic>)result.Value).Count());

            UTHelpers.Down(filePath);
        }

        [Fact]
        public void GetItems_FriendsWithQueryString()
        {
            var filePath = UTHelpers.Up();
            var ds = new DataStore(filePath);

            var controller = new DynamicController(ds);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.QueryString = new QueryString("?children.friends.name=Castillo");

            var result = controller.GetItems("family") as OkObjectResult;
            Assert.Equal(2, ((IEnumerable<dynamic>)result.Value).Count());

            UTHelpers.Down(filePath);
        }
    }
}