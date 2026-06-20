using Domain.Enums;

namespace Services.DTOs
{
	public class CoverAssignmentResponseDto
	{
		public int CoverID { get; set; }
		public int OriginalAssignmentID { get; set; }
		public int CoveringUserID { get; set; }
		public int AssignedByID { get; set; }
		public CoverType CoverType { get; set; }
		public bool OvertimeApplicable { get; set; }
		public CoverStatus Status { get; set; }
	}
}