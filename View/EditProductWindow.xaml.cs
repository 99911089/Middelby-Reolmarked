using System;
using System.Windows;
using Reolmarked.Model;

namespace Reolmarked.View
{
    /// <summary>
    /// Vindue til redigering af et produkt.
    /// </summary>
    public partial class EditProductWindow : Window
    {
        // Reference til det produkt, der redigeres
        private readonly Product _product;

        /// <summary>
        /// Konstruktor – modtager det produkt, som skal redigeres.
        /// </summary>
        public EditProductWindow(Product product)
        {
            InitializeComponent();

            // Gem reference til produktet
            _product = product;

            // Fyld tekstfelterne med eksisterende data
            NameTextBox.Text = product.ProductName;
            PriceTextBox.Text = product.Price.ToString();
            BarcodeTextBox.Text = product.Barcode;
            CustomerIdTextBox.Text = product.CustomerId.ToString(); // Brug CustomerId, ikke OwnerCustomerId
        }

        public EditProductWindow()
        {
        }

        /// <summary>
        /// Klik på "Gem" – opdaterer produktets værdier.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Tjek at felterne ikke er tomme
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Indtast produktnavn.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(PriceTextBox.Text))
                {
                    MessageBox.Show("Indtast pris.");
                    return;
                }

                // Konverter pris til decimal
                decimal price;
                if (!decimal.TryParse(PriceTextBox.Text, out price))
                {
                    MessageBox.Show("Pris skal være et gyldigt tal (fx 49.95).");
                    return;
                }

                // Opdater produktets data
                _product.ProductName = NameTextBox.Text.Trim();
                _product.Price = (double)price;
                _product.Barcode = BarcodeTextBox.Text.Trim();

                int customerId;
                if (int.TryParse(CustomerIdTextBox.Text, out customerId))
                    _product.CustomerId = customerId;

                // Luk vinduet og signaler succes
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fejl ved opdatering af produkt: " + ex.Message);
            }
        }

        /// <summary>
        /// Klik på "Annuller" – lukker vinduet uden at gemme.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
