namespace Services.DTOs
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // Expose linked IDs and names for clean API consumption
        public int LocationID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int RoleID { get; set; }
    }
}