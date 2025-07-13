using Services.Dto.request;

namespace Services.Interfaces;

public interface IExaminationService
{
    Task AddExamination(CreateExaminationReq request);
}