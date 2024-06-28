using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges) ??
            throw new CompanyNotFoundException(companyId);

        var employeesFromDb = _repository.Employee.GetEmployees(company.Id, trackChanges);

        return _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges) ??
            throw new CompanyNotFoundException(companyId);

        var employeeFromDb = _repository.Employee.GetEmployee(company.Id, employeeId, trackChanges) ??
            throw new EmployeeNotFoundException(employeeId);

        return _mapper.Map<EmployeeDto>(employeeFromDb);
    }

}
