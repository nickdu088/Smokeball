using BackEnd;
using BackEnd.SearchService;
using Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace BackEnd.UnitTest
{
    [TestClass]
    public class SearchControllerTest
    {

        [TestMethod]
        public void TestGetAsync_Keywords_NullOrEmpty_Throw_ExceptionAsync()
        {
            var mockSearchServiceObj = new Mock<ISearchService>();
            var mockLoggerObj = new Mock<ILogger<SearchController>>();

            SearchController searchController = new SearchController(mockLoggerObj.Object, mockSearchServiceObj.Object);
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()=> await searchController.GetAsync("", "url"));
        }

        [TestMethod]
        public void TestGetAsync_Url_NullOrEmpty_Throw_Exception()
        {
            var mockSearchServiceObj = new Mock<ISearchService>();
            var mockLoggerObj = new Mock<ILogger<SearchController>>();

            SearchController searchController = new SearchController(mockLoggerObj.Object, mockSearchServiceObj.Object);
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await searchController.GetAsync("keywords", ""));
        }


        [TestMethod]
        public async Task TestGetAsync_Fetch_ResultFromSearchServiceAsync()
        {
            var mockSearchServiceObj = new Mock<ISearchService>();
            var mockLoggerObj = new Mock<ILogger<SearchController>>();
            
            SearchController searchController = new SearchController(mockLoggerObj.Object, mockSearchServiceObj.Object);

            mockSearchServiceObj.Setup(x => x.GetRawResult(It.IsAny<string>())).Returns(Task.FromResult("html"));
            mockSearchServiceObj.Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns("result");
            
            var result = await searchController.GetAsync("keywords", "url");
            mockSearchServiceObj.Verify(x => x.GetRawResult(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(new SearchItem() { Keywords="keywords", Url="url", Result="result" }, result);
        }
    }
}
