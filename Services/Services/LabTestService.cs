using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Services.Dto.response;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly PRN232_Group8Context _context;

        public LabTestService(PRN232_Group8Context context)
        {
            _context = context;
        }

        public  async Task<CreateLabTestDto> CreateLabTestId(CreateLabTestDto input)
        {
          
            var dto = new LabTest
            {
                AppointmentId = input.AppointmentId,
                TestTypeId = input.TestTypeId,
                OrderTime = input.OrderTime,
                OrderStatus = input.OrderStatus,
                ResultValue = input.ResultValue,
                Unit = input.Unit,
                ReferenceRange = input.ReferenceRange,
                ResultStatus = input.ResultStatus,
                Comments = input.Comments,
                ResultDate = input.ResultDate,
                LabStaffId = input.LabStaffId
            };
            _context.LabTests.Add(dto);
            _context.SaveChanges();
            var allTests = await _context.LabTests
           .Where(x => x.AppointmentId == input.AppointmentId)
           .ToListAsync();
            foreach (var item in allTests)
            {
                if (item.OrderStatus == "done")
                {
                    var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == item.AppointmentId);
                    if (appointment != null)
                    {
                        appointment.Status = "success";
                        _context.Appointments.Update(appointment);
                        await _context.SaveChangesAsync();
                    }
                }
                if (item.OrderStatus == "pending" || item.OrderStatus == " ")
                {
                    var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == item.AppointmentId);
                    if (appointment != null)
                    {
                        appointment.Status = "checked_in";
                        _context.Appointments.Update(appointment);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            var labTestResponse = new CreateLabTestDto
            {
                AppointmentId = dto.AppointmentId,
                TestTypeId = dto.TestTypeId,
                OrderTime = dto.OrderTime,
                OrderStatus = dto.OrderStatus,
                ResultValue = dto.ResultValue,
                Unit = dto.Unit,
                ReferenceRange = dto.ReferenceRange,
                ResultStatus = dto.ResultStatus,
                Comments = dto.Comments,
                ResultDate = dto.ResultDate,
                LabStaffId = dto.LabStaffId
            };
            return labTestResponse;
        }

        public async Task DeleteLabTestId(int id)
        {
            var checkInput = _context.LabTests.FirstOrDefault(x => x.Id == id);
            if (checkInput == null)
            {
                throw new Exception("Lab test with this ID does not exist.");

            }
            _context.Remove(checkInput);
            await _context.SaveChangesAsync();
          

        }

        public async Task<List<GetLabTestResq>> GetAllLabTest(DateTime inpuDate)
        {
            var labtests = await _context.LabTests
         .Where(x =>
             (x.OrderStatus == "pending" || x.OrderStatus == " ")
             && x.OrderTime.Date == inpuDate.Date)
         .Include(a => a.Appointment)
             .ThenInclude(a => a.Examination)
         .Include(a => a.Appointment)
             .ThenInclude(a => a.Patient)
         .Include(a => a.TestType)
         .ToListAsync();
            var labTestResponses = labtests.Select(labTest => new GetLabTestResq(
                )
            { Id = labTest.Id,
                AppointmentId = labTest.AppointmentId,
                TestTypeId = labTest.TestTypeId,
                OrderTime = labTest.OrderTime,
                OrderStatus = labTest.OrderStatus,
                ResultValue = labTest.ResultValue,
                Unit = labTest.Unit,
                ReferenceRange = labTest.ReferenceRange,
                ResultStatus = labTest.ResultStatus,
                Comments = labTest.Comments,
                ResultDate = labTest.ResultDate,
                LabStaffId = labTest.LabStaffId,
                Date = labTest.Appointment.Date,
                session = labTest.Appointment.Session,
                patientId = labTest.Appointment.PatientId,
                FullName = labTest.Appointment.Patient.FirstName +" "+ labTest.Appointment.Patient.LastName,
                Phone = labTest.Appointment.Patient.Phone,
                DateOfBirth = labTest.Appointment.Patient.Dob,
                NameOfTestType = labTest.TestType.Name,
                UnitOfTestType = labTest.TestType.Unit,
                ReferenceRangeOfTestType = labTest.TestType.ReferenceRange


            }
            );
            return labTestResponses.ToList();

        }

      

        public async Task<GetLabTestResq> GetLabTestById(int id)
        {
            var labTest =  _context.LabTests.Include(a => a.Appointment)
             .ThenInclude(a => a.Examination)
         .Include(a => a.Appointment)
             .ThenInclude(a => a.Patient)
         .Include(a => a.TestType).FirstOrDefault(x => x.Id == id);
            var labresponse = new GetLabTestResq()
            {
                Id = labTest.Id,
                AppointmentId = labTest.AppointmentId,
                TestTypeId = labTest.TestTypeId,
                OrderTime = labTest.OrderTime,
                OrderStatus = labTest.OrderStatus,
                ResultValue = labTest.ResultValue,
                Unit = labTest.Unit,
                ReferenceRange = labTest.ReferenceRange,
                ResultStatus = labTest.ResultStatus,
                Comments = labTest.Comments,
                ResultDate = labTest.ResultDate,
                LabStaffId = labTest.LabStaffId,
                Date = labTest.Appointment.Date,
                session = labTest.Appointment.Session,
                patientId = labTest.Appointment.PatientId,
                FullName = labTest.Appointment.Patient.FirstName + " " + labTest.Appointment.Patient.LastName,
                Phone = labTest.Appointment.Patient.Phone,
                DateOfBirth = labTest.Appointment.Patient.Dob,
                NameOfTestType = labTest.TestType.Name,
                UnitOfTestType = labTest.TestType.Unit,
                ReferenceRangeOfTestType = labTest.TestType.ReferenceRange

            };
            return labresponse;


        }

        public async Task<GetLabTestResq> UpdateLabTestId(int id, CreateLabTestDto input)
        {
            var checkInput = await _context.LabTests.FirstOrDefaultAsync(x => x.Id == id);
            if (checkInput == null)
            {
                throw new Exception("Lab test with this ID does not exist.");
            }
            checkInput.AppointmentId = input.AppointmentId;
            checkInput.TestTypeId = input.TestTypeId;
            checkInput.OrderTime = input.OrderTime;
            checkInput.OrderStatus = input.OrderStatus;
            checkInput.ResultValue = input.ResultValue;
            checkInput.Unit = input.Unit;
            checkInput.ReferenceRange = input.ReferenceRange;
            checkInput.ResultStatus = input.ResultStatus;
            checkInput.Comments = input.Comments;
            checkInput.ResultDate = input.ResultDate;
            checkInput.LabStaffId = input.LabStaffId;
            _context.SaveChanges();
            //([status]='success' OR [status]='checked_in' OR [status]='booked') of Appointment table
            // labtest [orderStatus] ='pending' or 'done'
            var allTests = await _context.LabTests
            .Where(x => x.AppointmentId == input.AppointmentId)
            .ToListAsync();
            foreach (var item in  allTests)
            {
                if(item.OrderStatus == "done")
                {
                    var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == item.AppointmentId);
                    if (appointment != null)
                    {
                        appointment.Status = "success";
                        _context.Appointments.Update(appointment);
                        await _context.SaveChangesAsync();
                    }
                }
                if(item.OrderStatus == "pending" || item.OrderStatus == " ")
                {
                    var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == item.AppointmentId);
                    if (appointment != null)
                    {
                        appointment.Status = "checked_in";
                        _context.Appointments.Update(appointment);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var labTestResponse = new GetLabTestResq
            {
                Id = checkInput.Id,
                AppointmentId = checkInput.AppointmentId,
                TestTypeId = checkInput.TestTypeId,
                OrderTime = checkInput.OrderTime,
                OrderStatus = checkInput.OrderStatus,
                ResultValue = checkInput.ResultValue,
                Unit = checkInput.Unit,
                ReferenceRange = checkInput.ReferenceRange,
                ResultStatus = checkInput.ResultStatus,
                Comments = checkInput.Comments,
                ResultDate = checkInput.ResultDate,
                LabStaffId = checkInput.LabStaffId
            };
            return labTestResponse;
        }
    }
}
