using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CommonDto.Models;

namespace Front.Services;

public class DataService(HttpClient httpClient) : IDataService
{

    public async Task<Guid> AddNewCrop(CropDto crop)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                WriteIndented = true
            };
            string jsonPayload = JsonSerializer.Serialize(crop,options);
            var content = new StringContent(jsonPayload);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response;
            
            if (crop.Id == null)
            {
                response = await httpClient.PostAsync("Crops",  content);
            }
            else
            {
                response = await httpClient.PutAsync($"Crops/{crop.Id}",  content);
            }
            
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();

            return Guid.Parse(responseJson);
        }
        catch (HttpRequestException ex)
        {
            // Gérer les erreurs HTTP (par exemple, fichier non trouvé, problème de réseau)
            Console.WriteLine($"Erreur HTTP lors de la récupération des formations : {ex.Message}");
            return Guid.Empty;
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Gérer les erreurs de désérialisation JSON (par exemple, format JSON invalide)
            Console.WriteLine($"Erreur de désérialisation JSON : {ex.Message}");
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            // Gérer les autres exceptions imprévues
            Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
            return Guid.Empty;
        }
    }
    
    public async Task<List<CropDto>?> GetAllCrops()
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync("Crops");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            List<CropDto>? crops = System.Text.Json.JsonSerializer.Deserialize<List<CropDto>>(responseBody);
            return crops;
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
    
    
    
    public async Task<Guid> ModifySeed(SeedDto seed)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                WriteIndented = true
            };
            string jsonPayload = JsonSerializer.Serialize(seed,options);
            var content = new StringContent(jsonPayload);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response;
            
            if (seed.Id == null)
            {
                response = await httpClient.PostAsync("Seeds",  content);
            }
            else
            {
                response = await httpClient.PutAsync($"Seeds/{seed.Id}",  content);
            }
            
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();

            return Guid.Parse(responseJson);
        }
        catch (HttpRequestException ex)
        {
            // Gérer les erreurs HTTP (par exemple, fichier non trouvé, problème de réseau)
            Console.WriteLine($"Erreur HTTP lors de la récupération des formations : {ex.Message}");
            return Guid.Empty;
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Gérer les erreurs de désérialisation JSON (par exemple, format JSON invalide)
            Console.WriteLine($"Erreur de désérialisation JSON : {ex.Message}");
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            // Gérer les autres exceptions imprévues
            Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
            return Guid.Empty;
        }
    }
    
    public async Task DeleteSeed(Guid? seedId)
    {
        try
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"Seeds/{seedId}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            // Gérer les erreurs HTTP (par exemple, fichier non trouvé, problème de réseau)
            Console.WriteLine($"Erreur HTTP lors de la récupération des formations : {ex.Message}");
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Gérer les erreurs de désérialisation JSON (par exemple, format JSON invalide)
            Console.WriteLine($"Erreur de désérialisation JSON : {ex.Message}");
        }
        catch (Exception ex)
        {
            // Gérer les autres exceptions imprévues
            Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
        }
    }
   
    public async Task<List<SeedDto>?> GetAllSeeds()
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync("Seeds");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            List<SeedDto>? seeds = System.Text.Json.JsonSerializer.Deserialize<List<SeedDto>>(responseBody);
            return seeds;
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

    public async Task<SeedDto?> GetSeedById(Guid? seedId)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"Seeds/{seedId}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var seed = System.Text.Json.JsonSerializer.Deserialize<SeedDto>(responseBody);
            return seed;
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