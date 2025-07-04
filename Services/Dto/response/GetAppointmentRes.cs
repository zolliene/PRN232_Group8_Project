namespace Services.Dto.response;

public class GetAppointmentRes
{
    public int Id { get; set; }
    public string PatientName { get; set; }
    public string PatientPhone { get; set; }
    public DateOnly? PatientDob { get; set; }
    public string Session { get; set; }
    public string Status { get; set; }
    
}