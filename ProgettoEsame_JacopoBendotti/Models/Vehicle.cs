using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgettoEsame_JacopoBendotti.Models
{
    internal class Vehicle
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("model")]
        public String Model { get; set; }

        [JsonPropertyName("manufacturer")]
        public String Manufacturer { get; set; }

        [JsonPropertyName("cost_in_credits")]
        public string CostInCredits { get; set; }

        [JsonPropertyName("length")]
        public string Length { get; set; }

        [JsonPropertyName("max_atmosphering_speed")]
        public string MaxAtmospheringSpeed { get; set; }

        [JsonPropertyName("crew")]
        public string Crew { get; set; }

        [JsonPropertyName("passengers")]
        public String Passengers { get; set; }

        [JsonPropertyName("cargo_capacity")]
        public string CargoCapacity { get; set; }

        [JsonPropertyName("consumables")]
        public string Consumables { get; set; }

        [JsonPropertyName("vehicle_class")]
        public String VehicleClass { get; set; }

        [JsonPropertyName("pilots")]
        public string Pilots { get; set; }

        [JsonPropertyName("films")]
        public String Films { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("edited")]
        public string Edited { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }


        public int Id { get; set; } //Id è per identificare il pianeta

    }
}
