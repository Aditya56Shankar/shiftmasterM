namespace Services.DTOs
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; } 
        public string Name { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; } 
        public string Status { get; set; } 

        // Expose linked IDs and names for clean API consumption
        public int LocationID { get; set; }
        public string LocationName { get; set; } 
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } 
        public int RoleID { get; set; }
    }
}