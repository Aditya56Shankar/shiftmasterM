using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IRosterValidationService
    {
        /// <summary>
        /// Analyzes a shift assignment against labor rules and logs infractions automatically.
        /// </summary>
        /// <param name="assignmentId">The primary key ID of the shift assignment row to evaluate.</param>
        Task ValidateAssignmentConstraintsAsync(int assignmentId);
    }
}
