using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Interface;

namespace Auth.Domain.Factories;

public class DepartmentFactory : IDepartmentFactory
{
    public Department Create()
    {
        return new Department("人事部");
    }
}