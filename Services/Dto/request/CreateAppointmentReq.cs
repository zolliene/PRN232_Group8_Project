using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.request
{
    public class CreateAppointmentReq
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }
        public string Session { get; set; } = null!;
        public string? Note { get; set; }
    }

}
