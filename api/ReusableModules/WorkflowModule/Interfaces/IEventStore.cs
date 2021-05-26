using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IEventStore
    {
        Task<IEnumerable<EventPayload>> ReadEventStream(Guid aggregateId);
        Task<EventPayload> WriteToEventStream(Guid aggregateId, EventPayload payload);
    }
}
