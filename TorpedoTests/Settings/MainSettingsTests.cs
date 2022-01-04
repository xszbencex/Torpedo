using Microsoft.VisualStudio.TestTools.UnitTesting;
using Torpedo.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.Settings.Tests
{
    [TestClass()]
    public class MainSettingsTests
    {
        [TestMethod()]
        public void CoordinateValidationTest()
        {
            Assert.IsTrue(MainSettings.CoordinateValidation(new Model.Vector(2, 3)));
            Assert.IsFalse(MainSettings.CoordinateValidation(new Model.Vector(-1, 3)));
            Assert.IsFalse(MainSettings.CoordinateValidation(new Model.Vector(10, -1)));
            Assert.IsFalse(MainSettings.CoordinateValidation(new Model.Vector(MainSettings.GridWidth, 2)));
            Assert.IsFalse(MainSettings.CoordinateValidation(new Model.Vector(10, MainSettings.GridHeight)));
        }
    }
}