using Models.Core.Employees;

namespace FreyaApi.Tests.Controllers.Services.Builders;

internal class EmployeeControllerServiceBuilder
{

    private JwtSecurityTokenService jwtService;
    private Mock<IEmailService> emailService;
    private Mock<IEmployeeRepository> employeeRepository;
    private Mock<IClinicRepository> clinicRepository;

    private List<EmployeeTable> employees = new List<EmployeeTable>();

    public EmployeeControllerServiceBuilder()
    {
        jwtService = new JwtSecurityTokenServiceBuilder().Build();
        emailService = new Mock<IEmailService>();
        employeeRepository = new Mock<IEmployeeRepository>();
        clinicRepository = new Mock<IClinicRepository>();
    }

    internal EmployeeControllerServices Build()
    {
        if (employees.Any())
        {
            foreach (EmployeeTable employee in employees)
            {
                employeeRepository
                    .Setup(x => x.GetEmployeeWithClinicData(It.Is<string>(s => s.Equals(employee.Email))))
                    .ReturnsAsync(employee);

                employeeRepository
                    .Setup(x => x.GetEmployee(It.Is<string>(s => s.Equals(employee.Email))))
                    .ReturnsAsync(employee);

                employeeRepository
                    .Setup(x => x.EmployeeExists(It.Is<string>(s => s.Equals(employee.Email))))
                    .Returns(true);
            }
        }

        return new EmployeeControllerServices(
            jwtService,
            emailService.Object,
            employeeRepository.Object,
            clinicRepository.Object);
    }

    internal EmployeeControllerServiceBuilder WithEmployee(string email, string pass, EmployeeRol rol = EmployeeRol.CLINIC_OFFICER)
    {
        employees.Add(new EmployeeTable()
        {
            Id = Guid.NewGuid(),
            Email = email,
            Pass = pass,
            Rol = rol,
        });

        return this;
    }
    
    internal EmployeeControllerServiceBuilder WithEmployeeWithClinic(string email, string pass,Guid companyId, ICollection<ClinicTable> Clinics, EmployeeRol rol = EmployeeRol.CLINIC_OFFICER)
    {
        employees.Add(new EmployeeTable()
        {
            Id = Guid.NewGuid(),
            Email = email,
            Pass = pass,
            Rol = rol,
            CompanyId = companyId,
            Clinics = Clinics,
        });

        foreach (var clinic in Clinics)
        {
            clinicRepository
                .Setup(z => z.GetClinic(It.Is<Guid>(s => s.Equals(clinic.Id))))
                .Returns(clinic);
        }

        return this;
    }

    internal EmployeeControllerServiceBuilder AddClinic(ClinicTable clinic)
    {
        clinicRepository
                .Setup(z => z.GetClinic(It.Is<Guid>(s => s.Equals(clinic.Id))))
                .Returns(clinic);
        return this;
    }
}
