using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Reolmarked.Model;

namespace Reolmarked.View
{
    public partial class MainWindow : Window
    {
        // ================= REPOSITORIES =================
        private readonly CustomerRepository _customerRepo = new CustomerRepository();
        private readonly SaleRepository _saleRepo = new SaleRepository();
        private readonly ProductRepository _productRepo = new ProductRepository();
        private readonly RackRepository _rackRepo = new RackRepository();

        // ================= OBSERVABLE LISTS =================
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();
        public ObservableCollection<Sale> SalesHistory { get; set; } = new ObservableCollection<Sale>();
        public ObservableCollection<Rack> Racks { get; set; } = new ObservableCollection<Rack>();

        // ================= KONSTRUKTOR =================
        public MainWindow()
        {
            InitializeComponent();

            // Binder data til lister i XAML
            CustomerListView.ItemsSource = Customers;
            SalesHistoryListView.ItemsSource = SalesHistory;
            RackTree.ItemsSource = Racks;

            // Indlæs data fra databasen
            LoadCustomers();
            LoadSales();
            LoadRacks();
        }

        // ================= KUNDER =================
        private void LoadCustomers()
        {
            Customers.Clear();
            List<Customer> all = _customerRepo.GetAllCustomers();
            foreach (Customer c in all)
                Customers.Add(c);
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow addWindow = new AddCustomerWindow();
            addWindow.Owner = this;
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                Customer newCustomer = addWindow.NewCustomer;
                _customerRepo.AddCustomer(newCustomer);
                LoadCustomers();
                MessageBox.Show("Kunde tilføjet!");
            }
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer selected = (Customer)CustomerListView.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Vælg en kunde, du vil redigere.");
                return;
            }

            EditCustomerWindow editWindow = new EditCustomerWindow(selected);
            editWindow.Owner = this;
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                _customerRepo.UpdateCustomer(selected);
                LoadCustomers();
                MessageBox.Show("Kunde opdateret!");
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer selected = (Customer)CustomerListView.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Vælg en kunde, du vil slette.");
                return;
            }

            if (MessageBox.Show($"Er du sikker på, at du vil slette {selected.CustomerName}?",
                "Bekræft sletning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _customerRepo.DeleteCustomer(selected.CustomerId);
                LoadCustomers();
                MessageBox.Show("Kunde slettet!");
            }
        }

        // ================= REOLER =================
        private void LoadRacks()
        {
            Racks.Clear();
            foreach (Rack r in _rackRepo.GetAllRacks())
                Racks.Add(r);
        }

        // Viser kun ledige reoler
        private void ShowAvailable_Click(object sender, RoutedEventArgs e)
        {
            List<Rack> available = new List<Rack>();
            foreach (Rack r in _rackRepo.GetAllRacks())
                if (r.IsAvailable)
                    available.Add(r);

            RackTree.ItemsSource = available;
        }

        // Viser kun optagede reoler
        private void ShowOccupied_Click(object sender, RoutedEventArgs e)
        {
            List<Rack> occupied = new List<Rack>();
            foreach (Rack r in _rackRepo.GetAllRacks())
                if (!r.IsAvailable)
                    occupied.Add(r);

            RackTree.ItemsSource = occupied;
        }

        // Viser alle reoler igen
        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadRacks();
        }

        // Tilføjer ny reol
        private void AddRack_Click(object sender, RoutedEventArgs e)
        {
            Rack r = new Rack();
            r.RackName = "Ny reol " + DateTime.Now.ToString("HHmmss");
            r.IsAvailable = true;
            r.Hangers = new List<Hanger>();

            int id = _rackRepo.AddRack(r);
            r.RackId = id;
            LoadRacks();
        }

        // Skifter status (ledig/optaget)
        private void ToggleRackStatus_Click(object sender, RoutedEventArgs e)
        {
            Rack selected = RackTree.SelectedItem as Rack;
            if (selected == null)
            {
                MessageBox.Show("Vælg en reol først.");
                return;
            }

            selected.IsAvailable = !selected.IsAvailable;
            _rackRepo.UpdateRack(selected);
            LoadRacks();
        }

        // ================= PRODUKTER =================
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addWindow = new AddProductWindow();
            addWindow.Owner = this;
            addWindow.ShowDialog();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            EditProductWindow editWindow = new EditProductWindow();
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Her kunne du slette et produkt.");
        }

        // Åbn produktadministration
        private void OpenProductWindow_Click(object sender, RoutedEventArgs e)
        {
            // Åbn det rigtige vindue i stedet for MessageBox
            ProductWindow win = new ProductWindow();
            win.Owner = this;           // gør vinduet modalt over hovedvinduet
            win.ShowDialog();
        }


        // ================= SALG =================
        private void LoadSales()
        {
            SalesHistory.Clear();
            foreach (Sale s in _saleRepo.GetAllSales())
                SalesHistory.Add(s);
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            // 1️⃣ Læs stregkode fra tekstfeltet
            string barcode = SellBarcodeTextBox.Text.Trim();
            if (barcode == "")
            {
                MessageBox.Show("Indtast eller scan en stregkode.");
                return;
            }

            // 2️⃣ Find produktet i databasen via stregkoden
            Product product = _productRepo.GetProductByBarcode(barcode);
            if (product == null)
            {
                MessageBox.Show("Produkt med denne stregkode blev ikke fundet.");
                return;
            }

            // 3️⃣ Opret nyt salg
            Sale sale = new Sale();
            sale.ProductId = product.ProductId;
            sale.CustomerId = (int?)product.CustomerId;
            sale.Price = (int)product.Price;
            sale.SoldDate = DateTime.Now;

            // 4️⃣ Gem i databasen
            int saleId = _saleRepo.AddSale(sale);
            sale.SaleId = saleId;
            sale.ProductName = product.ProductName;
            sale.Barcode = product.Barcode;

            // 5️⃣ Opdater UI (Salgshistorik)
            SalesHistory.Add(sale);

            // 6️⃣ Ryd tekstfelt og giv besked
            SellBarcodeTextBox.Clear();
            MessageBox.Show($"Produkt '{product.ProductName}' solgt for {product.Price} kr.");
        }


        private void EditSale_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Her kunne du redigere et salg.");
        }

        private void DeleteSale_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Her kunne du slette et salg.");
        }

        // ================= BETALING =================
        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe != null && fe.Tag != null)
            {
                string metode = fe.Tag.ToString();
                MessageBox.Show("Betaling valgt: " + metode);
            }
        }
    }

    
}
