using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeApp.Configs;
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

        public readonly IMapper _mapper;
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> loggerManager, IMapper mapper)
        {
            _employeeService = employeeService;
            _logger = loggerManager;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult AddEmployee([FromBody] EmployeeRequest employeeRequest)
        {
            ResponseDTO<Employee> response = new ResponseDTO<Employee>();

            try
            {
                _logger.LogInformation($"Adding a new employee: {employeeRequest.Phone}");

                Employee employee = _mapper.Map<Employee>(employeeRequest);
                response.Data = _employeeService.AddOrUpdateEmployee(employee);

                _logger.LogInformation($"Employee {employeeRequest.Phone} added successfully.");

                return StatusCode(201, response);
            }
            catch (SqlException ex) when (ex.Number == 2601)
            {
                // Handle specific SQL exception for duplicate key violation
                _logger.LogWarning($"Duplicate key violation: The phone number {employeeRequest.Phone} already exists.");

                response.Success = false;
                response.ErrorMessage = $"Duplicate key violation: The phone number {employeeRequest.Phone} already exists.";
                return StatusCode(400, response);
            }
            catch (SqlException ex)
            {
                // Handle other SQL exceptions
                _logger.LogError($"SQL error: {ex.Message}");

                response.Success = false;
                response.ErrorMessage = $"SQL error: {ex.Message}";
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                // Handle other generic exceptions
                _logger.LogError($"An unexpected error occurred: {ex.Message}");

                response.Success = false;
                response.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllEmployee([FromQuery] Dictionary<string, string> param)
        {
            ResponseDTO<IEnumerable<Employee>> response = new ResponseDTO<IEnumerable<Employee>>();
            try
            {
                _logger.LogInformation("Fetching all the Employee from the storage");

                // Additional logging for query parameters (optional)
                if (param != null && param.Count > 0)
                {
                    _logger.LogInformation($"Query parameters: {string.Join(", ", param.Select(p => $"{p.Key}={p.Value}"))}");
                }

                response.Data = _employeeService.GetAll(param);

                _logger.LogInformation("Employees fetched successfully.");

                return StatusCode(200, response);
            }
            catch (Exception)
            {
                _logger.LogError("An unexpected error occurred.");

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
                _logger.LogInformation($"Fetching Employee with ID: {id}");

                var employee = _employeeService.GetById(id);

                if (employee is not null)
                {
                    responseDTO.Success = true;
                    responseDTO.Data = employee;

                    _logger.LogInformation($"Employee with ID {id} found successfully.");
                }

                return StatusCode(200, responseDTO);
            }
            catch (Exception)
            {
                _logger.LogError("An unexpected error occurred.");

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
                _logger.LogInformation($"Deleting Employee with ID: {id}");

                if (_employeeService.DeleteEmployee(id))
                {
                    responseDTO.Data = true;
                    _logger.LogInformation($"Employee with ID {id} deleted successfully.");
                }
                else
                {
                    responseDTO.Data = false;
                    _logger.LogInformation($"Employee with ID {id} not found or could not be deleted.");
                }
                return StatusCode(200, responseDTO);
            }
            catch (Exception)
            {
                _logger.LogError("An unexpected error occurred.");

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
                _logger.LogInformation($"Updating Employee with ID: {id}");

                var employee = _employeeService.GetById(id);

                if (employee == null)
                {
                    responseDTO.Success = false;
                    responseDTO.ErrorMessage = "Employee not found";
                    _logger.LogWarning($"Employee with ID {id} not found for update.");
                    return NotFound(responseDTO);
                }

                _mapper.Map(updateRequest, employee);
                var updatedEmployee = _employeeService.AddOrUpdateEmployee(employee);

                responseDTO.Data = updatedEmployee;

                _logger.LogInformation($"Employee with ID {id} updated successfully.");

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
        public IActionResult AddCompanyToEmployee(int id, int idC)
        {
            var responseDTO = new ResponseDTO<string>();

            try
            {
                _logger.LogInformation($"Adding Company with ID {idC} to Employee with ID {id}");

                _employeeService.AddCompanyToEmployee(id, idC);

                _logger.LogInformation($"Company with ID {idC} added to Employee with ID {id} successfully.");

                return Ok(responseDTO);
            }
            catch (Exception)
            {
                _logger.LogError("An unexpected error occurred.");

                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);
            }
        }

        [HttpGet("count")]
        public IActionResult getCount()
        {
            var responseDTO = new ResponseDTO<int>();
            try
            {
                var count = _employeeService.countEmployee();
                int pageSize = 10;
                int result = (int) Math.Ceiling(count * 1.0 / pageSize);
                responseDTO.Data = result;
                return Ok(responseDTO);
            }
            catch (Exception)
            {
                _logger.LogError("An unexpected error occurred.");

                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);
            }
        }

    }

}