using Xunit;
using WorkflowModule.StateMachine;
using WorkflowModule.StateMachine.Converters;
using Moq;
using WorkflowModule.Interfaces;
using WorkflowModule.Descriptors;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class ValidatorTranslatorTest
    {
        [Fact]
        public void CanParse_should_return_true_for_int()
        {
            var translator = new ValidatorTranslator();
            translator.RegisterConverter("int", new IntConverter());
            var result = translator.CanParse("int", "1");

            Assert.True(result);
        }

        [Fact]
        public void CanParse_should_return_false_for_non_ints()
        {
            var translator = new ValidatorTranslator();
            translator.RegisterConverter("int", new IntConverter());
            var result = translator.CanParse("int", "sample string");

            Assert.False(result);
        }

        [Fact]
        public void CanParse_should_return_true_for_string_inputs()
        {
            var translator = new ValidatorTranslator();
            translator.RegisterConverter("string", new StringConverter());
            var result = translator.CanParse("string", "sample string");

            Assert.True(result);
        }

        [Fact]
        public void CanParse_should_return_false_for_non_existing_type()
        {
            var translator = new ValidatorTranslator();
            var result = translator.CanParse("NonExistingType", "sample string");

            Assert.False(result);
        }

        [Fact]
        public void GetValidator_should_return_registered_validator_function()
        {
            var translator = new ValidatorTranslator();
            var validatorMock = new Mock<IInputValidator>();
            validatorMock.Setup(x => x.IsTrue(It.IsAny<object[]>())).Returns(true);
            translator.RegisterValidator("IsDefined", validatorMock.Object);

            var descriptor = new InputValidatorDescriptor()
            {
                Type = "IsDefined"
            };
            var validator = translator.GetValidator(descriptor);

            var validationResult = validator(new object[0]);

            Assert.True(validationResult);
        }

        [Fact]
        public void GetValidator_should_return_oposite_validator_when_prefixed()
        {
            var translator = new ValidatorTranslator();
            var validatorMock = new Mock<IInputValidator>();
            validatorMock.Setup(x => x.IsTrue(It.IsAny<object[]>())).Returns(true);
            translator.RegisterValidator("IsDefined", validatorMock.Object);

            var descriptor = new InputValidatorDescriptor()
            {
                Type = "NOT_IsDefined"
            };
            var validator = translator.GetValidator(descriptor);

            var validationResult = validator(new object[0]);

            Assert.False(validationResult);
        }

        [Fact]
        public void GetValidator_should_return_always_false_validator_when_function_is_not_defined()
        {
            var translator = new ValidatorTranslator();
            var descriptor = new InputValidatorDescriptor()
            {
                Type = "NonExistingValidator"
            };
            var validator = translator.GetValidator(descriptor);

            var validationResult = validator(new object[0]);

            Assert.False(validationResult);
        }
    }
}
