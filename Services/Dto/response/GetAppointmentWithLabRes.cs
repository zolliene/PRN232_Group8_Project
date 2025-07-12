namespace Services.Dto.response;

public class GetAppointmentWithLabRes
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public string PatientPhone { get; set; }
    public DateOnly PatientDob { get; set; }
    public string PatientGender { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public string Session { get; set; }
    public string Status { get; set; }
    public int LabResultCount { get; set; }
}