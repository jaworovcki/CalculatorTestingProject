using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AnalaizerClassLibrary;
using ErrorLibrary;


namespace AnalizerClassLibraryTest
{
    [TestClass]
    public class FormatTests
    {
        // Mocked constants (update these if necessary)
        private const int MAX_LENGHT_EXPRESSION = 65536;
        

        [TestInitialize]
        public void Setup()
        {
            // Reset or initialize any static state if necessary
        }

        [TestMethod]
        public void ExpressionIsEmpty()
        {
            AnalaizerClass.expression = "";  // Set the static expression field
            string result = AnalaizerClass.Format();
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ExpressionExceedsMaxLength()
        {
            AnalaizerClass.expression = new string('1', MAX_LENGHT_EXPRESSION + 1);  // Too long
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_07, result);
        }

        [TestMethod]
        public void UnknownCharacterIsPresent()
        {
            AnalaizerClass.expression = "5 + a";  // Invalid character 'a'
            string result = AnalaizerClass.Format();
            Assert.IsTrue(result.StartsWith("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_02, 2)));
        }

        [TestMethod]
        public void InvalidStartCharacter()
        {
            AnalaizerClass.expression = "*5 + 3";  // Invalid start '*'
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_03, result);
        }

        [TestMethod]
        public void InvalidEndCharacter()
        {
            AnalaizerClass.expression = "5 + 3*";  // Invalid end '*'
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_05, result);
        }

        [TestMethod]
        public void TwoOperatorsAreConsecutive()
        {
            AnalaizerClass.expression = "5 ++ 3";  // Two consecutive operators '++'
            string result = AnalaizerClass.Format();
            Assert.IsTrue(result.StartsWith("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_04, 2)));
        }

        [TestMethod]
        public void DigitFollowedByOpenBracket()
        {
            AnalaizerClass.expression = "5(3 + 2)";  // Invalid syntax with digit followed by open bracket
            string result = AnalaizerClass.Format();
            Assert.IsTrue(result.StartsWith("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_01, 1)));
        }

        [TestMethod]
        public void ExpressionIsValid()
        {
            AnalaizerClass.expression = "5 + (3 * 2)";  // Valid expression
            string result = AnalaizerClass.Format();
            Assert.AreEqual("5+(3*2)", result);
        }

        // Add more tests for additional cases as needed
    }
}
