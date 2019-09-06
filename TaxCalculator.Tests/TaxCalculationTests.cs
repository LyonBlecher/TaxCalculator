using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TaxCalculator.Core;
using TaxCalculator.Core.Calculators;
using TaxCalculator.Core.Datalayer;
using TaxCalculator.Core.Entities;
using TaxCalculator.Core.Exceptions;
using TaxCalculator.Core.Factory;
using TaxCalculator.Core.Repository;

namespace TaxCalculator.Tests
{
    [TestFixture]
    public class TaxCalculationTests
    {
        private ITaxCalculator _taxCalculator;
        private ServiceProvider _provider;
        private ITaxCalculatorRepository _taxCalculatorRepository;
        private ICalculatorFactory _calculatorFactory;
        private ITaxCalculatorDb _db;
        private Mock<ITaxCalculatorDb> _mockDb;

        [SetUp]
        public void Init()
        {
            _mockDb = new Mock<ITaxCalculatorDb>();

            var services = new ServiceCollection();
            services.AddTransient<ITaxCalculatorDb>(sp => _mockDb.Object);
            services.AddTransient<ICalculatorFactory, CalculatorFactory>();
            services.AddTransient<ITaxCalculatorRepository, TaxCalculatorRepository>();
            services.AddTransient<ITaxCalculator, Core.TaxCalculator>();

            _provider = services.BuildServiceProvider();

            _db = _provider.GetService<ITaxCalculatorDb>();
            _taxCalculator = _provider.GetService<ITaxCalculator>();
            _taxCalculatorRepository = _provider.GetService<ITaxCalculatorRepository>();
            _calculatorFactory = _provider.GetService<ICalculatorFactory>();

        }

       

        [Test] public void MaxProgressiveTableAmountMatchesConstant()
        {
            var last = _taxCalculatorRepository.GetProgressiveTaxTable().OrderByDescending(p => p.Rate).First();
            Assert.AreEqual(last.To, TaxCalculatorRepository.MaxProgressiveAmount);
        }

        [Test]
        public void ThrowNullArgumentExceptionIfNullRateInput()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ProgressiveCalculator(null));


