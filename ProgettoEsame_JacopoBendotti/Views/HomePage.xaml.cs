using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ProgettoEsame_JacopoBendotti.Models;
using HttpClient = System.Net.Http.HttpClient;
using ProgettoEsame_JacopoBendotti.ViewModels;
using System.Diagnostics;



namespace ProgettoEsame_JacopoBendotti.Views
{

    internal sealed partial class HomePage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _urlPeople = "https://swapi.dev/api/people/";
        private readonly string _urlPlanets = "https://swapi.dev/api/planets/";
        private readonly string _urlVehicles = "https://swapi.dev/api/vehicles/";
        private readonly string _urlStarships = "https://swapi.dev/api/starships/";


        public PersonViewModel personViewModel = new();


        public HomePage()
        {
            this.InitializeComponent();

            DataContext = personViewModel;

            this.Loaded += HomePage_Loaded;
        }

        private async void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            await FetchPlanets();
            await FetchPerson();
        }


        private async Task FetchPerson()
        {
            for (int i = 1; i < 84; i++)
            {
                try
                {
                    var person = await _httpClient.GetFromJsonAsync<Person>(_urlPeople + i);

                    if (person != null)
                    {
                        if (!string.IsNullOrEmpty(person.Homeworld))
                        {
                            // Estrai l'ID dal URL Homeworld
                            var match = System.Text.RegularExpressions.Regex.Match(person.Homeworld, @"(\d+)/$");
                            if (match.Success)
                            {
                                int planetId = int.Parse(match.Groups[1].Value);

                                // Cerca il pianeta nella lista PlanetsList
                                var planet = personViewModel.PlanetsList.FirstOrDefault(p => p.Id == planetId);
                                if (planet != null)
                                {
                                    // Associo il pianeta trovato dall'id alla persona
                                    person.Planet = planet; 
                                }
                            }
                        }

                        personViewModel.PeopleList.Add(person);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        private async Task FetchPlanets()
        {
            for (int i = 1; i < 61; i++)
            {
                try
                {
                    var planet = await _httpClient.GetFromJsonAsync<Planet>(_urlPlanets + i);

                    if (planet != null)
                    {
                        // Associo l'ID del pianeta al campo Id dell'oggetto
                        planet.Id = i; 
                        personViewModel.PlanetsList.Add(planet);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        private async Task FetchVehicles()
        {
            for (int i = 1; i < 74; i++)
            {
                try
                {
                    var vehicle = await _httpClient.GetFromJsonAsync<Vehicle>(_urlVehicles + i);

                    if (vehicle != null)
                    {
                        // Associo l'ID del pianeta al campo Id dell'oggetto
                        vehicle.Id = i;
                        personViewModel.VehiclesList.Add(vehicle);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async Task FetchStarships()
        {
            for (int i = 1; i < 44; i++)
            {
                try
                {
                    var starship = await _httpClient.GetFromJsonAsync<Starship>(_urlStarships + i);

                    if (starship != null)
                    {
                        // Associo l'ID del pianeta al campo Id dell'oggetto
                        starship.Id = i;
                        personViewModel.StarshipList.Add(starship);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        private async void InfoPerson(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Person selectedPerson)
            {
                var dialog = new ContentDialog
                {
                    Title = selectedPerson.Name,
                    Content = new TextBlock
                    {
                        Text = "Dati sul PIANETA:\n" +
                               $"Name: {selectedPerson.Planet?.Name ?? "Unknown"}\n" +
                               $"Gravity: {selectedPerson.Planet?.Gravity ?? "Unknown"}\n" +
                               $"Terrain: {selectedPerson.Planet?.Terrain ?? "Unknown"}\n" +
                               $"Surface Water: {selectedPerson.Planet?.SurfaceWater ?? "Unknown"}\n" +
                               $"Population: {selectedPerson.Planet?.Population ?? "Unknown"}\n",
                        TextWrapping = TextWrapping.Wrap
                    },
                    CloseButtonText = "Close"
                };

                await dialog.ShowAsync();
            }
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExportPage));
        }
    }


}
