using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WPFUI.ViewModel;

namespace WPFUI.UnitTest
{
    [TestClass]
    public class SearchViewModelTest
    {
        [TestMethod]
        public void Test_Search_Keywords_NullEmpty_Returns_Result_In_View()
        {
            var mockViewModel = new SearchViewModel(null);

            mockViewModel.Keywords = string.Empty;
            mockViewModel.Url = string.Empty;
            mockViewModel.SearchCommand.Execute(null);

            Assert.AreEqual("keywords or url cannot be empty!", mockViewModel.Result);
        }

        [TestMethod]
        public void Test_Url_Keywords_NotNullEmpty_Returns_Result_In_View()
        {
            string html = "{ \"keywords\":\"conveyancing software\",\"url\":\"www.smokeball.com.au\",\"result\":\"3,\"}";
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

            var homeController = new SearchViewModel(httpClient);
            homeController.Keywords = "conveyancing software";
            homeController.Url = "www.smokeball.com.au";
            homeController.SearchCommand.Execute(null);

            Assert.AreEqual("3,", homeController.Result);
        }
    }
}
