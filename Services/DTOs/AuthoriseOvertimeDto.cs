using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
	public class AuthoriseOvertimeDto
	{
		[Range(1, int.MaxValue)]
		public int AuthorisedByID { get; set; }
		public bool Approved { get; set; }
	}
}
