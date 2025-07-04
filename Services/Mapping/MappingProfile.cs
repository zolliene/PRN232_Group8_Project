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
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Patient.Phone))
            ;
    }
}