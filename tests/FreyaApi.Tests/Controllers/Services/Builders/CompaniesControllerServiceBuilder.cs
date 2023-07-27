using Microsoft.Extensions.Configuration;
using Models.Core.Clinics;
using Models.Core.Companies;
using System.ComponentModel.Design;
using System.Linq;

namespace FreyaApi.Tests.Controllers.Services.Builders;

internal class CompaniesControllerServiceBuilder
{
    private Mock<IConfiguration> configuration;
    private readonly Mock<IProductsPaymentsService> productsPaymentsService;
    private readonly Mock<ICompaniesRepository> companiesRepository;
    private readonly Mock<IClinicRepository> clinicRepository;
    private readonly Mock<IEmployeeRepository> employeeRepository;
    private readonly Mock<ISubscriptionProductRepository> subscriptionProductRepository;
    private readonly Mock<ISubscriptionPlanRepository> subscriptionPlanRepository;

    private List<Company> listCompanies = new List<Company>();
    private List<Clinic> listClinic = new List<Clinic>();

    public CompaniesControllerServiceBuilder()
    {
        productsPaymentsService = new Mock<IProductsPaymentsService>();
        companiesRepository = new Mock<ICompaniesRepository>();
        clinicRepository = new Mock<IClinicRepository>();
        employeeRepository = new Mock<IEmployeeRepository>();
        subscriptionPlanRepository = new Mock<ISubscriptionPlanRepository>();
        subscriptionProductRepository = new Mock<ISubscriptionProductRepository>();
        configuration = new Mock<IConfiguration>();

        //var configurationSection = new Mock<IConfigurationSection>();
        //configurationSection.Setup(x => x.Value).Returns(Consts.Test_Key);
        //configuration.Setup(x => x.GetSection(It.Is<string>(k => k == "Jwt:Key"))).Returns(configurationSection.Object);
    }

    internal CompaniesControllerService Build()
    {
        if (listCompanies.Any())
        {
            companiesRepository.Setup(x => x.GetCompanies()).Returns(listCompanies.AsQueryable());
            foreach (var company in listCompanies)
            {
                companiesRepository.Setup(x => x.DeleteCompany(It.Is<Guid>(y => y == company.Id))).ReturnsAsync(true);
            }
        }

        if (listClinic.Any())
        {
            foreach (var clinic in listClinic)
            {
                var iquery = new List<ClinicTable>() { new ClinicTable() { CompanyId = clinic.CompanyId, Id = clinic.Id, Name = clinic.Name } }.AsQueryable();
                companiesRepository.Setup(x => x.GetClinicsForCompany(It.Is<Guid>(y => y == clinic.CompanyId), null)).Returns(iquery);
            }
            
        }

        return new CompaniesControllerService(
            productsPaymentsService.Object,
            companiesRepository.Object,
            subscriptionProductRepository.Object,
            subscriptionPlanRepository.Object,
            clinicRepository.Object,
            employeeRepository.Object,
            configuration.Object);
    }

    internal CompaniesControllerServiceBuilder WithCompany(string companyName)
    {
        listCompanies.Add(new Company() { Name = companyName });
        return this;
    }
    
    internal CompaniesControllerServiceBuilder WithCompany(Guid id, string companyName)
    {
        listCompanies.Add(new Company() { Id = id, Name = companyName });
        return this;
    }
    
    internal CompaniesControllerServiceBuilder WithClinic(Guid companyId, Guid id, string clinicName)
    {
        listClinic.Add(new Clinic() { Id = id, CompanyId = companyId, Name = clinicName });
        
        return this;
    }
}
