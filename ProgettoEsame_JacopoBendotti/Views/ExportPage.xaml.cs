using ProgettoEsame_JacopoBendotti.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using ProgettoEsame_JacopoBendotti.ViewModels;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Windows.UI.ViewManagement;
using System.Text;


namespace ProgettoEsame_JacopoBendotti.Views
{

    internal sealed partial class ExportPage : Page
    {
        private string _selectedFormat;  


        public ExportPage()
        {
            this.InitializeComponent();
        }



        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (JsonRadioButton.IsChecked == true)
            {
                _selectedFormat = "JSON";  
            }
            else if (XmlRadioButton.IsChecked == true)
            {
                _selectedFormat = "XML";  
            }

            // Abilita "Save File" se il campo fileName non è vuoto e un formato è selezionato
            SaveFileButton.IsEnabled = !string.IsNullOrEmpty(fileName.Text) && !string.IsNullOrEmpty(_selectedFormat);
        }

        private void checkName(object sender, RoutedEventArgs routedEventArgs)
        {
            SaveFileButton.IsEnabled = !string.IsNullOrEmpty(fileName.Text) && !string.IsNullOrEmpty(_selectedFormat);
        }


        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            if (_selectedFormat == "JSON")
            {
                SaveAsJsonAsync();
            }
            else if (_selectedFormat == "XML")
            {
                SaveAsXmlAsync();
            }
        }


        private async Task SaveAsJsonAsync()
        {
            string fileNameText = fileName.Text; 

            // Verifica se il file esiste 
            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync($"{fileNameText}.json");

            //se file != da null il file esiste
            if (file != null)
            {
                var existingJson = await FileIO.ReadTextAsync((StorageFile)file);

                // Deserializziamo il JSON esistente in una lista di oggetti Person
                var existingPeopleList = JsonSerializer.Deserialize<ObservableCollection<Person>>(existingJson);

                // Aggiungiamo i nuovi dati alla lista esistente
                foreach (var person in PersonViewModel.PeopleListSelected)
                {
                    existingPeopleList.Add(person);
                }

                // Serializziamo di nuovo la lista aggiornata in JSON
                var updatedJson = JsonSerializer.Serialize(existingPeopleList);

                // Sovrascriviamo il file con i nuovi dati
                await FileIO.WriteTextAsync((StorageFile)file, updatedJson);

                Debug.WriteLine("File JSON aggiornato in: " + file.Path);
            }
            else
            {
                var newJson = JsonSerializer.Serialize(PersonViewModel.PeopleListSelected);
                var newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync($"{fileNameText}.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(newFile, newJson);

                Debug.WriteLine("File JSON creato in: " + newFile.Path);
            }
        }



        private async Task SaveAsXmlAsync()
        {
            string fileNameText = fileName.Text;

            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync($"{fileNameText}.xml");

            if (file != null)
            {
                var existingXml = await FileIO.ReadTextAsync((StorageFile)file);
                var xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Person>));

                // Usando un MemoryStream, deserializziamo il contenuto esistente
                using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(existingXml));
                var existingPeopleList = (ObservableCollection<Person>)xmlSerializer.Deserialize(memoryStream);

                // Aggiungiamo i nuovi dati alla lista esistente
                foreach (var person in PersonViewModel.PeopleListSelected)
                {
                    existingPeopleList.Add(person);
                }

                var fileToWrite = (StorageFile)file;  // Converte IStorageFile in StorageFile
                using (var stream = await fileToWrite.OpenAsync(FileAccessMode.ReadWrite))
                {
                    // Serializziamo la lista aggiornata in XML
                    xmlSerializer.Serialize(stream.AsStreamForWrite(), existingPeopleList);
                }

                Debug.WriteLine("File XML aggiornato in: " + file.Path);
            }
            else
            {
                try
                {
                    var newXml = new XmlSerializer(typeof(ObservableCollection<Person>));
                    var newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync($"{fileNameText}.xml",
                        CreationCollisionOption.ReplaceExisting);

                    // Serializziamo direttamente i nuovi dati in XML
                    using (var stream = await newFile.OpenStreamForWriteAsync())
                    {
                        newXml.Serialize(stream, PersonViewModel.PeopleListSelected);
                    }

                    Debug.WriteLine("File XML creato in: " + newFile.Path);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("PROBLEMI PROBLEMI : " +e);

                }
            }
        }


        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            PersonViewModel.PeopleListSelected.Clear();
            Frame.Navigate(typeof(HomePage));
        }
    }
}
