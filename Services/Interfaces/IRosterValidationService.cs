using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IRosterValidationService
    {
        Task ValidateAssignmentConstraintsAsync(int assignmentId);
    }
}
