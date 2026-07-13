using System;
using System.Threading.Tasks;

namespace ShiftMaster.SchedulingService.Application.Interfaces
{
    public interface IRosterValidationService
    {
        Task ValidateAssignmentConstraintsAsync(int assignmentId);
    }
}
