using System;
using System.Windows;
using Reolmarked.Model;

namespace Reolmarked.View
{
    public partial class AddProductWindow : Window
    {
        // Produktet, der bliver oprettet
        public Product NewProduct { get; private set; }

        public AddProductWindow()
        {
            InitializeComponent();
        }

        // Tryk på "Gem"
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Simpel validering
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Udfyld alle felter.");
                return;
            }

            decimal price;
            if (!decimal.TryParse(PriceTextBox.Text, out price))
            {
                MessageBox.Show("Pris skal være et tal.");
                return;
            }

            // Opret nyt produkt
            NewProduct = new Product();
            NewProduct.ProductName = NameTextBox.Text;
            NewProduct.Price = (double)price;
            NewProduct.Barcode = BarcodeTextBox.Text;
            NewProduct.CustomerId = int.TryParse(CustomerIdTextBox.Text, out int id) ? id : 0;

            DialogResult = true;
            Close();
        }

        // Tryk på "Annuller"
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
