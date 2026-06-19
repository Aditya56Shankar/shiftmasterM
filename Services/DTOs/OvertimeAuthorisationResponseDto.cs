using Domain.Enums;

namespace Services.DTOs
{
	public class OvertimeAuthorisationResponseDto
	{
		public int OTID { get; set; }
		public int UserID { get; set; }
		public int AuthorisedByID { get; set; }
		public DateTime WeekStartDate { get; set; }
		public decimal PlannedOTHours { get; set; }
		public decimal ActualOTHours { get; set; }
		public OTType OTType { get; set; }
		public ApprovalStatus Status { get; set; }
	}
}