using Reolmarked.Model;
using Reolmarked.Model.Reolmarked.Model;
using System.Collections.Generic;

namespace Reolmarked.Repository.IRepo
{
    /// <summary>
    /// Interface for Customer repository – definerer CRUD operationer.
    /// </summary>
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        void AddCustomer(Customer newCustomer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        void CreateCustomer(Customer customer);
    }

    public interface IRackRepository
    {
        List<Rack> GetAllRacks();
        void AddRack(Rack rack);
        void UpdateRack(Rack rack);
        void DeleteRack(int id);
    }

    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }

    public interface IPaymentRepository
    {
        List<Payment> GetAllPayments();
        void AddPayment(Payment payment);
    }
}
