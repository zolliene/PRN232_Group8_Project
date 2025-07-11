namespace Services.Dto.request;

public class CreatePatientRegimenReq
{
    public int AppointmentId { get; set; }
    public int RegimenMasterId { get; set; }
    public string? CustomNotes { get; set; }
}