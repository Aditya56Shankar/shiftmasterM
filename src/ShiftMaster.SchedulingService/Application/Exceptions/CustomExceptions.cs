using System;

namespace ShiftMaster.SchedulingService.Application.Exceptions
{
    public class InvalidWorkflowStateException : Exception
    {
        public InvalidWorkflowStateException(string message) : base(message)
        {
        }
    }

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
