1. O usuário informa o endereço de destino e escolhe o tipo de entrega (normal ou expressa).
   
2. O sistema utiliza a API Nominatim (OpenStreetMap) para converter os endereços da empresa e do cliente em coordenadas geográficas (latitude e longitude).

3. As coordenadas são enviadas para a API OSRM, responsável pelo cálculo de rotas.

4. A API retorna a distância e o tempo estimado de deslocamento entre os dois pontos.
   
5. O sistema converte os dados retornados para quilômetros, horas e minutos.
   
6. O valor do frete é calculado com base na distância percorrida e na taxa fixa do tipo de entrega selecionado.
   
7. Por fim, são exibidos ao usuário:
Distância da rota;
Tempo estimado de viagem;
Valor final do frete.


Tecnologias utilizadas:
    C#
    .NET
    HttpClient
    API Nominatim (OpenStreetMap)
    API OSRM
    JSON (System.Text.Json
