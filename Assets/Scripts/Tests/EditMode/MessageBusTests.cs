using NUnit.Framework;
using BaseArchitecture.Core;
using System;
using UnityEngine;
using UnityEngine.TestTools;

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
        public void Subscribe_DuplicateHandler_LogsWarningAndInvokesOnce()
        {
            var count = 0;
            Action<TestMessage> handler = msg => count++;

            LogAssert.Expect(LogType.Warning, "[MessageBus] [Warning] Handler already subscribed for TestMessage. Ignoring duplicate.");
            _messageBus.Subscribe(handler);
            _messageBus.Subscribe(handler);

            _messageBus.Publish(new TestMessage { Value = 1 });

            Assert.AreEqual(1, count);
        }

        [Test]
        public void Publish_HandlerUnsubscribesDuringDispatch_DoesNotDoubleInvoke()
        {
            var count = 0;
            Action<TestMessage> handlerA = null;
            Action<TestMessage> handlerB = msg => count++;

            handlerA = msg =>
            {
                count++;
                _messageBus.Unsubscribe(handlerB);
            };

            _messageBus.Subscribe(handlerA);
            _messageBus.Subscribe(handlerB);

            _messageBus.Publish(new TestMessage { Value = 1 });

            // Both were in the snapshot at dispatch time, so both run once.
            Assert.AreEqual(2, count);

            // Second publish: handlerB was unsubscribed, only handlerA runs.
            _messageBus.Publish(new TestMessage { Value = 1 });
            Assert.AreEqual(3, count);
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
