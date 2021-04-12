using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Engine.Actions;
using Engine.Factories;
using Engine.Models;

namespace TestEngine.Actions
{
    [TestClass]
    public class TestWithWeapon
    {
        [TestMethod]
        public void Test_Constructor_GoodParameters()
        {
            GameItem pointyStick = ItemFactory.CreateGameItem(1001);
            AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pointyStick, 1, 5);
        }
    }
}
