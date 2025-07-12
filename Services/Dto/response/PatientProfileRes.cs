using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Services.Dto.response
    {
        public class PatientProfileRes
        {
            public int Id { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public DateOnly Dob { get; set; }
            public string Gender { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string Address { get; set; } = null!;
            public string InsuranceNumber { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Username { get; set; } = null!;
        }
    }


