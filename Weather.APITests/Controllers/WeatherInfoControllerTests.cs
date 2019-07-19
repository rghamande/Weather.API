using NUnit.Framework;
using Weather.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using MvcContrib.TestHelper.Fakes;

namespace Weather.API.Controllers.Tests
{
    [TestFixture()]
    public class WeatherInfoControllerTests
    {
        [Test()]
        public async Task GetTest()
        {
            var WeatherInfoController = new WeatherInfoController();
            var info =await WeatherInfoController.Get("4229546");
            Assert.IsNotNull(info);
            Assert.AreEqual("Washington", info.Name);
        }
        [Test()]
        public async Task GetTest_NoRecord()
        {
            var WeatherInfoController = new WeatherInfoController();
            var info = await WeatherInfoController.Get("12343");
            Assert.IsNotNull(info);
            Assert.AreEqual("No record found for given City Id", info.Name);
        }

        [Test()]
        public async Task GetAllTest()
        {
                var WeatherInfoController = new WeatherInfoController();
                var resp = await WeatherInfoController.GetAll();
                var expectedResponse = new HttpResponseMessage();
                Assert.AreEqual(expectedResponse.IsSuccessStatusCode, resp.IsSuccessStatusCode);
            
        }
    }
}