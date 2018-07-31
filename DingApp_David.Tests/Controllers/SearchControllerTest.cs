using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using DingApp_David;
using DingApp_David.Controllers;
using DingApp_David.Models;
using DingApp_David.Services;
using DingApp_David.Tests.TestModels;
using DingApp_David.Areas.HelpPage.Controllers;

namespace DingApp_David.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTest
    {
        [TestMethod]
        public void Search_Local_Lookup_Test()
        {
            // Arrange
            IDingDb db = new DingDbTest();
            ILookupService lookupService = new LookupService(db);
            SearchController controller = new SearchController(lookupService);
            WordModel testWord = new WordModel { word = "test_Word_2", definitions = "test Definition 2" };

            // Act
            ViewResult result = controller.Search("test_Word_2") as ViewResult;
            WordModel resultWord = result.Model as WordModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testWord.word, resultWord.word);
            Assert.AreEqual(testWord.definitions, resultWord.definitions);
        }

        [TestMethod]
        public void Search_API_Lookup_Test()
        {
            // Arrange
            IDingDb db = new DingDbTest();
            var mockService = new Mock<ILookupService>();
            mockService.Setup(x => x.DbLookup(It.IsAny<string>())).Returns((string x) => { return null; });
            mockService.Setup(x => x.APILookup(It.IsAny<string>())).Returns((string x) => { return new WordModel() { word = "api-test", definitions = "api test definition" }; });
            SearchController controller = new SearchController(mockService.Object);
            WordModel testWord = new WordModel { word = "api-test", definitions = "api test definition" }; //expected result from mocked Dictionary API call 
            //int expectedCount = 6; //5 initial entries + intel = 6 entries


            // Act
            ViewResult result = controller.Search("api-test") as ViewResult;
            WordModel resultWord = result.Model as WordModel;
            //int testCount = db.Query<WordModel>().Count();


            // Assert
            Assert.IsNotNull(resultWord);
            Assert.AreEqual(testWord.word, resultWord.word);
            Assert.AreEqual(testWord.definitions, resultWord.definitions);
            //Assert.AreEqual(expectedCount, testCount);
        }


        [TestMethod]
        public void Search_Word_Not_Found()
        {

            // Arrange
            IDingDb db = new DingDbTest();
            ILookupService lookupService = new LookupService(db);
            SearchController controller = new SearchController(lookupService);
            string testWord = "hahahathisjibberishwillreturnnothing";


            // Act
            ViewResult result = controller.Search(testWord) as ViewResult;


            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Word Not Found", result.ViewBag.Title);
            Assert.IsTrue(result.ViewBag.Message.Contains(testWord));
        }
    }
}