using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.DTO;
using EmployeeApp.Models;
using EmployeeApp.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpPost]
        public IActionResult AddCompany([FromBody] CompanyRequest companyRequest)
        {
            ResponseDTO<Company> responseDTO = new ResponseDTO<Company>();
            try
            {
                Company company = new Company()
                {
                    Name = companyRequest.Name,
                    Address = companyRequest.Address
                };
                var c = _companyService.AddOrUpdateCompany(company);
                if (c is not null)
                {

                    responseDTO.Data = c;
                }
                return StatusCode(201, responseDTO);


            }
            catch (Exception)
            {

                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);

            }
        }
        [HttpGet]
        public IActionResult GetAllCompany()
        {
            ResponseDTO<IEnumerable<Company>> responseDTO = new ResponseDTO<IEnumerable<Company>>();
            try
            {
                responseDTO.Data = _companyService.GetCompanies();
                return StatusCode(200, responseDTO);
            }
            catch (Exception)
            {
                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            ResponseDTO<bool> responseDTO = new ResponseDTO<bool>();
            try
            {
                if (_companyService.DeleteCompany(id))
                {
                    responseDTO.Data = true;
                }
                else
                {
                    responseDTO.Success = false;
                    responseDTO.Data = false;
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

        [HttpGet("{id}")]
        public IActionResult GetCompanyById(int id)
        {
            ResponseDTO<Company> responseDTO = new ResponseDTO<Company>();
            try
            {
                var c = _companyService.GetCompanyById(id);
                if (c is not null)
                {

                    responseDTO.Data = c;
                    return StatusCode(200, responseDTO);
                }
                else
                {
                    responseDTO.Success = false;
                    responseDTO.ErrorMessage = "Company not found";
                    return StatusCode(404, responseDTO);

                }

            }
            catch (Exception)
            {
                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCompanyById(int id, [FromBody] CompanyRequest companyRequest)
        {
            var responseDTO = new ResponseDTO<Company>();

            try
            {
                var company = _companyService.GetCompanyById(id);

                if (company is null)
                {
                    responseDTO.Success = false;
                    responseDTO.ErrorMessage = "Company not fount";
                    return NotFound(responseDTO);
                }

                if (!string.IsNullOrEmpty(companyRequest.Name))
                {
                    company.Name = companyRequest.Name;
                }
                if (!string.IsNullOrEmpty(companyRequest.Address))
                {
                    company.Address = companyRequest.Address;
                }

                var updateCompany = _companyService.AddOrUpdateCompany(company);
                if (updateCompany is not null)
                {

                    responseDTO.Data = updateCompany;
                }

                return Ok(responseDTO);


            }
            catch (Exception)
            { 
                responseDTO.Success = false;
                responseDTO.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(500, responseDTO);
            }
        }

        [HttpPost("{id}/add-employee/{idE}")]
        public IActionResult AddEmployeeToCompany(int id, int idE){
            var responseDTO = new ResponseDTO<Company>();

            try
            {
                _companyService.AddEmployeeToCompany(id, idE);

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