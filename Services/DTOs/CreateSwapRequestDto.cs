namespace Services.DTOs
{
	public class CreateSwapRequestDto
	{
		public int TargetUserID { get; set; }
		public int OriginalAssignmentID { get; set; }
		public int? ProposedAssignmentID { get; set; }
		public string Reason { get; set; }
	}
}
