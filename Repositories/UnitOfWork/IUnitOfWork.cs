using PlanyApp.Repository.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        GenericRepository<Appointment> AppointmentRepository { get; }
        GenericRepository<ArvDrug> ArvDrugRepository { get; }
        GenericRepository<ArvDrugGroup> ArvDrugGroupRepository { get; }
        GenericRepository<Doctor> DoctorRepository { get; }
        GenericRepository<Examination> ExaminationRepository { get; }
        GenericRepository<LabTest> LabTestRepository { get; }
        GenericRepository<Patient> PatientRepository { get; }
        GenericRepository<PatientRegimen> PatientRegimenRepository { get; }
        GenericRepository<RegimenDrug> RegimenDrugRepository { get; }
        GenericRepository<TestType> TestTypeRepository { get; }
        GenericRepository<TreatmentRegimensMaster> TreatmentRegimensMasterRepository { get; }
        GenericRepository<User> UserRepository { get; }

        int Save();
        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}
