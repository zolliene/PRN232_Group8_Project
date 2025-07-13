using AutoMapper;
using Repositories.Models;
using Services.Dto;
using Services.Dto.request;
using Services.Dto.response;

namespace Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Appointment, GetAppointmentRes>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
            .ForMember(dest => dest.PatientPhone, opt => opt.MapFrom(src => src.Patient.Phone))
            .ForMember(dest => dest.PatientDob, opt => opt.MapFrom(src => src.Patient.Dob))
            ;

        CreateMap<Patient, GetPatientDetail>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            ;

        CreateMap<CreateExaminationReq, Examination>();

        CreateMap<TestType, GetTestTypeRes>();
        
        CreateMap<CreateLabTestReq, LabTest>()
            .ForMember(dest => dest.TestTypeId, opt => opt.Ignore());

        CreateMap<LabTest, GetLabResultRes>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => 
                src.Appointment.Patient.FirstName + " " + src.Appointment.Patient.LastName))
            .ForMember(dest => dest.PatientPhone, opt => opt.MapFrom(src => src.Appointment.Patient.Phone))
            .ForMember(dest => dest.PatientDob, opt => opt.MapFrom(src => src.Appointment.Patient.Dob))
            .ForMember(dest => dest.PatientGender, opt => opt.MapFrom(src => src.Appointment.Patient.Gender))
            .ForMember(dest => dest.TestTypeName, opt => opt.MapFrom(src => src.TestType.Name))
            .ForMember(dest => dest.TestTypeCode, opt => opt.MapFrom(src => src.TestType.Code));

        CreateMap<TreatmentRegimensMaster, GetTreatmentRegimenRes>();

        CreateMap<RegimenDrug, GetRegimenDrugRes>()
            .ForMember(dest => dest.DrugName, opt => opt.MapFrom(src => src.Drug.Name))
            .ForMember(dest => dest.ActiveIngredient, opt => opt.MapFrom(src => src.Drug.ActiveIngredient))
            .ForMember(dest => dest.DrugGroupName, opt => opt.MapFrom(src => src.Drug.Group.Name));

        CreateMap<CreatePatientRegimenReq, PatientRegimen>();

        CreateMap<CreateArvDrugDTO, ArvDrug>();
        CreateMap<UpdateArvDrugDTO, ArvDrug>();
        CreateMap<ArvDrugGroup, ArvDrugGroupDTO>(); // group

        CreateMap<CreateTreatmentRegimenDTO, TreatmentRegimensMaster>();
        CreateMap<UpdateTreatmentRegimenDTO, TreatmentRegimensMaster>();
    }
}