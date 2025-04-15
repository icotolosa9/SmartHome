using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using ModeloValidador.Abstracciones;
using Models.In;
using Models.Out;
using System.Reflection;

namespace BusinessLogic
{
    public class CompanyLogic : ICompanyLogic
    {
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        public CompanyLogic(ICompanyRepository companyRepository, IUserRepository userRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        public Company CreateCompany(Company company, string emailOwner)
        {
            User user = _userRepository.GetUserByEmail(emailOwner);
            CompanyOwner owner = (CompanyOwner)user;

            if (owner.CompanyId != null)
            {
                throw new CompanyOwnerAlreadyHasACompanyException();
            }
            company.Owner = owner;
            company.OwnerId = owner.Id;
            owner.CompanyId = company.CompanyId;

            if (_companyRepository.GetAll().Any(existingCompany => existingCompany.Equals(company)))
            {
                throw new CompanyAlreadyExistsException();
            }

            _companyRepository.Save(company);
            _userRepository.Update(owner);
            return company;
        }

        public PagedResult<CompanyDto> ListCompanies(ListCompaniesRequest request)
        {
            PagedResult<Company> pagedCompanies = _companyRepository.GetPagedCompanies(request);

            List<CompanyDto> companyDtos = pagedCompanies.Results.Select(company => new CompanyDto
            {
                CompanyId = company.CompanyId,
                CompanyRut = company.Rut,
                CompanyName = company.Name,
                CompanyOwnerFullName = _userRepository.GetUserById(company.OwnerId).FirstName + " " + _userRepository.GetUserById(company.OwnerId).LastName,
                CompanyOwnerEmail = company.Owner.Email,
            }).ToList();

            return new PagedResult<CompanyDto>(
                companyDtos,
                pagedCompanies.TotalCount,
                pagedCompanies.PageNumber,
                pagedCompanies.PageSize
            );
        }

        public List<IModeloValidador> LoadValidators()
        {
            string baseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string validatorsPath = Path.Combine(baseDirectory, "ValidatorModels");
            if (!Directory.Exists(validatorsPath))
            {
                throw new DirectoryNotFoundException($"La carpeta '{validatorsPath}' no fue encontrada.");
            }

            string[] filePaths = Directory.GetFiles(validatorsPath, "*.dll");
            List<IModeloValidador> availableValidators = new List<IModeloValidador>();

            foreach (var file in filePaths)
            {
                try
                {
                    FileInfo dllFile = new FileInfo(file);
                    Assembly myAssembly = Assembly.LoadFile(dllFile.FullName);

                    foreach (Type type in myAssembly.GetTypes())
                    {
                        if (typeof(IModeloValidador).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            IModeloValidador validatorInstance = (IModeloValidador)Activator.CreateInstance(type);
                            availableValidators.Add(validatorInstance);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar el archivo DLL: {ex.Message}");
                }
            }

            return availableValidators;
        }

        public CompanyDto? GetCompanyByOwner(Guid userId)
        {
            User user = _userRepository.GetUserById(userId);
            CompanyOwner owner = (CompanyOwner)user;

            if (owner.CompanyId != null)
            {
                Company company = _companyRepository.GetCompanyById(owner.CompanyId.Value);
                CompanyDto companyDto = new CompanyDto
                {
                    CompanyId = company.CompanyId,
                    CompanyRut = company.Rut,
                    CompanyName = company.Name,
                    CompanyOwnerFullName = owner.FirstName + " " + owner.LastName,
                    CompanyOwnerEmail = owner.Email
                };
                return companyDto;
            }
            else return null;
        }
    }
}
