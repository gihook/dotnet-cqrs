using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.EventStore
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Guid, IEnumerable<EventPayload>> _store;

        public InMemoryEventStore()
        {
            _store = new ConcurrentDictionary<Guid, IEnumerable<EventPayload>>();
        }

        public Task<IEnumerable<EventPayload>> ReadEventStream(Guid aggregateId)
        {
            if (!_store.ContainsKey(aggregateId))
                return Task.FromResult(Enumerable.Empty<EventPayload>());

            return Task.FromResult(_store[aggregateId]);
        }

        public async Task<EventPayload> WriteToEventStream(Guid aggregateId, EventPayload payload)
        {
            var storedItems = await ReadEventStream(aggregateId);
            var updatedStoredItems = storedItems.Append(payload);

            _store[aggregateId] = updatedStoredItems;

            return payload;
        }
    }
}
