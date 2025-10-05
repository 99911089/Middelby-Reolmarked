using System;
using System.Collections.ObjectModel;
using System.Windows;
using Reolmarked.Model;

namespace Reolmarked.View
{
    /// <summary>
    /// Vindue til administration af produkter – tilføj, rediger og slet.
    /// </summary>
    public partial class ProductWindow : Window
    {
        // Repository giver adgang til produktdata i databasen
        private readonly ProductRepository _productRepo = new ProductRepository();

        // Liste over produkter, som automatisk opdateres i UI
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public Tenant SelectedTenant { get; }

        // Konstruktor (standard)
        public ProductWindow()
        {
            InitializeComponent();

            // Binder ObservableCollection til ListView
            ProductList.ItemsSource = Products;

            // Indlæser produkter fra databasen
            LoadProducts();
        }

        public ProductWindow(Tenant selectedTenant)
        {
            SelectedTenant = selectedTenant;
        }

        // ===================== HENT PRODUKTER =====================
        private void LoadProducts()
        {
            Products.Clear();

            // Henter alle produkter fra databasen
            foreach (Product p in _productRepo.GetAllProducts())
            {
                Products.Add(p);
            }
        }

        // ===================== TILFØJ PRODUKT =====================
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            // Inputfelter via simple popup-dialoger
            string name = Microsoft.VisualBasic.Interaction.InputBox("Produktnavn:", "Tilføj produkt");
            string priceStr = Microsoft.VisualBasic.Interaction.InputBox("Pris:", "Tilføj produkt");
            string barcode = Microsoft.VisualBasic.Interaction.InputBox("Stregkode:", "Tilføj produkt");
            string custStr = Microsoft.VisualBasic.Interaction.InputBox("Kunde ID:", "Tilføj produkt");

            // Tjek at input er gyldigt
            if (!string.IsNullOrWhiteSpace(name) &&
                decimal.TryParse(priceStr, out decimal price) &&
                int.TryParse(custStr, out int customerId))
            {
                // Opret nyt produkt
                Product p = new Product();
                p.ProductName = name;
                p.Price = (double)price;
                p.Barcode = barcode;
                p.CustomerId = customerId;

                // Gem produkt i databasen og få ID tilbage
                int newId = _productRepo.AddProduct(p);
                p.ProductId = newId;

                // Tilføj produktet til den viste liste
                Products.Add(p);

                MessageBox.Show("Produkt tilføjet!");
            }
            else
            {
                MessageBox.Show("Ugyldige værdier. Kontroller at pris og kunde-ID er tal.");
            }
        }

        // ===================== REDIGER PRODUKT =====================
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            // Sørg for at et produkt er valgt
            Product selected = ProductList.SelectedItem as Product;
            if (selected == null)
            {
                MessageBox.Show("Vælg et produkt, du vil redigere.");
                return;
            }

            // Åbn små inputbokse med eksisterende værdier udfyldt
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Nyt navn:", "Rediger produkt", selected.ProductName);
            string newPriceStr = Microsoft.VisualBasic.Interaction.InputBox("Ny pris:", "Rediger produkt", selected.Price.ToString());
            string newBarcode = Microsoft.VisualBasic.Interaction.InputBox("Ny stregkode:", "Rediger produkt", selected.Barcode);
            string newCustIdStr = Microsoft.VisualBasic.Interaction.InputBox("Nyt kunde-ID:", "Rediger produkt", selected.CustomerId.ToString());

            // Tjek for gyldige værdier
            if (decimal.TryParse(newPriceStr, out decimal newPrice) &&
                int.TryParse(newCustIdStr, out int newCustId))
            {
                // Opdater produktet
                selected.ProductName = newName;
                selected.Price = (double)newPrice;
                selected.Barcode = newBarcode;
                selected.CustomerId = newCustId;

                // Gem ændringer i databasen
                _productRepo.UpdateProduct(selected);

                // Opdater listen på skærmen
                ProductList.Items.Refresh();

                MessageBox.Show("Produkt opdateret!");
            }
            else
            {
                MessageBox.Show("Ugyldige værdier. Tjek pris og kunde-ID.");
            }
        }

        // ===================== SLET PRODUKT =====================
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            Product selected = ProductList.SelectedItem as Product;

            if (selected == null)
            {
                MessageBox.Show("Vælg et produkt, du vil slette.");
                return;
            }

            // Bekræft sletning
            MessageBoxResult result = MessageBox.Show(
                "Er du sikker på, at du vil slette produktet '" + selected.ProductName + "'?",
                "Bekræft sletning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                _productRepo.DeleteProduct(selected.ProductId);
                Products.Remove(selected);
                MessageBox.Show("Produkt slettet!");
            }
        }

        // ===================== LUK VINDUET =====================
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
