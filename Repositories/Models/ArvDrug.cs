﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class ArvDrug
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? GroupId { get; set; }

    public string ActiveIngredient { get; set; }

    public string Description { get; set; }

    public virtual ArvDrugGroup Group { get; set; }

    public virtual ICollection<RegimenDrug> RegimenDrugs { get; set; } = new List<RegimenDrug>();
}