using System.Net.Http.Headers;
using System.Text;

public static class Discord
{
    public static async void WebhookAsync(string username, string avatar, string webhookUrl, string highlight)
    {
        try
        {
            Logger.LogMessage($"Sharing Highlight on Discord..");
            
            using (var httpClient = new HttpClient())
            {     
                var jsonContent = $"{{ \"content\": null, \"embeds\": null, \"username\": \"{username}\", \"avatar_url\": \"{avatar}\" }}";
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var form = new MultipartFormDataContent())
                {
                    form.Add(stringContent, "payload_json");

                    using (var fileContent = new StreamContent(File.OpenRead(highlight)))
                    {
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
                        form.Add(fileContent, "file", Path.GetFileName(highlight));

                        var response = await httpClient.PostAsync(webhookUrl, form);
                        if (response.IsSuccessStatusCode)
                        {
                            Logger.LogMessage($"Highlight successfully shared.");
                        }
                        else
                        {
                            Logger.LogError($"Could not share Highlight: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                        }
                    }
                }
            }
        }
        catch(Exception e)
        {
            Logger.LogError($"WebhookAsync exception: {e.Message}");
        }
    }
}