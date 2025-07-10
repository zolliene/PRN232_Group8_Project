using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.response
{
    public class CreateLabTestDto
    {

        public int? AppointmentId { get; set; }

        public int? TestTypeId { get; set; }

        public DateTime OrderTime { get; set; }

        public string OrderStatus { get; set; }

        public string ResultValue { get; set; }

        public string Unit { get; set; }

        public string ReferenceRange { get; set; }

        public string ResultStatus { get; set; }

        public string Comments { get; set; }

        public DateTime? ResultDate { get; set; }

        public int? LabStaffId { get; set; }

    }
}
