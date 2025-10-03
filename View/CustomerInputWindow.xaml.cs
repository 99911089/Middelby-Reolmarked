using Reolmarked.Model;
using System.Windows;

namespace Reolmarked.View
{
    public partial class CustomerInputWindow : Window
    {
        // Kunde vi arbejder med
        public Customer Customer { get; private set; }

        // Konstruktør til ny kunde
        public CustomerInputWindow()
        {
            InitializeComponent();
        }

        // Konstruktør til at redigere eksisterende kunde
        public CustomerInputWindow(Customer selectedCustomer)
        {
            InitializeComponent();

            // Gem reference
            Customer = selectedCustomer;

            // Udfyld tekstbokse
            if (Customer != null)
            {
                NameTextBox.Text = Customer.CustomerName;
                EmailTextBox.Text = Customer.CustomerEmail;
                PhoneTextBox.Text = Customer.CustomerPhone;
            }
        }

        // Når man trykker OK
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Customer == null)
            {
                // Ny kunde
                Customer = new Customer();
            }

            // Gem værdier
            Customer.CustomerName = NameTextBox.Text;
            Customer.CustomerEmail = EmailTextBox.Text;
            Customer.CustomerPhone = PhoneTextBox.Text;

            this.DialogResult = true;
        }

        // Når man trykker Annuller
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
