using System.Collections.ObjectModel;
using System.ComponentModel;
using Reolmarked.Model;
using Reolmarked.Repository.IRepo;
using Reolmarked.Repository.DbRepo;

namespace Reolmarked.ViewModel
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private readonly ICustomerRepository _repo;
        private ObservableCollection<Customer> _customers;

        public CustomerViewModel()
        {
            _repo = new DbCustomerRepository();
            Refresh();
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public void Refresh()
        {
            Customers = new ObservableCollection<Customer>(_repo.GetAllCustomers());
        }

        public void AddCustomer(Customer c) => _repo.AddCustomer(c);
        public void UpdateCustomer(Customer c) => _repo.UpdateCustomer(c);
        public void DeleteCustomer(int id) => _repo.DeleteCustomer(id);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
