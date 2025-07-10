using AutoMapper;
using Repositories.Models;
using Services.Dto;
using Services.Dto.response;

namespace Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Appointment, GetAppointmentRes>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName));

        CreateMap<CreateArvDrugDTO, ArvDrug>();
        CreateMap<UpdateArvDrugDTO, ArvDrug>();
        CreateMap<ArvDrugGroup, ArvDrugGroupDTO>(); // group

        CreateMap<CreateTreatmentRegimenDTO, TreatmentRegimensMaster>();
        CreateMap<UpdateTreatmentRegimenDTO, TreatmentRegimensMaster>();
    }
}