using System;
using System.Threading.Tasks;

namespace ShiftMaster.AttendanceService.Clients
{
    public class EmployeeShiftShortDto
    {
        public int AssignmentId { get; set; }
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = null!;
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public decimal DurationHours { get; set; }
    }

    public interface ISchedulingClient
    {
        Task<EmployeeShiftShortDto?> GetAssignmentAsync(int assignmentId);
    }
}
