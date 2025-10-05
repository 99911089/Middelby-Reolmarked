using System;
using System.Globalization;
using System.Windows.Data;

namespace Reolmarked.View
{
    // Denne converter bruges i XAML til at vise tekst i stedet for boolske værdier.
    // F.eks. konverterer "true" → "Ledig" og "false" → "Optaget".
    public class BoolToStatusConverter : IValueConverter
    {
        // Kaldes når data skal vises i UI (fra bool til tekst)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Hvis værdien er en bool
            if (value is bool b)
            {
                // Returner passende tekst
                if (b)
                    return "Ledig";
                else
                    return "Optaget";
            }

            // Hvis noget går galt
            return "Ukendt";
        }

        // Kaldes hvis UI-værdien ændres og skal sendes tilbage til objektet (tekst → bool)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Hvis brugeren har valgt en tekststreng
            if (value is string s)
            {
                if (s.Equals("Ledig", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (s.Equals("Optaget", StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            // Hvis teksten ikke kan oversættes, gør ingenting
            return Binding.DoNothing;
        }
    }
}
