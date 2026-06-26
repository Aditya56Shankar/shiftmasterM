using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
	public class CreateCoverAssignmentDto
	{
		[Range(1, int.MaxValue)]
		public int OriginalAssignmentID { get; set; }

		[Range(1, int.MaxValue)]
		public int CoveringUserID { get; set; }

		[Range(1, int.MaxValue)]
		public int AssignedByID { get; set; }

		public CoverType CoverType { get; set; }
		public bool OvertimeApplicable { get; set; }
	}
}
