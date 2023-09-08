using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Clear(); 
        Console.WriteLine("Presiona una tecla para comenzar el juego...");
        Console.ReadKey();

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://pokeapi.co/api/v2/pokemon?limit=100000&offset=0")
        };

        using (var response = client.SendAsync(request).Result)
        {
            response.EnsureSuccessStatusCode();
            string body = response.Content.ReadAsStringAsync().Result;
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(body);

            JArray resultsArray = (JArray)jsonObject["results"];

            List<string> nombresPokemon = resultsArray.Select(result => (string)result["name"]).ToList();

            
            Random random = new Random();
            string palabraSecreta = nombresPokemon[random.Next(nombresPokemon.Count)];
            int intentosMaximos = 6;
            int intentosRestantes = intentosMaximos;
            char[] palabraAdivinada = new char[palabraSecreta.Length];

            
            for (int i = 0; i < palabraAdivinada.Length; i++)
            {
                palabraAdivinada[i] = '_';
            }

            Console.Clear(); 
            Console.WriteLine("¡Bienvenido al juego de ahorcado!");
            Console.WriteLine($"Tienes {intentosRestantes} intentos para adivinar la palabra.");

            while (intentosRestantes > 0)
            {
                Console.WriteLine($"Palabra a adivinar: {new string(palabraAdivinada)}");
                Console.Write("Ingresa una letra: ");
                char letra = Console.ReadKey().KeyChar;
                Console.WriteLine(); 

                bool letraAdivinada = false;

                for (int i = 0; i < palabraSecreta.Length; i++)
                {
                    if (palabraSecreta[i] == letra)
                    {
                        palabraAdivinada[i] = letra;
                        letraAdivinada = true;
                    }
                }

                if (!letraAdivinada)
                {
                    intentosRestantes--;
                    Console.WriteLine($"Letra incorrecta. Te quedan {intentosRestantes} intentos.");
                }

                
                if (new string(palabraAdivinada) == palabraSecreta)
                {
                    Console.WriteLine("¡Felicidades! Has adivinado la palabra.");
                    break;
                }
            }

            if (intentosRestantes == 0)
            {
                Console.WriteLine($"¡Agotaste tus intentos! La palabra secreta era: {palabraSecreta}");
            }

            Console.WriteLine("Presiona una tecla para salir...");
            Console.ReadKey(); 
        }
    }
}

