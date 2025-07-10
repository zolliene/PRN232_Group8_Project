using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.response
{
    public class GetDoctorRes
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Qualification { get; set; } = null!;
    }

}
