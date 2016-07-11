using NUnit.Framework;

namespace RpgCombatKata.Tests {
    [TestFixture]
    public abstract class TestBase {
        protected TestFixtures Given;
        protected TestFixtures When;

        [SetUp]
        public void SetUp()
        {
            Given = new TestFixtures();
            When = Given;
        }

        [TearDown]
        public void TearDown()
        {
            When = null;
            Given.Dispose();
        }
    }
}