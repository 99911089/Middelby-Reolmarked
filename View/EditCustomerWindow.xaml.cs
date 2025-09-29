using System.Windows;
using Reolmarked.Model;
using Reolmarked.Repository.IRepo;

namespace Reolmarked.View
{
    public partial class EditCustomerWindow : Window
    {
        private readonly ICustomerRepository _repo;
        private readonly Customer _customer;

        public EditCustomerWindow(ICustomerRepository repo, Customer customer)
        {
            InitializeComponent();
            _repo = repo;
            _customer = customer;

            NameTextBox.Text = _customer.CustomerName;
            EmailTextBox.Text = _customer.CustomerEmail;
            PhoneTextBox.Text = _customer.CustomerPhone;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _customer.CustomerName = NameTextBox.Text;
            _customer.CustomerEmail = EmailTextBox.Text;
            _customer.CustomerPhone = PhoneTextBox.Text;

            _repo.UpdateCustomer(_customer);
            this.Close();
        }
    }
}
