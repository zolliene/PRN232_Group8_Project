using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.response
{
    public class  GetLabTestResqV1
    {
        public int Id { get; set; }
        // examination details
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
        // apointment details
        public int patientId { get; set; }
        public DateOnly? Date { get; set; }
        public string session { get; set; }
        // patient details
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        // test_type details

        public string NameOfTestType { get; set; }
        public string UnitOfTestType { get; set; }

        public string ReferenceRangeOfTestType { get; set; }


    }
}
