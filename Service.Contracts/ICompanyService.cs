using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetCompanies(bool trackChanges);
    CompanyDto GetCompany(Guid companyId, bool trackChanges);  
}
