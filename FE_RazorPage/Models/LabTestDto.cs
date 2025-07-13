namespace FE_RazorPage.Models
{
    public class LabTestModel
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NameOfTestType { get; set; }
        public string ResultValue { get; set; }
        public string ResultStatus { get; set; }
    }
}
