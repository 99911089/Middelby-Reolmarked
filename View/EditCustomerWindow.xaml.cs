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

            // Fyld felterne med kundens nuværende data
            NameTextBox.Text = _customer.CustomerName;
            EmailTextBox.Text = _customer.CustomerEmail;
            PhoneTextBox.Text = _customer.CustomerPhone;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Opdater kunden med nye værdier
            _customer.CustomerName = NameTextBox.Text;
            _customer.CustomerEmail = EmailTextBox.Text;
            _customer.CustomerPhone = PhoneTextBox.Text;

            // Gem i databasen
            _repo.UpdateCustomer(_customer);

            MessageBox.Show("Kunde opdateret!");
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
