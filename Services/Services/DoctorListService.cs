using Repositories.UnitOfWork;
using Services.Dto.response;
using Services.Interfaces;

public class DoctorListService : IDoctorListService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorListService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IList<GetDoctorRes>> GetAllDoctors()
    {
        var doctors = await _unitOfWork.DoctorRepository.GetAllIncludeAsync(d => d.User);

        return doctors.Select(d => new GetDoctorRes
        {
            Id = d.Id,
            FullName = d.User.FullName ??"N/A", 
            Qualification = d.Qualification ?? "N/A"
        }).ToList();
    }
}
