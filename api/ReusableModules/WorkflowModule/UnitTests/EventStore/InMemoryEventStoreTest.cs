using System;
using System.Threading.Tasks;
using WorkflowModule.EventStore;
using WorkflowModule.Models;
using Xunit;

namespace UnitTests.WorkflowModule.EventStore
{
    public class InMemoryEventStoreTest
    {
        [Fact]
        public async Task Should_ReturnEmpty_Enumerable_When_There_Is_No_Aggregate()
        {
            var eventStore = new InMemoryEventStore();
            var aggregateId = Guid.NewGuid();

            var result = await eventStore.ReadEventStream(aggregateId);

            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_Contain_Added_Items()
        {
            var eventStore = new InMemoryEventStore();
            var aggregateId = Guid.NewGuid();

            var firstPayload = new EventPayload()
            {
                AggregateId = aggregateId,
                Timestamp = DateTime.Now,
                EventName = "FirstEvent",
                OrderNumber = 0
            };

            await eventStore.WriteToEventStream(aggregateId, firstPayload);

            var storedItems = await eventStore.ReadEventStream(aggregateId);

            Assert.Single(storedItems);

            var secondPayload = new EventPayload()
            {
                AggregateId = aggregateId,
                Timestamp = DateTime.Now,
                EventName = "SecondEvent",
                OrderNumber = 1
            };

            await eventStore.WriteToEventStream(aggregateId, secondPayload);

            var storedItemsAfterSecond = await eventStore.ReadEventStream(aggregateId);

            Assert.Collection(storedItemsAfterSecond, item =>
            {
                Assert.Equal("FirstEvent", item.EventName);
                Assert.Equal(0, item.OrderNumber);
            }, item =>
            {
                Assert.Equal("SecondEvent", item.EventName);
                Assert.Equal(1, item.OrderNumber);
            });
        }
    }
}
