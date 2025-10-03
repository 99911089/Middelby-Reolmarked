using System.Collections.ObjectModel;
using System.ComponentModel;
using Reolmarked.Repository.IRepo;
using Reolmarked.Repository.DbRepo;

namespace Reolmarked.ViewModel
{
    // ViewModel som binder data til UI
    public class RackViewModel : INotifyPropertyChanged
    {
        private IRackRepository repository;

        // Samling af racks som UI kan binde til
        public ObservableCollection<Rack> Racks { get; set; }

        // Event til property changes (til WPF-binding)
        public event PropertyChangedEventHandler PropertyChanged;

        // Konstruktor
        public RackViewModel()
        {
            repository = new DbRackRepository();
            Racks = new ObservableCollection<Rack>();

            // Tilføj test-data
            repository.AddRack(new Rack(1, "Reol A1", true));
            repository.AddRack(new Rack(2, "Reol B2", false));
            repository.AddRack(new Rack(3, "Reol C3", true));

            // Indlæs alle racks ind i ObservableCollection
            foreach (Rack rack in repository.GetAllRacks())
            {
                Racks.Add(rack);
            }
        }

        // Metode til at "notify" UI
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

