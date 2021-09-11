using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleCodingChallenge.Business.Actions.Employees;
using SimpleCodingChallenge.Common.DTO;
using SimpleCodingChallenge.DataAccess.Database;
using SimpleCodingChallenge.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleCodingChallenge.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class EmployeesController : Controller
    {
        private readonly IMediator mediator;
        private readonly SimpleCodingChallengeDbContext _context;

        public EmployeesController(IMediator mediator, SimpleCodingChallengeDbContext context)
        {
            this.mediator = mediator;
            _context = context;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<EmployeeDto>>> Index()
        {
            var result = await mediator.Send(new GetAllEmployeesCommand());
            return result.EmployeeList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(long id)
        {
            var todoItem = await _context.Employees.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return EmployeeToDTO(todoItem);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<EmployeeDto>> Create(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                EmployeeID = employeeDto.EmployeeID,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                //FullName = employeeDto.FullName,
                Address = employeeDto.Address,
                Email = employeeDto.Email,
                BirthDate = DateTime.Parse(employeeDto.BirthDate),
                JobTitle = employeeDto.JobTitle,
                Salary = employeeDto.Salary,
                Department = employeeDto.Department,

            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, EmployeeToDTO(employee));
        }

        private static EmployeeDto EmployeeToDTO(Employee employee) =>
        new EmployeeDto
        {
            EmployeeID = employee.EmployeeID,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
        //FullName = employeeDto.FullName,
            Address = employee.Address,
            Email = employee.Email,
            BirthDate =employee.BirthDate.ToString(),
            JobTitle = employee.JobTitle,
            Salary = employee.Salary,
            Department = employee.Department,
        };
    }

}

