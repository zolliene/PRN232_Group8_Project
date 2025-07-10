using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto
{

    public class TreatmentRegimenDTO
    {
        public int Id { get; set; }
        public string RegimenName { get; set; } = null!;
        public string Drugs { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class CreateTreatmentRegimenDTO
    {
        public string RegimenName { get; set; } = null!;
        public string Drugs { get; set; } = null!; // Mô tả tổng quát các thuốc
        public string Description { get; set; } = null!;
    }

    public class UpdateTreatmentRegimenDTO
    {
        public int Id { get; set; }
        public string RegimenName { get; set; } = null!;
        public string Drugs { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class RegimenDrugDetailDTO
    {
        public int Id { get; set; }
        public int? DrugId { get; set; }
        public string DrugName { get; set; } = null!;
        public string Dose { get; set; } = null!;
        public string Frequency { get; set; } = null!;
    }

    public class AddRegimenDrugDTO
    {
        public int RegimenId { get; set; }
        public int DrugId { get; set; }
        public string Dose { get; set; } = null!;
        public string Frequency { get; set; } = null!;
    }

    public class UpdateRegimenDrugDTO
    {
        public int Id { get; set; }
        public string Dose { get; set; } = null!;
        public string Frequency { get; set; } = null!;
    }

}
