using BackEnd.SearchService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.UnitTest
{
    [TestClass]
    public class GoogleSearchTest
    {
        [TestMethod]
        public void TestGetRawResult_Keywords_NullOrEmpty_Throw_Exception()
        {
            HttpClient httpClient = new HttpClient();
            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await googleSearch.GetRawResult(""));
        }

        [TestMethod]
        public async Task TestGetRawResult_Keywords_SuccessfullyAsync()
        {
            string html = "<html />";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(html),
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);

            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            var result = await googleSearch.GetRawResult("keywords");
            Assert.AreEqual(html, result);
        }

        [TestMethod]
        public void TestParse_Url__NullOrEmpty_Throw_Exception()
        {
            HttpClient httpClient = new HttpClient();
            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            Assert.ThrowsException<ArgumentNullException>(() => googleSearch.Parse("", ""));
        }

        [TestMethod]
        public void TestParse_Url__None_Match_Record()
        {
            HttpClient httpClient = new HttpClient();
            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            string result = googleSearch.Parse("<html />", "hello");
            Assert.AreEqual("0", result);
        }

        [TestMethod]
        public void TestParse_Url__Match_Record_But_Zero_Url()
        {
            string html = "<div class=\"BNeawe UPmit AP7Wnd\">test</div>";
            HttpClient httpClient = new HttpClient();
            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            string result = googleSearch.Parse(html, "hello");
            Assert.AreEqual("0", result);
        }

        [TestMethod]
        public void TestParse_Url__Match_Record_With_One_Url()
        {
            string html = "<div class=\"BNeawe UPmit AP7Wnd\">test</div>";
            HttpClient httpClient = new HttpClient();
            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            string result = googleSearch.Parse(html, "test");
            Assert.AreEqual("1,", result);
        }

        [TestMethod]
        public void TestParse_Url__Match_Record_With_Two_Consecutive_Url()
        {
            string html = "<div class=\"BNeawe UPmit AP7Wnd\">test</div><div class=\"BNeawe UPmit AP7Wnd\">test</div>";
            HttpClient httpClient = new HttpClient();
            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            string result = googleSearch.Parse(html, "test");
            Assert.AreEqual("1,2,", result);
        }

        [TestMethod]
        public void TestParse_Url__Match_Record_With_Two_Seperated_Url()
        {
            string html = "<div class=\"BNeawe UPmit AP7Wnd\">test</div><div class=\"BNeawe UPmit AP7Wnd\">no</div><div class=\"BNeawe UPmit AP7Wnd\">test</div>";
            HttpClient httpClient = new HttpClient();
            GoogleSearch googleSearch = new GoogleSearch(httpClient);
            string result = googleSearch.Parse(html, "test");
            Assert.AreEqual("1,3,", result);
        }
    }
}