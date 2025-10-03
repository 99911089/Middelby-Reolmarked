using Reolmarked.Model;
using System.Windows;

namespace Reolmarked.View
{
    public partial class ProductInputWindow : Window
    {
        public ProductInputWindow()
        {
            InitializeComponent();
        }

        public ProductInputWindow(Product selectedProduct)
        {
            InitializeComponent(); // Husk at køre XAML-opbygning
            SelectedProduct = selectedProduct;
        }

        public Product SelectedProduct { get; }
        public Product Product { get; internal set; }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Gem input i Product-objekt
            Product = new Product
            {
                Name = NameTextBox.Text,
                Barcode = BarcodeTextBox.Text,
                Price = (double)(decimal.TryParse(PriceTextBox.Text, out decimal price) ? price : 0)
            };

            this.DialogResult = true; // Luk vindue med "OK"
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Luk vindue med "Annuller"
        }
    }
}
