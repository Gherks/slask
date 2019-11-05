using TechTalk.SpecFlow;
using Slask.Application;
using FluentAssertions;

namespace UnitTestProject1
{
    [Binding]
    public class CalculatorStepsSteps
    {
        private static Calculator calculator = new Calculator();
        private static int result;

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int number)
        {
            calculator.FirstNumber = number;
        }
        
        [Given(@"I have also entered (.*) into the calculator")]
        public void GivenIHaveAlsoEnteredIntoTheCalculator(int number)
        {
            calculator.SecondNumber = number;
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            result = calculator.Add();
        }

        [When(@"I press subract")]
        public void WhenIPressSubract()
        {
            result = calculator.Subtract();
        }


        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int expectedResult)
        {
            expectedResult.Should().Equals(expectedResult);
        }
    }
}
