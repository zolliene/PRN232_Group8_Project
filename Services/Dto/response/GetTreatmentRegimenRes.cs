namespace Services.Dto.response;

public class GetTreatmentRegimenRes
{
    public int Id { get; set; }
    public string RegimenName { get; set; }
    public string Drugs { get; set; }
    public string Description { get; set; }
    public List<GetRegimenDrugRes> RegimenDrugs { get; set; } = new List<GetRegimenDrugRes>();
}

public class GetRegimenDrugRes
{
    public int Id { get; set; }
    public string DrugName { get; set; }
    public string Dose { get; set; }
    public string Frequency { get; set; }
    public string ActiveIngredient { get; set; }
    public string DrugGroupName { get; set; }
}