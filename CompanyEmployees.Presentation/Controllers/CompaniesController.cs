using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public CompaniesController(IServiceManager serviceManager) => _serviceManager = serviceManager;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _serviceManager.CompanyService.GetCompanies(trackChanges: false);
            return Ok(companies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = _serviceManager.CompanyService.GetByIds(ids, trackChanges: false);
            return Ok(companies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}", Name = "GetCompany")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _serviceManager.CompanyService.GetCompany(id, trackChanges: false);
            return Ok(company);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
            {
                return BadRequest("CompanyForCreationDto object is null.");
            }

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var createdCompany = _serviceManager.CompanyService.CreateComppany(company);

            return CreatedAtRoute("GetCompany", new { id = createdCompany.Id }, createdCompany);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyCollection"></param>
        /// <returns></returns>
        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var (companies, ids) = _serviceManager.CompanyService.CreateCompanyCollection(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { ids }, companies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id)
        {
            _serviceManager.CompanyService.DeleteCompany(id, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            if (company is null)
                return BadRequest("CompanyForUpdateDto object is null.");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _serviceManager.CompanyService.UpdateCompany(id, company, trackChanges: true);

            return NoContent();
        }
    }
}