            Assert.AreEqual("rateInput", ex.ParamName);
        }

        [Test]
        public void CanGetCalculators()
        {
            var calcFlatRate = _calculatorFactory.GetCalculator(TaxCalculationType.FlatRate);

            Assert.IsInstanceOf<FlatRateCalculator>(calcFlatRate);

            var calcProgressive = _calculatorFactory.GetCalculator(TaxCalculationType.Progressive);

            Assert.IsInstanceOf<ProgressiveCalculator>(calcProgressive);

            var calcFlatValue = _calculatorFactory.GetCalculator(TaxCalculationType.FlatValue);

            Assert.IsInstanceOf<FlatValueCalculator>(calcFlatValue);
        }

        [Test]
        public void CanGetFlatRate()
        {
            var val = _taxCalculatorRepository.GetFlatRate();

            Assert.AreEqual(0.175M, val);
        }

        [Test]
        public void CanGetTaxTable()
        {
            var table = _taxCalculatorRepository.GetProgressiveTaxTable();
            var expectedTable = new List<TaxTableItem>
            {
                new TaxTableItem {From = 0, To = 8350, Rate = 0.10M},
                new TaxTableItem {From = 8351, To = 33950, Rate = 0.15M},
                new TaxTableItem {From = 33951, To = 88250, Rate = 0.25M},
                new TaxTableItem {From = 88251, To = 171550, Rate = 0.28M},
                new TaxTableItem {From = 171551, To = 372950, Rate = 0.33M},
                new TaxTableItem {From = 372951, To = TaxCalculatorRepository.MaxProgressiveAmount, Rate = 0.35M},
            };

            for (int i = 0; i < expectedTable.Count; i++)
            {
                var val = table[i];
                var expected = expectedTable[i];

                Assert.AreEqual(expected.Rate, val.Rate);
                Assert.AreEqual(expected.From, val.From);
                Assert.AreEqual(expected.To, val.To);
            }
        }

        [Test]
        public void CanGetPostalTaxTypes()
        {
            var postalTaxTypes = _taxCalculatorRepository.GetTaxCalculationTypes();

            var progressiveAType = postalTaxTypes["7441"];

            Assert.AreEqual(TaxCalculationType.Progressive, progressiveAType);

            var flatValueType = postalTaxTypes["A100"];

            Assert.AreEqual(TaxCalculationType.FlatValue, flatValueType);

            var flatRateType = postalTaxTypes["7000"];

            Assert.AreEqual(TaxCalculationType.FlatRate, flatRateType);

            var progressiveBType = postalTaxTypes["1000"];

            Assert.AreEqual(TaxCalculationType.Progressive, progressiveBType);


        }

        [Test]
        public void CanGetPostalTaxTypesByPostalCode()
        {
            var progressiveAType = _taxCalculatorRepository.GetTaxCalculationTypeByPostalCode("7441");

            Assert.AreEqual(TaxCalculationType.Progressive, progressiveAType);

            var flatValueType = _taxCalculatorRepository.GetTaxCalculationTypeByPostalCode("A100");

            Assert.AreEqual(TaxCalculationType.FlatValue, flatValueType);

            var flatRateType = _taxCalculatorRepository.GetTaxCalculationTypeByPostalCode("7000");

            Assert.AreEqual(TaxCalculationType.FlatRate, flatRateType);

            var progressiveBType = _taxCalculatorRepository.GetTaxCalculationTypeByPostalCode("1000");

            Assert.AreEqual(TaxCalculationType.Progressive, progressiveBType);
        }

        [Test]
        public void CanThrowPostalCodeNotFound()
        {
            var ex = Assert.Throws<PostalCodeNotFoundException>(() => _taxCalculatorRepository.GetTaxCalculationTypeByPostalCode("1111"));

            Assert.AreEqual($"Postal Code 1111 not found", ex.Message);
        }

        [Test]
        public void CanCreateCalculatorNotFoundException()
        {
            var ex = new CalculatorNotFoundException(TaxCalculationType.Progressive);

            Assert.AreEqual($"Calculator not found for type {TaxCalculationType.Progressive.ToString()}", ex.Message);
        }


        [Test]
        public void CalculateBaseProgressiveMinimumSalary1000()
        {
            var calculator = new ProgressiveCalculator(_taxCalculatorRepository.GetProgressiveTaxTable());

            var tax = calculator.DoCalculation(1000);            

            Assert.AreEqual(100, tax);
        }

        [Test]
        public void CalculateBaseProgressiveSalary15000()
        {
            var calculator = new ProgressiveCalculator(_taxCalculatorRepository.GetProgressiveTaxTable());

            var tax = calculator.DoCalculation(15000);

            Assert.AreEqual(1832.35M, tax);
        }

        [Test]
        public void CalculateBaseThrowsExceptionLessThanEqual0()
        {
            var calculator = new ProgressiveCalculator(_taxCalculatorRepository.GetProgressiveTaxTable());

            Assert.Throws<ArgumentOutOfRangeException>(() => calculator.DoCalculation(0));

            Assert.Throws<ArgumentOutOfRangeException>(() => calculator.DoCalculation(-1));

        }

        [Test]
        public void CalculateBaseProgressiveSalary33950WithDecimal()
        {
            var calculator = new ProgressiveCalculator(_taxCalculatorRepository.GetProgressiveTaxTable());

            var tax = calculator.DoCalculation(33950.5M);

            Assert.AreEqual(4674.925M, tax);
        }

        [Test]
        public void CalculateBaseProgressiveSalaryLastBracket()
        {
            var calculator = new ProgressiveCalculator(_taxCalculatorRepository.GetProgressiveTaxTable());

            var tax = calculator.DoCalculation(5000000);

            Assert.AreEqual(1727502.14M, tax);
        }

        [Test]
        public void CalculateProgressiveFrom7441PostalCodeAsProgressiveSalary1000()
        {
            var taxAmount = _taxCalculator.CalculateTax(1000, "7441");

            _mockDb.Setup(db => db.AddCalculatedResult("7441", 1000, TaxCalculationType.Progressive, 100));

            _mockDb.Verify();

            Assert.AreEqual(100, taxAmount);
        }

        [Test]
        public void CalculateProgressiveFrom7441PostalCodeAsProgressiveSalary15000()
        {
            var taxAmount = _taxCalculator.CalculateTax(15000, "7441");

            _mockDb.Setup(db => db.AddCalculatedResult("7441", 15000, TaxCalculationType.Progressive, 1832.35M));

            _mockDb.Verify();

            Assert.AreEqual(1832.35M, taxAmount);
        }

        [Test]        
        public void CalculateProgressiveFrom7441PostalCodeAsProgressiveSalaryLastBracket()
        {
            var taxAmount = _taxCalculator.CalculateTax(5000000, "7441");

            _mockDb.Setup(db => db.AddCalculatedResult("7441", 5000000, TaxCalculationType.Progressive, 1727502.14M));

            _mockDb.Verify();

            Assert.AreEqual(1727502.14M, taxAmount);
        }

        [Test]
        public void CalculateProgressiveFrom1000PostalCodeAsProgressiveSalary1000()
        {
            var taxAmount = _taxCalculator.CalculateTax(1000, "1000");

            _mockDb.Setup(db => db.AddCalculatedResult("1000", 1000, TaxCalculationType.Progressive, 100));

            _mockDb.Verify();

            Assert.AreEqual(100, taxAmount);
        }

        [Test]
        public void CalculateProgressiveFrom1000PostalCodeAsProgressiveSalary15000()
        {
            var taxAmount = _taxCalculator.CalculateTax(15000, "1000");

            _mockDb.Setup(db => db.AddCalculatedResult("1000", 15000, TaxCalculationType.Progressive, 1832.35M));

            _mockDb.Verify();

            Assert.AreEqual(1832.35M, taxAmount);
        }

        [Test]
        public void CalculateProgressiveFrom1000PostalCodeAsProgressiveSalaryLastBracket()
        {
            var taxAmount = _taxCalculator.CalculateTax(5000000, "1000");

            _mockDb.Setup(db => db.AddCalculatedResult("1000", 5000000, TaxCalculationType.Progressive, 1727502.14M));

            _mockDb.Verify();

            Assert.AreEqual(1727502.14M, taxAmount);
        }

        [Test]
        public void CalculateFlatRateFromPostalCode7000AsFlatRateSalary1000()
        {
            var taxAmount = _taxCalculator.CalculateTax(1000, "7000");

            _mockDb.Setup(db => db.AddCalculatedResult("7000", 1000, TaxCalculationType.FlatRate, 175));

            _mockDb.Verify();

            Assert.AreEqual(175, taxAmount);
        }


        

        [Test]
        public void CalculateFlatValueLessThan200000FromPostalCodeA100()
        {
            var taxAmount = _taxCalculator.CalculateTax(100, "A100");

            _mockDb.Setup(db => db.AddCalculatedResult("7000", 100, TaxCalculationType.FlatValue, 5));

            _mockDb.Verify();

            Assert.AreEqual(5, taxAmount);
        }

        [Test]
        public void CalculateFlatValueGreaterThan200000FromPostalCodeA100()
        {
            var taxAmount = _taxCalculator.CalculateTax(1000000, "A100");

            _mockDb.Setup(db => db.AddCalculatedResult("A100", 1000000, TaxCalculationType.FlatValue, 10000));

            _mockDb.Verify();

            Assert.AreEqual(10000, taxAmount);
        }
    }
}
