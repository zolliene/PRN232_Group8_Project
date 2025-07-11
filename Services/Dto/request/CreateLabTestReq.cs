namespace Services.Dto.request;

public class CreateLabTestReq
{
    public int AppointmentId { get; set; }
    public List<int> TestTypeId { get; set; }
    public DateTime OrderTime { get; set; }
}