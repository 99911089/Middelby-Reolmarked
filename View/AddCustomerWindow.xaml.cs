using System.Windows;
using Reolmarked.Model;
using Reolmarked.Repository.IRepo;

namespace Reolmarked.View
{
    public partial class AddCustomerWindow : Window
    {
        private readonly ICustomerRepository _repo;

        // Konstruktør tager repository
        public AddCustomerWindow(ICustomerRepository repo)
        {
            InitializeComponent();
            _repo = repo;
        }

        // Gem-knap
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer
            {
                CustomerName = NameTextBox.Text,
                CustomerEmail = EmailTextBox.Text,
                CustomerPhone = PhoneTextBox.Text
            };

            _repo.CreateCustomer(customer);
            this.Close();
        }
    }
}

