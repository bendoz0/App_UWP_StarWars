using ProgettoEsame_JacopoBendotti.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoEsame_JacopoBendotti.ViewModels
{
    internal class PersonViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Planet> PlanetsList { get; set; } = new();
        public ObservableCollection<Person> PeopleList { get; set; } = new();
        public ObservableCollection<Vehicle> VehiclesList { get; set; } = new();
        public ObservableCollection<Starship> StarshipList { get; set; } = new();
        public static ObservableCollection<Person> PeopleListSelected { get; set; } = new();

        private ObservableCollection<Person> _filteredPeopleList = new();

        private ObservableCollection<Person> _currentPeopleList;


        private readonly Dictionary<Person, bool> _selectionStates = new();



        private string _searchText;
        private bool _isSaveButtonEnabled;



        public PersonViewModel()
        {
            // Mostra tutte le card all'inizio
            CurrentPeopleList = PeopleList;

            //Intercetta le modifiche di IseSelected per ogni persona
            PeopleList.CollectionChanged += PeopleList_CollectionChanged;
        }



        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterPeopleList(); 
                }
            }
        }

        public ObservableCollection<Person> CurrentPeopleList
        {
            get => _currentPeopleList;
            set
            {
                _currentPeopleList = value;
                OnPropertyChanged(nameof(CurrentPeopleList));
            }
        }

        public ObservableCollection<Person> FilteredPeopleList
        {
            get => _filteredPeopleList;
            set
            {
                _filteredPeopleList = value;
                OnPropertyChanged(nameof(FilteredPeopleList));
            }
        }

        private void FilterPeopleList()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                CurrentPeopleList = PeopleList;
            }
            else
            {
                var filtered = PeopleList
                    .Where(p => p.Name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                (p.Planet?.Name != null && p.Planet.Name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                CurrentPeopleList = new ObservableCollection<Person>(filtered);
            }
        }


        private void PeopleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Person person in e.NewItems)
                {
                    person.PropertyChanged += Person_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Person person in e.OldItems)
                {
                    person.PropertyChanged -= Person_PropertyChanged;
                }
            }
        }

        private void Person_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Person.IsSelected))
            {
                var person = sender as Person;

                if (person != null)
                {
                    if (person.IsSelected && !PeopleListSelected.Contains(person))
                    {
                        PeopleListSelected.Add(person);
                    }
                    else if (!person.IsSelected && PeopleListSelected.Contains(person))
                    {
                        PeopleListSelected.Remove(person);
                    }

                    // Aggiorna lo stato del bottone
                    IsSaveButtonEnabled = PeopleListSelected.Count > 0;
                }
            }
        }


        public bool IsSaveButtonEnabled
        {
            get => _isSaveButtonEnabled;
            set
            {
                if (_isSaveButtonEnabled != value)
                {
                    _isSaveButtonEnabled = value;
                    OnPropertyChanged(nameof(IsSaveButtonEnabled));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
