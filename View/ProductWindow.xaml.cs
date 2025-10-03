using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using Reolmarked.Model;
using ZXing; // bibliotek til stregkoder

namespace Reolmarked.View
{
    public partial class ProductWindow : Window
    {
        private List<Product> _products;

        public Tenant SelectedTenant { get; }

        public ProductWindow()
        {
            InitializeComponent();

            // Eksempeldata
            _products = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Cola", Price = 15, Barcode = "1234567890" },
                new Product { ProductId = 2, ProductName = "Chips", Price = 20, Barcode = "9876543210" }
            };

            UpdateProductDisplay();
        }

        public ProductWindow(Tenant selectedTenant)
        {
            SelectedTenant = selectedTenant;
        }

        // Opdater listen med produkter
        private void UpdateProductDisplay()
        {
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = _products;
        }

        // Tilføj et nyt produkt
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var inputWindow = new ProductInputWindow();
            inputWindow.Owner = this;

            if (inputWindow.ShowDialog() == true)
            {
                var newProduct = inputWindow.Product;
                newProduct.ProductId = _products.Count + 1;

                // Hvis der ikke er sat en stregkode, generér en baseret på ID
                if (string.IsNullOrWhiteSpace(newProduct.Barcode))
                {
                    newProduct.Barcode = $"P{newProduct.ProductId:00000}";
                }

                _products.Add(newProduct);
                UpdateProductDisplay();

                // Vis stregkoden i UI
                ShowBarcode(newProduct);
            }
        }

        // Rediger eksisterende produkt
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                var inputWindow = new ProductInputWindow(selectedProduct);
                inputWindow.Owner = this;

                if (inputWindow.ShowDialog() == true)
                {
                    // Hvis der stadig mangler stregkode → generér automatisk
                    if (string.IsNullOrWhiteSpace(selectedProduct.Barcode))
                    {
                        selectedProduct.Barcode = $"P{selectedProduct.ProductId:00000}";
                    }

                    UpdateProductDisplay();

                    // Vis stregkoden i UI
                    ShowBarcode(selectedProduct);
                }
            }
            else
            {
                MessageBox.Show("Vælg et produkt først.");
            }
        }

        // Slet produkt
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                if (MessageBox.Show("Er du sikker på, du vil slette dette produkt?",
                    "Bekræft", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _products.Remove(selectedProduct);
                    UpdateProductDisplay();

                    // Ryd stregkode-visning
                    BarcodeTextBox.Text = "";
                    BarcodeImage.Source = null;
                }
            }
            else
            {
                MessageBox.Show("Vælg et produkt først.");
            }
        }

        // Når man vælger et produkt i listen
        private void ProductListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                ShowBarcode(selectedProduct);
            }
        }

        // Hjælpefunktion → viser stregkoden for et produkt
        private void ShowBarcode(Product product)
        {
            BarcodeTextBox.Text = product.Barcode;
            BarcodeImage.Source = GenerateBarcode(product.Barcode);
        }

        
        // Generér stregkode-billede
        private BitmapImage GenerateBarcode(string text)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128, // Standard stregkode
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 80,
                    Width = 250,
                    Margin = 2
                }
            };

            var pixelData = writer.Write(text);

            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bitmap.PixelFormat);

                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                    pixelData.Pixels.Length);
                bitmap.UnlockBits(bitmapData);

                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                var image = new BitmapImage();
                ms.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}
