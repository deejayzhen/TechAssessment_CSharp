using Xunit;
using TechAssessment_C_.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TechAssessment_C_.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TechAssessment_C_.Data;

namespace TechAssessment_XUnitTest
{
    public class ProductTest : IDisposable
    {
        private readonly ILogger<ProductController> _logger;
        private Products testcase;

        public ProductTest()
        {
            testcase = new Products();
        }

        public void Dispose()
        {

        }

        [Fact]
        public void Product_Name_Is_Required()
        {
            testcase.Name = null;
            testcase.Description = "This is a test description";
            testcase.Price = 10.0f;

            Debug.WriteLine("Product_Name_Is_Required - TestCase: " + testcase);

            var results = ValidateModel(testcase);

            var nameError = results.FirstOrDefault(
                v => v.MemberNames.Contains(nameof(testcase.Name)) &&
                v.ErrorMessage.Contains("Product Name is required"));
            Assert.NotNull(nameError);
        }

        [Fact]
        public void Product_Price_Must_Be_Greater_Than_Zero()
        {
            testcase.Name = "Valid Test Product Name";
            testcase.Description = "This is a test description";
            testcase.Price = -1.0f;

            Debug.WriteLine("Product_Price_Must_Be_Greater_Than_Zero - TestCase: " + testcase);

            var results = ValidateModel(testcase);

            var priceError = results.FirstOrDefault(
                v => v.MemberNames.Contains(nameof(testcase.Price)) &&
                v.ErrorMessage.Contains("Price must be greater than 0"));
            Assert.NotNull(priceError);
        }

        [Fact]
        public void Product_Is_Valid()
        {
            testcase.Name = "Valid Test Product Name";
            testcase.Description = "This is a test description";
            testcase.Price = 10.0f;

            Debug.WriteLine("Product_Is_Valid - TestCase: " + testcase);

            var results = ValidateModel(testcase);

            Assert.Empty(results);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            return validationResults;
        }
    }
}
