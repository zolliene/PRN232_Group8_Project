using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Services.Dto.request;
using Services.Dto.response;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LabTestServiceV1 : ILabTestServiceV1
    {
        private readonly PRN232_Group8Context _context;

        public LabTestServiceV1(PRN232_Group8Context context)
        {
            _context = context;
        }

        public  async Task<CreateLabTestDtoV1> CreateLabTestId(CreateLabTestDtoV1 input)
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
            var labTestResponse = new CreateLabTestDtoV1
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

        public async Task<List<GetLabTestResqV1>> GetAllLabTestByDate(DateTime inpuDate)
        {
            // Tạo khoảng ngày
            var startDate = inpuDate.Date;
            var endDate = startDate.AddDays(1);

            var labtests = await _context.LabTests
                .Where(x =>
                    (x.OrderStatus == "pending" || x.OrderStatus == " ") &&
                    x.OrderTime >= startDate && x.OrderTime < endDate
                )
                .Include(a => a.Appointment)
                    .ThenInclude(a => a.Examination)
                .Include(a => a.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(a => a.TestType)
                .ToListAsync();

            var labTestResponses = labtests.Select(labTest => new GetLabTestResqV1()
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
            });

            return labTestResponses.ToList();
        }
       
        public async Task<List<GetLabTestResqV1>> GetAllLabtest()
        {
            var labtests = await _context.LabTests
       .Include(a => a.Appointment)
           .ThenInclude(a => a.Examination)
       .Include(a => a.Appointment)
           .ThenInclude(a => a.Patient)
       .Include(a => a.TestType)
       .ToListAsync();
            var labTestResponses = labtests.Select(labTest => new GetLabTestResqV1(
                )
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


            }
            );
            return labTestResponses.ToList();
        }

        public async Task<GetLabTestResqV1> GetLabTestById(int id)
        {
            var labTest =  _context.LabTests.Include(a => a.Appointment)
             .ThenInclude(a => a.Examination)
         .Include(a => a.Appointment)
             .ThenInclude(a => a.Patient)
         .Include(a => a.TestType).FirstOrDefault(x => x.Id == id);
            var labresponse = new GetLabTestResqV1()
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

       

        public  async Task<List<GetLabTestResqV1>> GetAllLabTestByAppointmentId(int appointmentId)
        {
            var labtests = await _context.LabTests
    .Include(a => a.Appointment)
        .ThenInclude(a => a.Examination)
    .Include(a => a.Appointment)
        .ThenInclude(a => a.Patient)
    .Include(a => a.TestType).Where(x => x.AppointmentId == appointmentId)
    .ToListAsync();
            var labTestResponses = labtests.Select(labTest => new GetLabTestResqV1(
                )
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


            }
            );
            return labTestResponses.ToList();
        }

        public async Task<GetLabTestResqV1> UpdateLabTestId(int id, UpdateLabTestDto input)
        {
            var getLabtest = await _context.LabTests.Include(a => a.Appointment)
        .ThenInclude(a => a.Examination)
    .Include(a => a.Appointment)
        .ThenInclude(a => a.Patient)
    .Include(a => a.TestType).FirstOrDefaultAsync(x => x.Id == id);
            Console.WriteLine($"🟡 Update ID: {id}");
            Console.WriteLine($"🟡 Value: {input.ResultValue}, Status: {input.ResultStatus}, Comments: {input.Comments}");
            getLabtest.ResultValue = input.ResultValue;
            getLabtest.Comments = input.Comments;
            getLabtest.ResultStatus = input.ResultStatus;
            _context.Update(getLabtest);
            _context.SaveChanges();
            var labTestResponse = new GetLabTestResqV1
            {
                Id = getLabtest.Id,
                AppointmentId = getLabtest.AppointmentId,
                TestTypeId = getLabtest.TestTypeId,
                OrderTime = getLabtest.OrderTime,
                OrderStatus = getLabtest.OrderStatus,
                ResultValue = getLabtest.ResultValue,
                Unit = getLabtest.Unit,
                ReferenceRange = getLabtest.ReferenceRange,
                ResultStatus = getLabtest.ResultStatus,
                Comments = getLabtest.Comments,
                ResultDate = getLabtest.ResultDate,
                LabStaffId = getLabtest.LabStaffId
            };
            return labTestResponse;
        }
    }
}
