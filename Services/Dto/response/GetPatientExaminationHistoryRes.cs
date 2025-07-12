using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.response
{
    public class GetPatientExaminationHistoryRes
    {
        public int AppointmentId { get; set; }
        public DateOnly? Date { get; set; }
        public string Session { get; set; }
        public string Status { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string? DoctorName { get; set; }
        public string? Note { get; set; }
        public string? ClinicalData { get; set; }
    }

}
