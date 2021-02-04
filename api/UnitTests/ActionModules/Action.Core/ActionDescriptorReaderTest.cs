using System;
using System.Linq;
using Action.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ActionModules.Action.Core
{
    [TestClass]
    public class ActionDescriptorReaderTest
    {
        [TestMethod]
        public void CreateCorrectActionDescriptionFromYaml()
        {
            var actionDescriptorReader = new YamlActionDescriptorReader();
            var contetnt = @"
name: CreateAuction
description: Creates auction and stuff...
formDisplayName: Create Auction
type: command
parameters:
  - parameterName: Name
    type: string
    validators:
      - type: minLength
        data: 3
    componentName: TextField
";
            var actionInfo = actionDescriptorReader.CreateActionInfo(contetnt);

            Assert.AreEqual(actionInfo.Name, "CreateAuction");
            Assert.IsTrue(actionInfo.Description.Contains("Creates auction"));
            Assert.IsTrue(actionInfo.FormDisplayName.Contains("Create Auction"));
            Assert.AreEqual(actionInfo.Type, "command");
            Assert.AreEqual(actionInfo.Parameters.Count(), 1);

            var parameter = actionInfo.Parameters.First();
            Assert.AreEqual(parameter.ParameterName, "Name");
            Assert.AreEqual(parameter.DisplayName, "Name");
            Assert.AreEqual(parameter.ComponentName, "TextField");

            var validator = parameter.Validators.First();
            Assert.AreEqual(validator.Type, "minLength");
            Assert.AreEqual(Int32.Parse(validator.Data as string), 3);
        }
    }
}
