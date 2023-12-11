using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
namespace Lab5_Bondareva
{
    public class CalculatorPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private IWebElement valueAInput;
        private IWebElement valueBInput;
        private IWebElement operationDropdown;
        private IWebElement calculateButton;
        private IWebElement resultLabel;
        private IWebElement buttonAPlus;
        private IWebElement buttonAMinus;
        private IWebElement buttonBPlus;
        private IWebElement buttonBMinus;

        public CalculatorPage(IWebDriver driver)
        {
            this.driver = driver;
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            valueAInput = this.driver.FindElement(By.CssSelector("[ng-model='a']"));
            valueBInput = this.driver.FindElement(By.CssSelector("[ng-model='b']"));
            operationDropdown = this.driver.FindElement(By.CssSelector("[ng-model='operation']"));
            resultLabel = this.driver.FindElement(By.CssSelector(".result.ng-binding"));

            buttonAPlus = this.driver.FindElement(By.CssSelector("[ng-click='inca()']"));
            buttonAMinus = this.driver.FindElement(By.CssSelector("[ng-click='deca()']"));

            buttonBPlus = this.driver.FindElement(By.CssSelector("[ng-click='incb()']"));
            buttonBMinus = this.driver.FindElement(By.CssSelector("[ng-click='decb()']"));
        }

        public void ButtonAPlus()
        {
            buttonAPlus.Click();
        }
        public void ButtonAMinus()
        {
            buttonAMinus.Click();
        }

        public void ButtonBPlus()
        {
            buttonBPlus.Click();
        }
        public void ButtonBMinus()
        {
            buttonBMinus.Click();
        }

        public void EnterValueA(double value)
        {
            valueAInput.Clear();
            valueAInput.SendKeys(value.ToString());
        }

        public void EnterValueB(double value)
        {
            valueBInput.Clear();
            valueBInput.SendKeys(value.ToString());
        }

        public void SelectOperation(string operation)
        {
            operationDropdown.SendKeys(operation);
        }

