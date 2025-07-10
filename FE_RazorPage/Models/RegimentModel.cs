namespace FE_RazorPage.Models
{
    public class GetTreatmentRegimenModel
    {
        public int Id { get; set; }
        public string RegimenName { get; set; } = null!;
        public string Drugs { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class CreateTreatmentRegimenModel
    {
        public string RegimenName { get; set; } = null!;
        public string Drugs { get; set; } = null!; // Mô tả tổng quát các thuốc
        public string Description { get; set; } = null!;
    }

    public class UpdateTreatmentRegimenModel
    {
        public int Id { get; set; }
        public string RegimenName { get; set; } = null!;
        public string Drugs { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class GetRegimenDrugModel
    {
        public int Id { get; set; }
        public int? DrugId { get; set; }
        public string DrugName { get; set; } = null!;
        public string Dose { get; set; } = null!;
        public string Frequency { get; set; } = null!;
    }

    public class CreateRegimenDrugModel
    {
        public int RegimenId { get; set; }
        public int DrugId { get; set; }
        public string Dose { get; set; } = null!;
        public string Frequency { get; set; } = null!;
    }

    public class UpdateRegimenDrugModel
    {
        public int Id { get; set; }
        public string Dose { get; set; } = null!;
        public string Frequency { get; set; } = null!;
    }
}
