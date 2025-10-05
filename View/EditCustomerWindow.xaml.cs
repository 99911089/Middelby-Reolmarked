using System;
using System.Windows;
using Reolmarked.Model;

namespace Reolmarked.View
{
    public partial class EditCustomerWindow : Window
    {
        private Customer _customer; // Den kunde der redigeres

        // ------------------------------
        // KONSTRUKTOR
        // ------------------------------
        public EditCustomerWindow(Customer existingCustomer)
        {
            InitializeComponent();

            // Gem referencen til kunden
            _customer = existingCustomer;

            // Udfyld tekstfelterne med eksisterende data
            NameTextBox.Text = _customer.CustomerName;
            AddressTextBox.Text = _customer.Address;
            PhoneTextBox.Text = _customer.Phone;
            EmailTextBox.Text = _customer.Email;
        }

        // ------------------------------
        // GEM ændringer
        // ------------------------------
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Opdater kundens oplysninger ud fra tekstfelterne
            _customer.CustomerName = NameTextBox.Text.Trim();
            _customer.Address = AddressTextBox.Text.Trim();
            _customer.Phone = PhoneTextBox.Text.Trim();
            _customer.Email = EmailTextBox.Text.Trim();

            // Simpelt tjek for tomme felter
            if (_customer.CustomerName == "")
            {
                MessageBox.Show("Kunden skal have et navn.");
                return;
            }

            // Luk vinduet og send 'true' tilbage
            this.DialogResult = true;
            this.Close();
        }

        // ------------------------------
        // ANNULLER ændringer
        // ------------------------------
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
