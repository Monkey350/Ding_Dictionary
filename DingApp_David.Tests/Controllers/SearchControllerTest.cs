using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DingApp_David;
using DingApp_David.Controllers;
using DingApp_David.Models;
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
            SearchController controller = new SearchController(db);
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
            SearchController controller = new SearchController(db);
            WordModel testWord = new WordModel { word = "intel", definitions = "1) intelligence " }; //expected result from Dictionary API call for this word, constructed by SearchController.CallDictionary method
            int expectedCount = 6; //5 initial entries + intel = 6 entries


            // Act
            ViewResult result = controller.Search("intel") as ViewResult;
            WordModel resultWord = result.Model as WordModel;
            int testCount = db.Query<WordModel>().Count();


            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testWord.word, resultWord.word);
            Assert.AreEqual(testWord.definitions, resultWord.definitions);
            Assert.AreEqual(expectedCount, testCount);
        }


        [TestMethod]
        public void Search_Word_Not_Found()
        {

            // Arrange
            IDingDb db = new DingDbTest();
            SearchController controller = new SearchController(db);
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