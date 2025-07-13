namespace Services.Dto.response;

public class GetPatientDetail
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public DateOnly Dob { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
    public string InsuranceNumber { get; set; } 
}