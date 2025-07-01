using PlanyApp.Repository.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly PRN232_Group8Context _context;

        private GenericRepository<Appointment> _appointmentRepository = null!;
        private GenericRepository<ArvDrug> _arvDrugRepository = null!;
        private GenericRepository<ArvDrugGroup> _arvDrugGroupRepository = null!;
        private GenericRepository<Doctor> _doctorRepository = null!;
        private GenericRepository<Examination> _examinationRepository = null!;
        private GenericRepository<LabTest> _labTestRepository = null!;
        private GenericRepository<Patient> _patientRepository = null!;
        private GenericRepository<PatientRegimen> _patientRegimenRepository = null!;
        private GenericRepository<RegimenDrug> _regimenDrugRepository = null!;
        private GenericRepository<TestType> _testTypeRepository = null!;
        private GenericRepository<TreatmentRegimensMaster> _treatmentRegimensMasterRepository = null!;
        private GenericRepository<User> _userRepository = null!;

        public UnitOfWork(PRN232_Group8Context context)
        {
            _context = context;
        }

        public GenericRepository<Appointment> AppointmentRepository => _appointmentRepository ??= new GenericRepository<Appointment>(_context);
        public GenericRepository<ArvDrug> ArvDrugRepository => _arvDrugRepository ??= new GenericRepository<ArvDrug>(_context);
        public GenericRepository<ArvDrugGroup> ArvDrugGroupRepository => _arvDrugGroupRepository ??= new GenericRepository<ArvDrugGroup>(_context);
        public GenericRepository<Doctor> DoctorRepository => _doctorRepository ??= new GenericRepository<Doctor>(_context);
        public GenericRepository<Examination> ExaminationRepository => _examinationRepository ??= new GenericRepository<Examination>(_context);
        public GenericRepository<LabTest> LabTestRepository => _labTestRepository ??= new GenericRepository<LabTest>(_context);
        public GenericRepository<Patient> PatientRepository => _patientRepository ??= new GenericRepository<Patient>(_context);
        public GenericRepository<PatientRegimen> PatientRegimenRepository => _patientRegimenRepository ??= new GenericRepository<PatientRegimen>(_context);
        public GenericRepository<RegimenDrug> RegimenDrugRepository => _regimenDrugRepository ??= new GenericRepository<RegimenDrug>(_context);
        public GenericRepository<TestType> TestTypeRepository => _testTypeRepository ??= new GenericRepository<TestType>(_context);
        public GenericRepository<TreatmentRegimensMaster> TreatmentRegimensMasterRepository => _treatmentRegimensMasterRepository ??= new GenericRepository<TreatmentRegimensMaster>(_context);
        public GenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(_context);

        public int Save() => _context.SaveChanges();

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
           await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        // IDisposable
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
