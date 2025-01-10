using Payhub.Domain.Entities.CustomerManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface ICustomerRepository : IReadRepository<Customer>, IWriteRepository<Customer>{}