using Domain.Enums;

namespace Services.DTOs
{
	public class CreateCoverAssignmentDto
	{
		public int OriginalAssignmentID { get; set; }
		public int CoveringUserID { get; set; }
		public int AssignedByID { get; set; }
		public CoverType CoverType { get; set; }
		public bool OvertimeApplicable { get; set; }
	}
}
