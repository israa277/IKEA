﻿using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistence.Repositories.Employess;
using LinkDev.IKEA.DAL.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    public class EmployeeServices : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        //  private readonly IEmployeeRepository _employeeRepository;
        public EmployeeServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            // _employeeRepository = employeeRepository;
        }
        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
             _unitOfWork.EmployeeRepository.Add(employee);
            return _unitOfWork.Complete();
        }
        public int UpdateEmployee(UpdateEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
             _unitOfWork.EmployeeRepository.Update(employee);
            return _unitOfWork.Complete();
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(id);
            if (employee is { })
                 _unitOfWork.EmployeeRepository.Delete(employee)  ;
            return _unitOfWork.Complete()>0;
        }
        public IEnumerable<EmployeeDto> GetAllEmployees(string Search)
        {
            var employees = _unitOfWork.EmployeeRepository.GetAllAsIQueryable()
                .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(Search) || E.Name.ToLower().Contains(Search.ToLower())))
                //.Include(E => E./*Department*/)
                .Select(employee => new EmployeeDto()
                {

                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Gender = nameof(employee.Gender),
                    EmployeeType = nameof(employee.EmployeeType),
                   // Department = employee.Department.Name


                }).ToList();



            return employees;
        }
        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(id);
            if (employee is { })
                return new EmployeeDetailsDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType
                };
            return null;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            throw new NotImplementedException();
        }
    }
}
