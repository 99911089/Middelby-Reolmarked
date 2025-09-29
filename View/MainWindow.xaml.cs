using Reolmarked.Model;
using Reolmarked.Repository.DbRepo;
using Reolmarked.Repository.IRepo;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Reolmarked.View
{
    // MainWindow håndterer både reoler og kunder (lejere)
    public partial class MainWindow : Window
    {
        private RackMarket market;           // Håndterer alle reoler
        private List<Customer> _customers;   // Liste over kunder/lejere
        private ICustomerRepository _repo;   // Repository til databaseoperationer
        private List<Rack> racks = new List<Rack>(); // Liste med reoler

        public MainWindow()
        {
            InitializeComponent(); // Dansk kommentar: linker XAML til code-behind

            // Initialiser repository med databaseforbindelse
            _repo = new DbCustomerRepository(new Database().GetConnectionString());

            LoadCustomers(); // Hent og vis alle kunder fra databasen

            // Initialiser RackMarket (samling af alle reoler)
            market = new RackMarket();

            // Tilføj 43 reoler med standardværdier
            for (int i = 1; i <= 43; i++)
            {
                market.Racks.Add(new Rack
                {
                    RackId = i,           // unik ID for hver reol
                    AmountShelves = 5,    // antal hylder pr. reol
                    HangerBar = true,     // reolen har stang
                    IsOccupied = false,   // reolen er ledig som standard
                    ProductName = ""      // ingen produkt endnu
                });
            }

            // Eksempeldata (kan være du henter fra fil eller database)
            racks.Add(new Rack { RackId = 1, AmountShelves = 4, HangerBar = true, IsOccupied = true, ProductName = "Jakke" });
            racks.Add(new Rack { RackId = 2, AmountShelves = 3, HangerBar = false, IsOccupied = false, ProductName = "" });
            racks.Add(new Rack { RackId = 3, AmountShelves = 5, HangerBar = true, IsOccupied = false, ProductName = "" });

            // Når programmet starter, vis alle racks
            RackListView.ItemsSource = racks;
        }

        // ------------------ Reol-knapper ------------------

        // Vis alle reoler som er optaget (IsOccupied = true)
        private void ShowRented_Click(object sender, RoutedEventArgs e)
        {
            List<Rack> rentedRacks = new List<Rack>();

            foreach (Rack r in racks)
            {
                if (r.IsOccupied)
                {
                    rentedRacks.Add(r);
                }
            }

            RackListView.ItemsSource = null;
            RackListView.ItemsSource = rentedRacks;
        }

        // Vis alle reoler som har bøjlestang (HangerBar = true)
        // Vis alle reoler som har bøjlestang (HangerBar = true)
        private void ShowHangers_Click(object sender, RoutedEventArgs e)
        {
            List<string> hangerRacksText = new List<string>();

            foreach (Rack r in racks)
            {
                if (r.HangerBar)
                {
                    hangerRacksText.Add(r.ToHangerString()); // Brug speciel tekst
                }
            }

            RackListView.ItemsSource = null;
            RackListView.ItemsSource = hangerRacksText;
        }

        private void SelectOccupiedRack_Click(object sender, RoutedEventArgs e)
        {
            // Liste til optagede reoler
            List<Rack> occupied = new List<Rack>();

            foreach (Rack r in racks)
            {
                if (r.IsOccupied)
                {
                    occupied.Add(r);
                }
            }

            if (occupied.Count == 0)
            {
                MessageBox.Show("Der er ingen optagede reoler.");
                return;
            }

            // Vis de optagede reoler i ListView
            RackListView.ItemsSource = null;
            RackListView.ItemsSource = occupied;

            MessageBox.Show("Vælg en reol fra listen ovenfor."); // Guiding tekst
        }


        // Vis alle reoler i racks-listen
        private void ShowAllRack_Click(object sender, RoutedEventArgs e)
        {
            RackListView.ItemsSource = null;
            RackListView.ItemsSource = racks;
        }

        // Opdater ListView med alle reoler i market
        private void UpdateRackDisplay()
        {
            RackListView.ItemsSource = null;
            RackListView.ItemsSource = market.Racks;
        }

        // Vis ledige reoler (uden LINQ)
        private void ShowAvailable_Click(object sender, RoutedEventArgs e)
        {
            List<Rack> available = new List<Rack>();

            foreach (Rack r in market.Racks)
            {
                if (!r.IsOccupied)
                {
                    available.Add(r);
                }
            }

            RackListView.ItemsSource = null;
            RackListView.ItemsSource = available;
        }

        // Vis optagede reoler (uden LINQ)
        private void ShowOccupied_Click(object sender, RoutedEventArgs e)
        {
            List<Rack> occupied = new List<Rack>();

            foreach (Rack r in market.Racks)
            {
                if (r.IsOccupied)
                {
                    occupied.Add(r);
                }
            }

            RackListView.ItemsSource = null;
            RackListView.ItemsSource = occupied;
        }

        // Vis alle reoler fra market
        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateRackDisplay();
        }

        // Sælg produkt på valgt reol
        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            if (RackListView.SelectedItem is Rack selectedRack && selectedRack.IsOccupied)
            {
                double commission = market.SellProduct(selectedRack.RackId, 500); // Eksempel: pris 500
                MessageBox.Show($"Commission: {commission} kr.");
                UpdateRackDisplay();
            }
            else
            {
                MessageBox.Show("Select an occupied rack first.");
            }

        }

        // ------------------ Kunde/Lejer ------------------

        private void LoadCustomers()
        {
            _customers = _repo.GetAllCustomers();

            CustomerListView.ItemsSource = null;
            CustomerListView.ItemsSource = _customers;
        }

        // Åbn separat vindue til administration af lejere
        private void ManageCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.ShowDialog();

            LoadCustomers();
        }

        // Rediger valgt kunde
        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = CustomerListView.SelectedItem as Customer;

            if (selectedCustomer != null)
            {
                // Åbn EditCustomerWindow med ShowDialog
                var editWindow = new EditCustomerWindow(_repo, selectedCustomer);

                // Kald ShowDialog én gang og gem resultat
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadCustomers(); // Opdater ListView med nye data
                }
            }
            else
            {
                MessageBox.Show("Vælg en lejer først.");
            }
        }


        // Slet valgt kunde
        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = CustomerListView.SelectedItem as Customer;

            if (selectedCustomer != null)
            {
                if (MessageBox.Show("Er du sikker på, du vil slette denne lejer?", "Bekræft", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _repo.DeleteCustomer(selectedCustomer.CustomerId);
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Vælg en lejer først.");
            }
        }

        // Betalingsfunktion
        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentMethodComboBox.SelectedItem is ComboBoxItem selectedMethod)
            {
                string method = selectedMethod.Content.ToString();

                switch (method)
                {
                    case "Kort":
                        MessageBox.Show("Betaling med kort gennemført!");
                        break;

                    case "Kontant":
                        MessageBox.Show("Betaling med kontanter gennemført!");
                        break;

                    case "MobilePay":
                        MessageBox.Show("Betaling med MobilePay gennemført!");
                        break;

                    default:
                        MessageBox.Show("Vælg venligst en betalingsmetode!");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Vælg venligst en betalingsmetode!");
            }
        }
    }
}
