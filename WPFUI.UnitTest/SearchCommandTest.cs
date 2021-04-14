using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WPFUI.Command;

namespace WPFUI.UnitTest
{
    [TestClass]
    public class SearchCommandTest
    {
        [TestMethod]
        public void Test_Search_CanExecute_Return_True()
        {
            var mockSearchCommand = new SearchCommand(null);

            Assert.AreEqual(true, mockSearchCommand.CanExecute(null));
        }

        [TestMethod]
        public void Test_Search_Execution_Throw_Null_Exception()
        {
            var mockSearchCommand = new SearchCommand(null);

            Assert.ThrowsException<ArgumentNullException>(()=>mockSearchCommand.Execute(null));
        }

        [TestMethod]
        public void Test_Search_Execution_Successful()
        {   
            var actual = string.Empty;
            var action = new Action(() => { actual = "A"; });
            var mockSearchCommand = new SearchCommand(action);
            mockSearchCommand.Execute(null);
            Assert.AreEqual("A", actual);

        }
    }
}
