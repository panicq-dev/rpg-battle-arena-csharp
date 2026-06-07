using System.Text;
using System.Text.Json;

namespace RPGBattleArena.Classes;

internal static class NarradorIA
{
    private static readonly HttpClient _http = new();

    private static string ApiKey => Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "";

    private const string ApiUrl =
        "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    public static async Task<string> NarrarAsync(string evento, string contexto = "")
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            Display.Escrever("[NARRADOR] GEMINI_API_KEY não encontrada.", ConsoleColor.DarkGray);
            return string.Empty;
        }

        string instrucao = "Você é um narrador épico de RPG medieval. " +
                           "Narre o evento em português de forma dramática e concisa (máximo 2 frases). " +
                           "Use linguagem vívida e imersiva. Não repita números já exibidos na tela.";

        string prompt = string.IsNullOrEmpty(contexto)
            ? $"{instrucao}\n\nEvento: {evento}"
            : $"{instrucao}\n\nContexto: {contexto}\nEvento: {evento}";

        try
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    maxOutputTokens = 150,
                    temperature = 0.9
                }
            };

            string url = $"{ApiUrl}?key={ApiKey}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Display.Escrever($"[NARRADOR] Erro HTTP {(int)response.StatusCode}: {json}", ConsoleColor.DarkRed);
                return string.Empty;
            }

            using var doc = JsonDocument.Parse(json);

            var texto = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;

            return texto.Trim();
        }
        catch (Exception ex)
        {
            Display.Escrever($"[NARRADOR] Exceção: {ex.Message}", ConsoleColor.DarkRed);
            return string.Empty;
        }
    }

    public static string Narrar(string evento, string contexto = "")
    {
        try
        {
            return NarrarAsync(evento, contexto).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Display.Escrever($"[NARRADOR] Exceção síncrona: {ex.Message}", ConsoleColor.DarkRed);
            return string.Empty;
        }
    }

    public static void ExibirNarracao(string narracao)
    {
        if (string.IsNullOrWhiteSpace(narracao)) return;

        Console.WriteLine();
        Display.Escrever("📖 " + narracao, ConsoleColor.Magenta);
        Console.WriteLine();
    }
}