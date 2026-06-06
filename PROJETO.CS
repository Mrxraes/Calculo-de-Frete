using System;
using System.Collections.Generic;
using System.Net.Http;
using CalculoFrete;
using System.Threading.Tasks;
using System.Text.Json;

namespace CalculoFrete
{
    class LatLon
    {
        static public async Task latLon(string enderecoCompleto, string tipoEntrega)
        {
            /* aqui tem uma fornmatação do texto, para ser enviado como a url requer 
            --> URI converte espaçoes e caracteres especiais em formato valido para o link
            --> q= é o texto que vc quer ver
            --> format pede o resultado em json */

            string enderecoA = "SP, São Paulo, Bela Vista, Avenida Paulista, 1000";
            string enderecoB = enderecoCompleto;

            string? urlA = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(enderecoA)}&format=jsonv2"; 
            string? urlB = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(enderecoB)}&format=jsonv2"; 

            
            var comunicar = new HttpClient(); //http client conversa com as apis
            comunicar.DefaultRequestHeaders.Add(
                "User-Agent",
                "API/PROJETO.CS" ); //usuario faz a requisição no projeto

            string? jsonA = await comunicar.GetStringAsync(urlA); // tranforma o json em string da forma correta 
            string? jsonB = await comunicar.GetStringAsync(urlB); // tranforma o json em string da forma correta 

            var organizerA = JsonDocument.Parse(jsonA); // organiza o json
            var organizerB = JsonDocument.Parse(jsonB); // organiza o json


            string? latA = 
                organizerA.RootElement[0]
                .GetProperty("lat")
                .GetString();

            string? lonA = 
                organizerA.RootElement[0]
                .GetProperty("lon")
                .GetString();

            string? latB = 
                organizerB.RootElement[0]
                .GetProperty("lat")
                .GetString();

            string? lonB = 
                organizerB.RootElement[0]
                .GetProperty("lon")
                .GetString();

            await APIrota.rotas(latA, lonA, latB, lonB, tipoEntrega);
            
        }
    }
}

namespace CalculoFrete
{
    class APIrota
    {
        static public async Task rotas(string? latA, string? lonA, string? latB, string? lonB, string tipoEntrega)
        {
            string? url = $"http://router.project-osrm.org/route/v1/driving/{lonA},{latA};{lonB},{latB}?overview=false";
            var comunicar = new HttpClient();
            comunicar.DefaultRequestHeaders.Add(
                "User-Agent",
                "API/PROJETO.CS"
            );
            string? json = await comunicar.GetStringAsync(url);
            

            var organizer = JsonDocument.Parse(json);

            double? duracao =
                organizer.RootElement
                .GetProperty("routes")[0]
                .GetProperty("duration")
                .GetDouble();
                
            double? distancia = 
                organizer.RootElement
                .GetProperty("routes")[0]
                .GetProperty("distance")
                .GetDouble();



            int horas = (int)duracao / 3600;
            double? min = duracao / 3600 * 60;;
            int km = (int)distancia / 1000;

            Console.WriteLine($"DISTÂNCIA: {km} Km.");
            Console.WriteLine($"DURAÇÃO: {horas} Horas e {(int)min} Minutos");

            double calculoFrete(double tipo)
                {
                    double valorFrete = 0.65 * km + tipo; 
                    Console.WriteLine($"FRETE: R$ {valorFrete}");
                    return valorFrete;
                    
                } 

            if (tipoEntrega.ToLower() == "normal")
            {
                calculoFrete(7.00);
                
            } else if (tipoEntrega.ToLower() == "expressa")
            {
                calculoFrete(10.00);
            } else
            {
                Console.WriteLine("Tipo de frete não reconhecido...");
            }

        }
    }
}

namespace CalculoFrete
{
    class Principal
{

    // o metodo precisa ser asynt para que a conversao do json em texto de certo
    static async Task Main(string[] args)
    {
        Console.Write("Digite a sigla do seu estado: ");
        string? estado = Console.ReadLine();
        Console.Write("Digite a sua cidade: ");
        string? cidade = Console.ReadLine();
        Console.Write("Digite o seu bairro: ");
        string? bairro = Console.ReadLine();
        Console.Write("Digite a sua rua e número de residência: ");
        string? rua = Console.ReadLine();

        string enderecoCompleto = $"{estado}, {cidade}, {bairro}, {rua}";

        Console.WriteLine("ENTREGA NORMAL: R$ 7,00");
        Console.WriteLine("ENTREGA EXPRESSA: R$ 10,00");
        Console.Write("Qual será o tipo de entrega? ");
        string? tipoEscolhido = Console.ReadLine();

        await LatLon.latLon(enderecoCompleto, tipoEscolhido);
    }
}
}
