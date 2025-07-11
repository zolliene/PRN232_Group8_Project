namespace Services.Dto.response;

public class GetAppointmentLabResultsRes
{
    public int AppointmentId { get; set; }
    public string PatientName { get; set; }
    public string PatientPhone { get; set; }
    public DateOnly PatientDob { get; set; }
    public string PatientGender { get; set; }
    public string PatientAddress { get; set; }
    public string PatientInsuranceNumber { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public string Session { get; set; }
    public List<LabResultDetailRes> LabResults { get; set; } = new List<LabResultDetailRes>();
}

public class LabResultDetailRes
{
    public int Id { get; set; }
    public string TestTypeName { get; set; }
    public string TestTypeCode { get; set; }
    public DateTime OrderTime { get; set; }
    public string? ResultValue { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? ResultStatus { get; set; }
    public string? Comments { get; set; }
    public DateTime? ResultDate { get; set; }
    public string? LabStaffName { get; set; }
}