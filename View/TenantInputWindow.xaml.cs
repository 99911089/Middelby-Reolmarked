using Reolmarked.Model;
using System.Windows;

namespace Reolmarked.View
{
    public partial class TenantInputWindow : Window
    {
        // Offentlig property til at hente/aflevere den valgte kunde
        public Tenant Tenant { get; private set; }

        // Konstruktør til NY kunde
        public TenantInputWindow()
        {
            InitializeComponent();
            Tenant = new Tenant(); // tom kunde
        }

        // Konstruktør til at REDIGERE eksisterende kunde
        public TenantInputWindow(Tenant tenant) : this()
        {
            Tenant = tenant;

            // Forudfyld tekstfelter
            NameTextBox.Text = tenant.TenantName;
            EmailTextBox.Text = tenant.TenantEmail;
            PhoneTextBox.Text = tenant.TenantPhone;
        }

        // Når brugeren trykker OK
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Tenant.TenantName = NameTextBox.Text;
            Tenant.TenantEmail = EmailTextBox.Text;
            Tenant.TenantPhone = PhoneTextBox.Text;

            DialogResult = true; // Luk vindue og returnér "OK"
        }

        // Hvis brugeren trykker Annuller
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Luk uden at gemme
        }
    }
}
