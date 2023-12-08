using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.DTO;
using EmployeeApp.Models;
using EmployeeApp.Services;
using EmployeeApp.Services.ServicesImpl;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService _employeeService;
        private readonly ILogger _logger;
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> loggerManager)
        {
            _employeeService = employeeService;
            _logger = loggerManager;
        }
        [HttpPost]
        public IActionResult AddEmployee([FromBody] EmployeeRequest employeeRequest)
        {
            ResponseDTO<Employee> response = new ResponseDTO<Employee>();

            try
            {
                Employee employee = new Employee()
                {
                    LastName = employeeRequest.LastName,
                    FirstName = employeeRequest.FirstName,
                    Phone = employeeRequest.Phone
                };
                if (employeeRequest.Phone.IsNullOrEmpty())
                {
                    response.Success = false;
                    response.ErrorMessage = "Phone is require";
                    return StatusCode(404, response);
                }

                response.Data = _employeeService.AddOrUpdateEmployee(employee);
                return StatusCode(201, response);


            }
            catch (Exception)
            {

                response.Success = false;
                response.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, response);

            }




        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllEmployee()
        {
            ResponseDTO<IEnumerable<Employee>> response = new ResponseDTO<IEnumerable<Employee>>();
            try
            {
                _logger.LogInformation("Fetching all the Employee from the storage");
                response.Data = _employeeService.GetAll();

                return StatusCode(200, response);
            }
            catch (Exception)
            {
                response.Success = false;
                response.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, response);
            }

        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            ResponseDTO<Employee> responseDTO = new ResponseDTO<Employee>();
            try
            {
                var employee = _employeeService.GetById(id);
                if (employee is not null)
                {
                    responseDTO.Success = true;
                    responseDTO.Data = employee;
                }
                return StatusCode(200, responseDTO);
            }
            catch (Exception)
            {
                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);

            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteEmployeeById(int id)
        {
            ResponseDTO<bool> responseDTO = new ResponseDTO<bool>();
            try
            {
                if (_employeeService.DeleteEmployee(id))
                {
                    responseDTO.Data = true;
                }
                else
                {
                    responseDTO.Data = false;
                }
                return StatusCode(200, responseDTO);
            }
            catch (Exception)
            {
                responseDTO.ErrorMessage = "Error server";
                responseDTO.Success = false;
                return StatusCode(500, responseDTO);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeById(int id, [FromBody] EmployeeRequest updateRequest)
        {
            var responseDTO = new ResponseDTO<Employee>();

            try
            {
                var employee = _employeeService.GetById(id);

                if (employee == null)
                {
                    responseDTO.Success = false;
                    responseDTO.ErrorMessage = "Employee not found";
                    return NotFound(responseDTO);
                }

                // Update employee details
                if (!string.IsNullOrEmpty(updateRequest.LastName))
                    employee.LastName = updateRequest.LastName;

                if (!string.IsNullOrEmpty(updateRequest.FirstName))
                    employee.FirstName = updateRequest.FirstName;

                // Check if Phone is provided in the update request
                if (updateRequest.Phone != null)
                    employee.Phone = updateRequest.Phone;

                var updatedEmployee = _employeeService.AddOrUpdateEmployee(employee);

                responseDTO.Data = updatedEmployee;

                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                responseDTO.Success = false;
                responseDTO.ErrorMessage = "Internal server error";
                _logger.LogError(ex, "Error updating employee");
                return StatusCode(500, responseDTO);
            }
        }
        [HttpPost("{id:int}/add-company/{idC:int}")]
        public IActionResult AddCompanyToEmployee(int id, int idC){
            var responseDTO = new ResponseDTO<Company>();

            try
            {
                _employeeService.AddCompanyToEmployee(id, idC);

                return Ok(responseDTO);


            }
            catch (Exception)
            {
                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);
            }
        }

    }
}