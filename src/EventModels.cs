using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{
    /// <summary>
    /// Scheduled EventType
    /// </summary>
    public enum EventType 
    {
        /// <summary>
        /// The Virtual Machine is scheduled for reboot (non-persistent memory is lost).
        /// </summary>
        Reboot,

        /// <summary>
        ///  The Virtual Machine is scheduled to move to another node (ephemeral disks are lost).
        /// </summary>
        Redeploy,

        /// <summary>
        /// The Virtual Machine is scheduled to pause for a few seconds. CPU and network connectivity may be suspended, but there is no impact on memory or open files.
        /// </summary>
        Freeze,

        /// <summary>
        /// The Spot Virtual Machine is being deleted (ephemeral disks are lost).
        /// </summary>
        Preempt,

        /// <summary>
        /// The virtual machine is scheduled to be deleted.
        /// </summary>
        Terminate
    }

    /// <summary>
    /// Scheduled event status
    /// </summary>
    public enum EventStatus 
    { 
        /// <summary>
        /// The event is scheduled
        /// </summary>
        Scheduled, 
        
        /// <summary>
        /// The event has started
        /// </summary>
        Started 
    }

    /// <summary>
    /// The source or the scheduled event
    /// </summary>
    public enum EventSource 
    { 
        /// <summary>
        /// Azure platform initiated
        /// </summary>
        Platform,
        
        /// <summary>
        /// User initiated
        /// </summary>
        User 
    }

    /// <summary>
    /// The Scheduled events payload
    /// </summary>
    public record ScheduledEvents(int DocumentIncarnation, IList<Event> Events) : RecordBase;
    
    /// <summary>
    /// A scheduled event
    /// </summary>
    public record Event(string EventId, EventType EventType, string ResourceType, IList<string> Resources, EventStatus EventStatus, DateTime? NotBefore, string Description, EventSource EventSource) : RecordBase;

    /// <summary>
    /// Request payload for starting events
    /// </summary>
    public record StartRequest(IList<EventStartRequest> StartRequests) : RecordBase;

    /// <summary>
    /// A single start request
    /// </summary>
    public record EventStartRequest(string EventId) : RecordBase;
}
