using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DingApp_David;
using DingApp_David.Controllers;

namespace DingApp_David.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get(string word)
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            string result = controller.Get("");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("value1", result);
        }
    }
}
