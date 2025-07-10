using AutoMapper;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services.Dto;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TreatmentRegimenService : ITreatmentRegimenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TreatmentRegimenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 🗂️ Lấy danh sách tất cả phác đồ
        public async Task<List<TreatmentRegimenDTO>> GetAllRegimensAsync()
        {
            var regimens = await _unitOfWork.TreatmentRegimensMasterRepository.GetAllAsync();
            return regimens.Select(r => new TreatmentRegimenDTO
            {
                Id = r.Id,
                RegimenName = r.RegimenName,
                Drugs = r.Drugs,
                Description = r.Description
            }).ToList();
        }

        // 📄 Lấy chi tiết 1 phác đồ
        public async Task<TreatmentRegimenDTO?> GetRegimenByIdAsync(int id)
        {
            var regimen = await _unitOfWork.TreatmentRegimensMasterRepository.GetByIdAsync(id);
            if (regimen == null) return null;

            return new TreatmentRegimenDTO
            {
                Id = regimen.Id,
                RegimenName = regimen.RegimenName,
                Drugs = regimen.Drugs,
                Description = regimen.Description
            };
        }

        // 🟢 Tạo mới phác đồ
        public async Task<bool> CreateRegimenAsync(CreateTreatmentRegimenDTO dto)
        {
            var entity = _mapper.Map<TreatmentRegimensMaster>(dto);
            await _unitOfWork.TreatmentRegimensMasterRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // ✏️ Cập nhật phác đồ
        public async Task<bool> UpdateRegimenAsync(UpdateTreatmentRegimenDTO dto)
        {
            var regimen = await _unitOfWork.TreatmentRegimensMasterRepository.GetByIdAsync(dto.Id);
            if (regimen == null) return false;

            regimen.RegimenName = dto.RegimenName;
            regimen.Drugs = dto.Drugs;
            regimen.Description = dto.Description;

            await _unitOfWork.SaveAsync();
            return true;
        }

        // ❌ Xóa phác đồ
        public async Task<bool> DeleteRegimenAsync(int id)
        {
            var regimen = await _unitOfWork.TreatmentRegimensMasterRepository.GetByIdAsync(id);
            if (regimen == null) return false;

            _unitOfWork.TreatmentRegimensMasterRepository.Remove(regimen);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // 📋 Lấy danh sách thuốc trong phác đồ
        public async Task<List<RegimenDrugDetailDTO>> GetDrugsByRegimenIdAsync(int regimenId)
        {
            var drugs = await _unitOfWork.RegimenDrugRepository.FindIncludeAsync(
                d => d.RegimenId == regimenId,
                d => d.Drug
            );

            return drugs.Select(d => new RegimenDrugDetailDTO
            {
                Id = d.Id,
                DrugId = d.DrugId,
                DrugName = d.Drug?.Name ?? "",
                Dose = d.Dose,
                Frequency = d.Frequency
            }).ToList();
        }

        // ➕ Thêm thuốc vào phác đồ
        public async Task<bool> AddDrugToRegimenAsync(AddRegimenDrugDTO dto)
        {
            var entity = new RegimenDrug
            {
                RegimenId = dto.RegimenId,
                DrugId = dto.DrugId,
                Dose = dto.Dose,
                Frequency = dto.Frequency
            };

            await _unitOfWork.RegimenDrugRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // ✏️ Cập nhật thuốc trong phác đồ
        public async Task<bool> UpdateRegimenDrugAsync(UpdateRegimenDrugDTO dto)
        {
            var drug = await _unitOfWork.RegimenDrugRepository.GetByIdAsync(dto.Id);
            if (drug == null) return false;

            drug.Dose = dto.Dose;
            drug.Frequency = dto.Frequency;

            await _unitOfWork.SaveAsync();
            return true;
        }

        // ❌ Xóa thuốc khỏi phác đồ
        public async Task<bool> DeleteRegimenDrugAsync(int regimenDrugId)
        {
            var drug = await _unitOfWork.RegimenDrugRepository.GetByIdAsync(regimenDrugId);
            if (drug == null) return false;

            _unitOfWork.RegimenDrugRepository.Remove(drug);
            await _unitOfWork.SaveAsync();
            return true;
        }



    }

}
