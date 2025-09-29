using Reolmarked.Model;
using Reolmarked.Model.Reolmarked.Model;
using System.Collections.Generic;
using System.Windows;

namespace Reolmarked.View
{
    // Partial class matcher XAML
    public partial class CustomerWindow : Window
    {
        private List<Tenant> _tenants; // Liste over lejere

        public CustomerWindow()
        {
            InitializeComponent(); // VIGTIGT: linker XAML til code-behind

            // Eksempellejere
            _tenants = new List<Tenant>
            {
                new Tenant { TenantId = 1, TenantName = "Per Bauer", TenantEmail = "per@example.com", TenantPhone = "12345678" },
                new Tenant { TenantId = 2, TenantName = "Anna Hansen", TenantEmail = "anna@example.com", TenantPhone = "87654321" }
            };

            UpdateTenantDisplay(); // Opdater ListView
        }

        // Opdater ListView med alle lejere
        private void UpdateTenantDisplay()
        {
            TenantListView.ItemsSource = null;      // Fjern gammel binding
            TenantListView.ItemsSource = _tenants;  // Bind ny liste
        }

        // Tilføj ny lejer
        private void AddTenant_Click(object sender, RoutedEventArgs e)
        {
            int newId = _tenants.Count + 1;  // Tildel nyt ID
            _tenants.Add(new Tenant { TenantId = newId, TenantName = "Ny Lejer", TenantEmail = "email@example.com", TenantPhone = "00000000" });
            UpdateTenantDisplay();            // Opdater ListView
        }

        // Rediger valgt lejer
        private void EditTenant_Click(object sender, RoutedEventArgs e)
        {
            if (TenantListView.SelectedItem is Tenant selectedTenant)
            {
                selectedTenant.TenantName += " (redigeret)"; // Eksempel: tilføj (redigeret) til navn
                UpdateTenantDisplay();                        // Opdater ListView
            }
            else
            {
                MessageBox.Show("Vælg en lejer først.");     // Dansk kommentar
            }
        }

        // Slet valgt lejer
        private void DeleteTenant_Click(object sender, RoutedEventArgs e)
        {
            if (TenantListView.SelectedItem is Tenant selectedTenant)
            {
                if (MessageBox.Show("Er du sikker på, du vil slette denne lejer?", "Bekræft", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _tenants.Remove(selectedTenant);        // Fjern valgt lejer
                    UpdateTenantDisplay();                  // Opdater ListView
                }
            }
            else
            {
                MessageBox.Show("Vælg en lejer først.");     // Dansk kommentar
            }
        }
    }
}
