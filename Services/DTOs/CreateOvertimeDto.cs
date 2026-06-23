using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
	public class CreateOvertimeDto
	{
		[Range(1, int.MaxValue)]
		public int UserID { get; set; }

		[Range(1, int.MaxValue)]
		public int AuthorisedByID { get; set; }

		public DateTime WeekStartDate { get; set; }

		[Range(typeof(decimal), "0.1", "79228162514264337593543950335")]
		public decimal PlannedOTHours { get; set; }

		[Range(typeof(decimal), "0.1", "79228162514264337593543950335")]
		public decimal ActualOTHours { get; set; }

		public OTType OTType { get; set; }
	}
}
