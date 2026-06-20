using Domain.Enums;

namespace Services.DTOs
{
	public class SwapRequestResponseDto
	{
		public int SwapID { get; set; }
		public int RequesterUserID { get; set; }
		public int TargetUserID { get; set; }
		public int OriginalAssignmentID { get; set; }
		public int? ProposedAssignmentID { get; set; }
		public int? ApprovedByID { get; set; }
		public string Reason { get; set; }
		public ApprovalStatus Status { get; set; }
	}
}