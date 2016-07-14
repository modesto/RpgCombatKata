using NUnit.Framework;
using RpgCombatKata.Core;

namespace RpgCombatKata.Tests {
    [TestFixture]
    public abstract class TestBase {
        protected GivenFixtures Given;
        protected WhenFixtures When;

        [SetUp]
        public void SetUp()
        {
            EventBus eventBus = new EventBus();
            Given = new GivenFixtures(eventBus);
            When = new WhenFixtures(eventBus);
        }

        [TearDown]
        public void TearDown()
        {
            When = null;
            Given.Dispose();
        }
    }
}