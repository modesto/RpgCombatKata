using System;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Tests.Infrastructure
{
    [TestFixture]
    public sealed class EventBusShould : IDisposable {
        private EventBus eventBus;

        [SetUp]
        public void SetUp() {
            eventBus = new EventBus();
        }

        [TearDown]
        public void TearDown() {
            eventBus.Dispose();
        }

        [Test]
        public void allow_to_subscribe_by_type() {
            var value = 0;
            using (eventBus.Observable<int>().Subscribe(x => value = x)) {
                eventBus.Publish(1);
                value.Should().Be(1);
            }
        }

        [Test]
        public void dispose_subscriber()
        {
            var value = 0;
            eventBus.Observable<int>().Subscribe(x => value = x).Dispose();
            eventBus.Publish(1);
            value.Should().Be(0);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "eventBus")]
        public void Dispose() {
            eventBus?.Dispose();
        }
    }
}
