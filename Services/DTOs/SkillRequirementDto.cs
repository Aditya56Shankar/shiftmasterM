namespace Services.DTOs
{
    public class SkillRequirementDto
    {
        public int SkillReqID { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public int MinCountPerShift { get; set; }
        public string Status { get; set; } = string.Empty;

        public int LocationID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}