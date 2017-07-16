using System;
using NUnit.Framework;
using RpgCombatKata.Core.Infrastructure;
using RpgCombatKata.Tests.Fixtures;

namespace RpgCombatKata.Tests {
    [TestFixture]
    public abstract class TestBase : IDisposable {
        protected GivenFixtures Given;
        protected WhenFixtures When;
        private EventBus eventBus;

        [SetUp]
        public void SetUp() {
            eventBus = new EventBus();
            Given = new GivenFixtures(eventBus);
            When = new WhenFixtures(eventBus);
        }

        [TearDown]
        public void TearDown() {
            eventBus.Dispose();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
             MessageId = "eventBus")]
        public void Dispose() {
            eventBus?.Dispose();
        }
    }
}