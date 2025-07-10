namespace FE_RazorPage.Models
{
    public class GetArvDrugModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string ActiveIngredient { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class CreateArvDrugModel
    {
        public string Name { get; set; } = null!;
        public int? GroupId { get; set; }
        public string ActiveIngredient { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UpdateArvDrugModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? GroupId { get; set; }
        public string ActiveIngredient { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class GetArvDrugGroupModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }


}
