using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{

    public enum EventType { Reboot, Redeploy, Freeze, Preempt, Terminate }

    public enum EventStatus { Scheduled, Started }

    public enum EventSource { Platform, User }

    public record ScheduledEvents(int DocumentIncarnation, IList<Event> Events);
    public record Event(string EventId, EventType EventType, string ResourceType, IList<string> Resources, EventStatus EventStatus, DateTime? NotBefore, string Description, EventSource EventSource);

    public record StartRequest(IList<EventStartRequest> StartRequests);

    public record EventStartRequest(string EventId);
}
