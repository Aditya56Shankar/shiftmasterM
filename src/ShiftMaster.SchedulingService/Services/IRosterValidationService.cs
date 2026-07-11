using System;
using System.Threading.Tasks;

namespace ShiftMaster.SchedulingService.Services
{
    public interface IRosterValidationService
    {
        Task ValidateAssignmentConstraintsAsync(int assignmentId);
    }
}
