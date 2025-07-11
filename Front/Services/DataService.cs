using CommonDto.Models;

namespace Front.Services;

public class DataService(HttpClient httpClient) : IDataService
{
    
    public async Task<List<DogDto>?> GetAllDogs()
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync("Referentiel/Dogs");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            List<DogDto>? dogs = System.Text.Json.JsonSerializer.Deserialize<List<DogDto>>(responseBody);
            return dogs;
        }
        catch (HttpRequestException ex)
        {
            // Gérer les erreurs HTTP (par exemple, fichier non trouvé, problème de réseau)
            Console.WriteLine($"Erreur HTTP lors de la récupération des formations : {ex.Message}");
            return null; // Retourne une liste vide en cas d'erreur
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Gérer les erreurs de désérialisation JSON (par exemple, format JSON invalide)
            Console.WriteLine($"Erreur de désérialisation JSON : {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            // Gérer les autres exceptions imprévues
            Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
            return null;
        }
    }
    
    
}