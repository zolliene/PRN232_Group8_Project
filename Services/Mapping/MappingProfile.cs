using AutoMapper;
using Repositories.Models;
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

        CreateMap<CreateLabTestReq, LabTest>();

        CreateMap<LabTest, GetLabResultRes>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => 
                src.Appointment.Patient.FirstName + " " + src.Appointment.Patient.LastName))
            .ForMember(dest => dest.PatientPhone, opt => opt.MapFrom(src => src.Appointment.Patient.Phone))
            .ForMember(dest => dest.PatientDob, opt => opt.MapFrom(src => src.Appointment.Patient.Dob))
            .ForMember(dest => dest.TestTypeName, opt => opt.MapFrom(src => src.TestType.Name))
            .ForMember(dest => dest.TestTypeCode, opt => opt.MapFrom(src => src.TestType.Code));
    }
}