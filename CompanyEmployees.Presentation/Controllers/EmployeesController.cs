﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeesController(IServiceManager serviceManager) => _serviceManager = serviceManager;

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = _serviceManager.EmployeeService
                .GetEmployees(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = _serviceManager.EmployeeService
                .GetEmployee(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
        {
            if (employeeForCreation is null)
            {
                return BadRequest("EmployeeForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("Custom field", "Custom field Error Message");
                return UnprocessableEntity(ModelState);
            }

            var employeeToReturn = _serviceManager.EmployeeService
                .CreateEmployeeForCompany(companyId, employeeForCreation, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForUpdateDto object is null");

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _serviceManager.EmployeeService
                .UpdateEmployeeForCompany(companyId, id, employee, trackCompanyChanges: false, trackEmployeeChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateEmployeeForCompany(
            Guid companyId,
            Guid id,
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc
        )
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null");

            var (employeeToPatch, employeeEntity) = _serviceManager.EmployeeService
                .GetEmployeeForPatch(companyId, id, trackCompanyChanges: false, trackEmployeeChanges: true);

            patchDoc.ApplyTo(employeeToPatch, ModelState);
            TryValidateModel(employeeToPatch);
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _serviceManager.EmployeeService.SaveChangesForPatch(employeeToPatch, employeeEntity);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            _serviceManager.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);
            return NoContent();
        }
    }
}
