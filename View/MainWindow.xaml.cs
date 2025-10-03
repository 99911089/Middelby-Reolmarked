using Reolmarked.Model;                  // Indeholder Customer, Rack, Sale klasser
using Reolmarked.Model.Reolmarked.Model;
using System;                            // Til DateTime
using System.Collections.ObjectModel;    // Til ObservableCollection
using System.IO;                         // Til MemoryStream
using System.Linq;                       // Til Where (filtrering af lister)
using System.Windows;                    // Til Window, MessageBox osv.
using System.Windows.Controls;           // Til MenuItem
using System.Windows.Media.Imaging;      // Til BitmapImage
using ZXing;                             // Til stregkode
using ZXing.Common;                      // Til EncodingOptions

namespace Reolmarked.View
{
    public partial class MainWindow : Window
    {
        // ========================== FELTER OG LISTE-BINDING ==========================

        private int _nextRackId;   // Holder styr på næste ledige reol-ID

        public ObservableCollection<Customer> Customers { get; set; }     // Kunde-liste
        public ObservableCollection<Rack> Racks { get; set; }             // Reol-liste
        public ObservableCollection<Sale> SalesHistory { get; set; }      // Salgshistorik-liste

        // ========================== KONSTRUKTØR ==========================

        public MainWindow()
        {
            InitializeComponent();

            // Initialiser collections
            Customers = new ObservableCollection<Customer>();
            Racks = new ObservableCollection<Rack>();
            SalesHistory = new ObservableCollection<Sale>();

            // Bind collections til UI
            CustomerListView.ItemsSource = Customers;
            RackListView.ItemsSource = Racks;
            SalesHistoryListView.ItemsSource = SalesHistory;

            // Tilføj lidt testdata
            Customers.Add(new Customer("Anders And", "anders@andeby.dk", "12345678"));
            Racks.Add(new Rack { RackId = 1, RackName = "Reol A", IsAvailable = true });
            SalesHistory.Add(new Sale { ProductName = "Cola", Price = 15, Barcode = "1234567890123", SoldDate = DateTime.Now });

            // Næste ledige ID til nye reoler
            _nextRackId = 2;
        }

        // ========================== KUNDER ==========================

        // Tilføj ny kunde
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerInputWindow win = new CustomerInputWindow();
            if (win.ShowDialog() == true)
            {
                Customers.Add(win.Customer);
            }
        }

        // Rediger valgt kunde
        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerListView.SelectedItem is Customer selected)
            {
                CustomerInputWindow win = new CustomerInputWindow(selected);
                win.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vælg en kunde først.");
            }
        }

        // Slet valgt kunde
        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerListView.SelectedItem is Customer selected)
            {
                Customers.Remove(selected);
            }
            else
            {
                MessageBox.Show("Vælg en kunde først.");
            }
        }

        // ========================== REOLER ==========================

        // Vis ledige reoler
        private void ShowAvailable_Click(object sender, RoutedEventArgs e)
        {
            RackListView.ItemsSource = new ObservableCollection<Rack>(Racks.Where(r => r.IsAvailable));
        }

        // Vis optagede reoler
        private void ShowOccupied_Click(object sender, RoutedEventArgs e)
        {
            RackListView.ItemsSource = new ObservableCollection<Rack>(Racks.Where(r => !r.IsAvailable));
        }

        // Vis alle reoler
        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            RackListView.ItemsSource = Racks;
        }

        // Tilføj en ny reol
        private void AddRack_Click(object sender, RoutedEventArgs e)
        {
            Racks.Add(new Rack
            {
                RackId = _nextRackId,
                RackName = "Reol " + _nextRackId,
                IsAvailable = true
            });

            _nextRackId++; // Forbered næste ID
        }

        // Skift status (ledig/optaget) på valgt reol
        private void ToggleRackStatus_Click(object sender, RoutedEventArgs e)
        {
            if (RackListView.SelectedItem is Rack selectedRack)
            {
                selectedRack.IsAvailable = !selectedRack.IsAvailable;
                RackListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Vælg en reol i listen først.", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // ========================== SALG ==========================

        // Sælg produkt via stregkode
        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SellBarcodeTextBox.Text))
            {
                Sale sale = new Sale
                {
                    ProductName = "Ukendt",
                    Price = 100,
                    Barcode = SellBarcodeTextBox.Text,
                    SoldDate = DateTime.Now
                };

                SalesHistory.Add(sale);

                // Generer stregkode og vis i popup
                ShowBarcodeWindow(sale.Barcode);

                SellBarcodeTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Indtast eller scan en stregkode først.");
            }
        }

        // Åbn produktvindue
        private void OpenProductWindow_Click(object sender, RoutedEventArgs e)
        {
            ProductInputWindow win = new ProductInputWindow();
            win.ShowDialog();
        }

        // Betaling
        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item && item.Tag is string metode)
            {
                MessageBox.Show("Betaling med: " + metode);
            }
        }

        // ========================== STREGKODE ==========================

        private void ShowBarcodeWindow(string barcode)
        {
            BitmapImage img = GenerateBarcode(barcode);

            Window win = new Window
            {
                Title = "Stregkode: " + barcode,
                Width = 300,
                Height = 150,
                Content = new System.Windows.Controls.Image { Source = img }
            };

            win.ShowDialog();
        }

        private BitmapImage GenerateBarcode(string text)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 80,
                    Width = 250,
                    Margin = 2
                }
            };

            var pixelData = writer.Write(text);

            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                var bitmapData = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                using (var memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }
        }
    }
}
