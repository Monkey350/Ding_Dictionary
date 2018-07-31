using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DingApp_David;
using DingApp_David.Models;
using DingApp_David.Services;
using DingApp_David.Tests.TestModels;
using DingApp_David.Controllers;

namespace DingApp_David.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Test_Get()
        {
            // Arrange
            IDingDb db = new DingDbTest();
            ILookupService lookupService = new LookupService(db);
            ValuesController controller = new ValuesController(lookupService);
            string invalidWord = "hahahainvalidjibberishlol";

            // Act
            JObject localResult = controller.Get("test_Word_1");
            JObject apiResult = controller.Get("intel");
            JObject invalidResult = controller.Get(invalidWord);

            // Assert
            Assert.IsNotNull(localResult);
            Assert.IsNotNull(apiResult);
            Assert.IsNotNull(invalidResult);
            Assert.AreEqual(localResult["definitions"], "test Definition 1");
            Assert.AreEqual(apiResult["definitions"], "1) intelligence ");
            Assert.AreEqual(invalidResult["Error"], $"Merriam Webster API returned no results for \"{invalidWord}\". Is your input valid?");


        }
    }
}
