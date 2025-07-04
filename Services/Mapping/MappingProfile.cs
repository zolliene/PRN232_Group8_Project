using AutoMapper;
using Repositories.Models;
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
        
    }
}