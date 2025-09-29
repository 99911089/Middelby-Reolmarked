using Reolmarked.Model;
using System.Collections.Generic;

namespace Reolmarked.Repository.IRepo
{
    public interface ICustomerRepository
    {
        // Tilføj ny kunde
        void CreateCustomer(Customer customer);

        // Hent alle kunder
        List<Customer> GetAllCustomers();

        // Hent kunde efter ID
        Customer GetById(int customerId);

        // Opdater kunde
        void UpdateCustomer(Customer customer);

        // Slet kunde
        void DeleteCustomer(int customerId);
    }
}
