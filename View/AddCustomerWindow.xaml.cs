using System.Windows;
using Reolmarked.Model;

namespace Reolmarked.View
{
    public partial class AddCustomerWindow : Window
    {
        public Customer NewCustomer { get; private set; }

        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string address = AddressTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();

            if (name == "")
            {
                MessageBox.Show("Navn skal udfyldes!");
                return;
            }

            NewCustomer = new Customer
            {
                CustomerName = name,
                Address = address,
                Phone = phone,
                Email = email
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
