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
    public class ArvDrugService : IArvDrugService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArvDrugService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 📄 Lấy tất cả thuốc + nhóm
        public async Task<List<ArvDrugDTO>> GetAllDrugsAsync()
        {
            var drugs = await _unitOfWork.ArvDrugRepository.GetAllIncludeAsync(d => d.Group);
            return drugs.Select(d => new ArvDrugDTO
            {
                Id = d.Id,
                Name = d.Name,
                GroupId = d.GroupId,
                GroupName = d.Group?.Name ?? "",
                ActiveIngredient = d.ActiveIngredient,
                Description = d.Description
            }).ToList();
        }

        // 📄 Lọc theo group_id
        public async Task<List<ArvDrugDTO>> GetDrugsByGroupIdAsync(int groupId)
        {
            var drugs = await _unitOfWork.ArvDrugRepository.FindIncludeAsync(
                d => d.GroupId == groupId,
                d => d.Group);

            return drugs.Select(d => new ArvDrugDTO
            {
                Id = d.Id,
                Name = d.Name,
                GroupId = d.GroupId,
                GroupName = d.Group?.Name ?? "",
                ActiveIngredient = d.ActiveIngredient,
                Description = d.Description
            }).ToList();
        }

        // 📄 Get theo ID (chi tiết)
        public async Task<ArvDrugDTO?> GetDrugByIdAsync(int id)
        {
            var drug = await _unitOfWork.ArvDrugRepository
                .FindIncludeAsync(d => d.Id == id, d => d.Group);

            var entity = drug.FirstOrDefault();
            if (entity == null) return null;

            return new ArvDrugDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                GroupId = entity.GroupId,
                GroupName = entity.Group?.Name ?? "",
                ActiveIngredient = entity.ActiveIngredient,
                Description = entity.Description
            };
        }

        // 🟢 Tạo thuốc mới
        public async Task<bool> CreateDrugAsync(CreateArvDrugDTO dto)
        {
            var drug = _mapper.Map<ArvDrug>(dto);
            await _unitOfWork.ArvDrugRepository.AddAsync(drug);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // ✏️ Cập nhật thuốc
        public async Task<bool> UpdateDrugAsync(UpdateArvDrugDTO dto)
        {
            var drug = await _unitOfWork.ArvDrugRepository.GetByIdAsync(dto.Id);
            if (drug == null) return false;

            drug.Name = dto.Name;
            drug.GroupId = dto.GroupId;
            drug.ActiveIngredient = dto.ActiveIngredient;
            drug.Description = dto.Description;

            await _unitOfWork.SaveAsync();
            return true;
        }

        // ❌ Xóa thuốc
        public async Task<bool> DeleteDrugAsync(int id)
        {
            var drug = await _unitOfWork.ArvDrugRepository.GetByIdAsync(id);
            if (drug == null) return false;

            _unitOfWork.ArvDrugRepository.Remove(drug);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // 🗂️ Lấy tất cả nhóm thuốc
        public async Task<List<ArvDrugGroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _unitOfWork.ArvDrugGroupRepository.GetAllAsync();
            return _mapper.Map<List<ArvDrugGroupDTO>>(groups);
        }
    }

}
