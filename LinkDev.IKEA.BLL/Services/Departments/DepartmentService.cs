﻿using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.DAL.Entities.Department;
using LinkDev.IKEA.DAL.Persistence.Repositories.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentsRepository;

        public DepartmentService(IDepartmentRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;
        }

        public IEnumerable<DepartmentToReturnDto> GetAllDepartment()
        {
            var departments = _departmentsRepository.GetAllAsIQueryable().Select(department => new DepartmentToReturnDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreationDate = department.CreationDate,
            }).AsNoTracking().ToList();
            return departments;

        }

        public DepartmentDetailsToReturnDto? GetDepartment(int id)
        {
            var department = _departmentsRepository.Get(id);
            if (department is { })
                return new DepartmentDetailsToReturnDto()
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    Description = department.Description,
                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModifiedBy = department.LastModifiedBy,
                };
            return null;
        }
        public int CreateDepartment(CreatedDepartmentDto departmentDto)
        {
            var Department = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                //CreatedOn = DateTime.UtcNow,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
            return _departmentsRepository.Add(Department);
        }

        public int UpdateDepartment(UpdateDepartmentDto departmentDto)
        {
            var Department = new Department()
            {
                Id = departmentDto.Id,
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
            return _departmentsRepository.Update(Department);
        }
        public bool DeleteDepartment(int id)
        {
            var department = _departmentsRepository.Get(id);
            if (department is { })
                return _departmentsRepository.Delete(department) > 0;
            return false;
        }

      

    }
}
