namespace Services.DTOs
{
    public class SkillRequirementDto
    {
        public int SkillReqID { get; set; }
        public string SkillName { get; set; }
        public int MinCountPerShift { get; set; }
        public string Status { get; set; }

        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}