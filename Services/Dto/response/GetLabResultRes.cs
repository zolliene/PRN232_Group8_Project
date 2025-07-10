namespace Services.Dto.response;

public class GetLabResultRes
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string PatientName { get; set; }
    public string PatientPhone { get; set; }
    public DateOnly PatientDob { get; set; }
    public string TestTypeName { get; set; }
    public string TestTypeCode { get; set; }
    public DateTime OrderTime { get; set; }
    public string? ResultValue { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? ResultStatus { get; set; }
    public string? Comments { get; set; }
    public DateTime? ResultDate { get; set; }
}