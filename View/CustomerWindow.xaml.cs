using Reolmarked.Model;
using Reolmarked.Repository.DbRepo;
using System.Collections.Generic;
using System.Windows;

namespace Reolmarked.View
{
    public partial class CustomerWindow : Window
    {
        private DbTenantRepository _repo; // Repository til database
        private List<Tenant> _tenants;    // Liste af kunder

        public CustomerWindow()
        {
            InitializeComponent();

            // Connection string til SQL Server
            _repo = new DbTenantRepository("Server=Server01;Database=ReolDb;Trusted_Connection=True;TrustServerCertificate=True;");

            // Hent kunder fra databasen når vinduet åbnes
            LoadTenants();
        }

        // Hent kunder fra DB og vis i ListView
        private void LoadTenants()
        {
            _tenants = _repo.GetAllTenants();
            TenantListView.ItemsSource = null;  // Fjern gammel binding
            TenantListView.ItemsSource = _tenants; // Sæt ny
        }

        // Tilføj ny kunde
        private void AddTenant_Click(object sender, RoutedEventArgs e)
        {
            // Åbn inputvindue uden data (ny kunde)
            var input = new TenantInputWindow();
            if (input.ShowDialog() == true)
            {
                _repo.AddTenant(input.Tenant); // Gem i DB
                LoadTenants(); // Opdater liste
            }
        }

        // Rediger en eksisterende kunde
        private void EditTenant_Click(object sender, RoutedEventArgs e)
        {
            if (TenantListView.SelectedItem is Tenant selectedTenant)
            {
                // Åbn inputvindue med eksisterende kunde
                var input = new TenantInputWindow(selectedTenant);
                if (input.ShowDialog() == true)
                {
                    _repo.UpdateTenant(selectedTenant); // Gem ændringer i DB
                    LoadTenants(); // Opdater liste
                }
            }
            else
            {
                MessageBox.Show("Vælg en kunde først.");
            }
        }

        // Slet en kunde
        private void DeleteTenant_Click(object sender, RoutedEventArgs e)
        {
            if (TenantListView.SelectedItem is Tenant selectedTenant)
            {
                if (MessageBox.Show("Er du sikker på at du vil slette denne kunde?",
                                    "Bekræft sletning",
                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _repo.DeleteTenant(selectedTenant.TenantId); // Slet i DB
                    LoadTenants(); // Opdater liste
                }
            }
            else
            {
                MessageBox.Show("Vælg en kunde først.");
            }
        }

        // Åbn produktvindue for valgt kunde
        private void OpenProducts_Click(object sender, RoutedEventArgs e)
        {
            if (TenantListView.SelectedItem is Tenant selectedTenant)
            {
                // Opret et nyt vindue til at vise/redigere produkter
                var productWindow = new ProductWindow(selectedTenant);

                // Åbn som dialog (blokerer indtil man lukker vinduet)
                productWindow.Owner = this;
                productWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vælg en lejer først, før du kan se produkter.");
            }
        }

    }
}
