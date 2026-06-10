using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            return await _supplierRepository.GetAllAsync();
        }

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            return await _supplierRepository.GetByIdAsync(id);
        }

        public async Task<SupplierDto> CreateAsync(CreateSupplierDto createDto)
        {
            var supplier = new Supplier
            {
                Name = createDto.Name,
                ContactPerson = createDto.ContactPerson,
                Email = createDto.Email,
                Phone = createDto.Phone,
                Address = createDto.Address,
                IsActive = true
            };

            var createdSupplier = await _supplierRepository.CreateAsync(supplier);

            return new SupplierDto
            {
                Id = createdSupplier.Id,
                Name = createdSupplier.Name,
                ContactPerson = createdSupplier.ContactPerson,
                Email = createdSupplier.Email,
                Phone = createdSupplier.Phone,
                Address = createdSupplier.Address,
                IsActive = createdSupplier.IsActive
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateSupplierDto updateDto)
        {
            var existingSupplier = await _supplierRepository.GetByIdAsync(id);
            if (existingSupplier == null) return false;

            var supplier = new Supplier
            {
                Id = id,
                Name = updateDto.Name,
                ContactPerson = updateDto.ContactPerson,
                Email = updateDto.Email,
                Phone = updateDto.Phone,
                Address = updateDto.Address,
                IsActive = existingSupplier.IsActive
            };

            await _supplierRepository.UpdateAsync(supplier);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingSupplier = await _supplierRepository.GetByIdAsync(id);
            if (existingSupplier == null) return false;

            await _supplierRepository.DeleteAsync(id);
            return true;
        }
    }
}
