using System.Text.Json;

namespace FrontEndMovile.Util
{
    public static   class UtilCodeExtension
    {
        public static async Task<Dictionary<string, List<string>>> GetErrorDictionaryAsync(this HttpContent response)
        {
            var errorMessage = await response.ReadAsStringAsync();

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errorMessage);

            return dictionary == null ? new Dictionary<string, List<string>>():dictionary ;
        }
    }
}
