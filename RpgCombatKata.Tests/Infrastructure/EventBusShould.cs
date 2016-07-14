using System;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core;

namespace RpgCombatKata.Tests.Infrastructure
{
    [TestFixture]
    public class EventBusShould {
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
            using (eventBus.Subscriber<int>().Subscribe(x => value = x)) {
                eventBus.Publish(1);
                value.Should().Be(1);
            }
        }

        [Test]
        public void dispose_subscriber()
        {
            var value = 0;
            eventBus.Subscriber<int>().Subscribe(x => value = x).Dispose();
            eventBus.Publish(1);
            value.Should().Be(0);
        }
    }
}
