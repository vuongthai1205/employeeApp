using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeApp.DTO;
using EmployeeApp.Models;

namespace EmployeeApp.Mappings
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {

            CreateMap<EmployeeRequest, Employee>();
            CreateMap<CompanyRequest, Company>();
        }
    }
}