using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto
{
    public class ArvDrugDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? GroupId { get; set; }
        public string GroupName { get; set; } = null!; 
        public string ActiveIngredient { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class CreateArvDrugDTO
    {
        public string Name { get; set; } = null!;
        public int? GroupId { get; set; }
        public string ActiveIngredient { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UpdateArvDrugDTO
    {
        public int Id { get; set; }                    
        public string Name { get; set; } = null!;
        public int? GroupId { get; set; }
        public string ActiveIngredient { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class ArvDrugGroupDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }


}
