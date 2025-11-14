using NUnit.Framework;
using BaseArchitecture.Core;
using System;

namespace BaseArchitecture.Tests
{
    public class TestMessage : IMessageObject
    {
        public int Value { get; set; }
    }

    [TestFixture]
    public class MessageBusTests
    {
        private MessageBus _messageBus;

        [SetUp]
        public void Setup()
        {
            _messageBus = new MessageBus();
            _messageBus.Initialize();
        }

        [TearDown]
        public void Teardown()
        {
            _messageBus.Dispose();
        }

        [Test]
        public void Publish_WithNoSubscribers_DoesNotThrow()
        {
            var message = new TestMessage { Value = 42 };

            Assert.DoesNotThrow(() => _messageBus.Publish(message));
        }

        [Test]
        public void Subscribe_WhenPublished_HandlerIsInvoked()
        {
            var receivedValue = 0;
            _messageBus.Subscribe<TestMessage>(msg => receivedValue = msg.Value);

            _messageBus.Publish(new TestMessage { Value = 42 });

            Assert.AreEqual(42, receivedValue);
        }

        [Test]
        public void Unsubscribe_AfterSubscribe_HandlerNotInvoked()
        {
            var receivedValue = 0;
            Action<TestMessage> handler = msg => receivedValue = msg.Value;
            _messageBus.Subscribe(handler);
            _messageBus.Unsubscribe(handler);

            _messageBus.Publish(new TestMessage { Value = 42 });

            Assert.AreEqual(0, receivedValue);
        }

        [Test]
        public void Publish_WithMultipleSubscribers_InvokesAll()
        {
            var count = 0;
            _messageBus.Subscribe<TestMessage>(msg => count++);
            _messageBus.Subscribe<TestMessage>(msg => count++);

            _messageBus.Publish(new TestMessage { Value = 42 });

            Assert.AreEqual(2, count);
        }

        [Test]
        public void Dispose_ClearsAllSubscribers()
        {
            var receivedValue = 0;
            _messageBus.Subscribe<TestMessage>(msg => receivedValue = msg.Value);

            _messageBus.Dispose();
            _messageBus.Initialize();

            _messageBus.Publish(new TestMessage { Value = 42 });

            Assert.AreEqual(0, receivedValue);
        }
    }
}
