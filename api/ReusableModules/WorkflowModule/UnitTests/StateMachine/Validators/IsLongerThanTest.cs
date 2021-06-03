using WorkflowModule.StateMachine.Validators;
using Xunit;

namespace UnitTests.WorkflowStorage.Validators
{
    public class IsLongerThanTest
    {
        [Fact]
        public void IsLongerThan_should_return_true_when_string_is_longer_than_4()
        {
            var validator = new IsLongerThan();

            var result = validator.IsTrue(new object[] { "example", 4 });

            Assert.True(result);
        }

        [Fact]
        public void IsLongerThan_should_return_true_when_array_is_longer_than_3()
        {
            var validator = new IsLongerThan();

            var array = new int[] { 1, 2, 3, 4, 5 };
            var result = validator.IsTrue(new object[] { array, 3 });

            Assert.True(result);
        }
    }
}