        public string GetResult()
        {
            return resultLabel.Text;
        }
    }



    public class Tests
    {
        private IWebDriver driver;
        private CalculatorPage calculatorPage;

        [TearDown]
        public void Teardown()
        {
            // ���������� ������ WebDriver
            driver.Quit();
        }

        [SetUp]
        public void Setup()
        {
            // ������������� WebDriver � �������� �������� ������������
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.globalsqa.com/angularJs-protractor/SimpleCalculator/");
            calculatorPage = new CalculatorPage(driver);
        }

        [TestCase(1, 2, "1 + 2 = 3")]
        [TestCase(-20, 10, "-20 + 10 = -10")]
        [TestCase(-15, 5, "-15 + 5 = -10")]
        [TestCase(0, 1, "0 + 1 = 1")]
        [TestCase(-5, -5, "-5 + -5 = -10")]
        public void TestAddition(double a, double b, string expect) // �������� �� ��������
        {
            calculatorPage.EnterValueA(a);
            calculatorPage.EnterValueB(b);
            calculatorPage.SelectOperation("+");
            string result = calculatorPage.GetResult();
            Assert.That(expect, Is.EqualTo(result));
        }

        [TestCase(10, 5, "10 - 5 = 5")]
        [TestCase(0, -5, "0 - -5 = 5")]
        [TestCase(8, 4, "8 - 3 = 5")]
        [TestCase(-10, 10, "-10 - 10 = -20")]
        public void TestSubtraction(double a, double b, string expect) // �������� �� ���������
        {
            calculatorPage.EnterValueA(a);
            calculatorPage.EnterValueB(b);
            calculatorPage.SelectOperation("-");
            string result = calculatorPage.GetResult();
            Assert.That(expect, Is.EqualTo(result));
        }

        [TestCase(10, 5, "10 * 5 = 50")]
        [TestCase(.5, .9, "null * null = null")]
        [TestCase(.1, 10, "null * 10 = null")]
        [TestCase(5, .3, "5 * null = null")]
        public void TestMultiplication(double a, double b, string expect) // �������� �� ���������
        {
            calculatorPage.EnterValueA(a);
            calculatorPage.EnterValueB(b);
            calculatorPage.SelectOperation("*");
            string result = calculatorPage.GetResult();
            Assert.That(expect, Is.EqualTo(result));
        }

        [TestCase(10, 5, "10 / 5 = 2")]
        public void TestDivision(double a, double b, string expect) // �������� �� �������
        {
            calculatorPage.EnterValueA(a);
            calculatorPage.EnterValueB(b);
            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(expect, Is.EqualTo(result));
        }

        [Test]
        public void ButtonAminus() // �������� �� ��������� �������������� � �������������
        {
            calculatorPage.EnterValueA(-15);
            calculatorPage.EnterValueB(-5);
            calculatorPage.ButtonAMinus();
            calculatorPage.SelectOperation("+");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("-16 + -5 = -21"));
        }

        [Test]
        public void ButtonAminusMultiplication() // �������� �� ��������� �������������� � �������������
        {
            calculatorPage.EnterValueA(-10);
            calculatorPage.EnterValueB(-5);
            calculatorPage.ButtonAMinus();
            calculatorPage.SelectOperation("*");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("-11 * -5 = 55"));
        }

        [Test]
        public void ButtonAminusMultiplicationPlus() // �������� �� ��������� �������������� � �������������
        {
            calculatorPage.EnterValueA(10);
            calculatorPage.EnterValueB(5);
            calculatorPage.ButtonAMinus();
            calculatorPage.SelectOperation("*");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("9 * 5 = 45"));
        }

        [Test]
        public void ButtonAMinusButtonBPlus() // �������� �� ������� �������������� � �������������
        {
            calculatorPage.EnterValueA(-10);
            calculatorPage.EnterValueB(-5);
            calculatorPage.ButtonAMinus();
            calculatorPage.ButtonBPlus();
            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("-11 / -4 = 2.75"));
        }

        [Test]
        public void ButtonAMinusButtonBPlusDevision() // �������� �� ������� �������������� � �������������
        {
            calculatorPage.EnterValueA(10);
            calculatorPage.EnterValueB(-5);
            calculatorPage.ButtonAMinus();
            calculatorPage.ButtonBPlus();
            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("9 / -4 = -2.25"));
        }

        [Test]
        public void BigPlus() // �������� �� ������� ��������� ��������������  � �������������
        {
            calculatorPage.EnterValueA(1000000000);
            calculatorPage.EnterValueB(-5);
            calculatorPage.ButtonAMinus();
            calculatorPage.ButtonBPlus();
            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("999999999 / -4 = -249999999.75"));
        }

        [Test]
        public void NullResult() // �������� �� ��������� ����
        {
            calculatorPage.EnterValueA(5);
            calculatorPage.EnterValueB(5);
            calculatorPage.ButtonAMinus();
            calculatorPage.ButtonBMinus();
            calculatorPage.SelectOperation("-");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("4 - 4 = 0"));
        }

        [Test]
        public void NullResultDevision() // �������� �� ��������� ����
        {
            calculatorPage.EnterValueA(1);
            calculatorPage.EnterValueB(5);
            calculatorPage.ButtonAMinus();
            calculatorPage.ButtonBMinus();
            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("0 / 4 = 0"));
        }

        [Test]
        public void OneResult() // �������� �� ��������� 1
        {
            calculatorPage.EnterValueA(2);
            calculatorPage.EnterValueB(2);
            calculatorPage.ButtonAMinus();
            calculatorPage.ButtonBMinus();
            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("1 / 1 = 1"));
        }

        [Test]
        public void InfinitiDevision() // �������� �� ����������� �������
        {
            calculatorPage.EnterValueA(3);
            calculatorPage.EnterValueB(4);
            calculatorPage.ButtonAMinus();
            calculatorPage.ButtonBMinus();
            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("2 / 3 = 0.6666666666666666"));
        }

        [Test]
        public void NullResultPlusMinus() // �������� �� �������� �������������� � �������������� ��� ��������� 0
        {
            calculatorPage.EnterValueA(-3);
            calculatorPage.EnterValueB(4);
            calculatorPage.ButtonAMinus();

            calculatorPage.SelectOperation("+");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("-4 + 4 = 0"));
        }

        [Test]
        public void DevisionPlus() // �������� �� ������� �������������� � ��������������
        {
            calculatorPage.EnterValueA(9);
            calculatorPage.EnterValueB(4);
            calculatorPage.ButtonAMinus();

            calculatorPage.SelectOperation("/");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("8 / 4 = 2"));
        }

        [Test]
        public void ErrorData() // �������� �� ���� ��������� ������, � ����� ������ �� �� ����������
        {
            calculatorPage.EnterValueA(.6);
            calculatorPage.EnterValueB(-4);
            calculatorPage.EnterValueA(4);
            calculatorPage.SelectOperation("-");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("4 - -4 = 8"));
        }

        [Test]
        public void ErrorData2() // �������� �� ���� ��������� ������, � ����� ������ �� �� ����������
        {
            calculatorPage.EnterValueA(7);
            calculatorPage.EnterValueB(.3);
            calculatorPage.EnterValueB(4);
            calculatorPage.SelectOperation("*");
            string result = calculatorPage.GetResult();
            Assert.That(result, Is.EqualTo("7 * 4 = 28"));
        }
    }
}
