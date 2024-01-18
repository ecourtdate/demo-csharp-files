using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;


class Program
{

    static async Task Main(string[] args)
    {
        string url = GetRequiredEnvironmentVariable("API_URL"); // API URL

        url += "/oauth/token"; // Token URL

        // Step 1: Get the Access Token
        string accessToken = await GetAccessToken(url);
        if (string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("Failed to obtain access token.");
            return;
        }

        // Step 2: Upload the File using the Access Token
        string fileResponse = await UploadFile(accessToken);

        if (string.IsNullOrEmpty(fileResponse))
        {
            Console.WriteLine("Failed to upload file.");
            return;
        }

        // Step 3: Send the Message using the Access Token and File Response
        await SendMessage(accessToken, fileResponse);
    }

    static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Environment variable {variableName} is not set.");
        }
        return value;
    }

    static async Task<string> GetAccessToken(string tokenUrl)
    {
        // Replace these with the appropriate values
        var requestData = new
        {
            client_id = GetRequiredEnvironmentVariable("API_CLIENT"),
            client_secret = GetRequiredEnvironmentVariable("API_SECRET"),
            grant_type = "client_credentials"
        };

        var jsonRequest = JsonConvert.SerializeObject(requestData);
        using var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        using var client = new HttpClient();

        try
        {
            var response = await client.PostAsync(tokenUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // Extract the access token from the response
                var responseToken = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (responseToken != null && responseToken.ContainsKey("access_token"))
                {
                    return responseToken["access_token"];
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        return "";
    }


    static async Task<string> UploadFile(string accessToken)
    {
        // Replace this with the path to the file to be uploaded
        string filePath = "test.pdf";

        // Encode the file to Base64
        string base64EncodedFile = Convert.ToBase64String(File.ReadAllBytes(filePath));

        // Prepare the JSON payload
        var payload = new
        {
            file_data = base64EncodedFile,
            name = "test.pdf",
            type = "application/pdf",
            size = new FileInfo(filePath).Length,
            attach_email = true
        };

        var jsonPayload = JsonConvert.SerializeObject(payload);

        // API endpoint to which the file is to be uploaded
        string url = GetRequiredEnvironmentVariable("API_URL");; // API URL

        url += "/v1/files"; // File Upload URL

        using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            // Send POST request
            var response = await client.PostAsync(url, content);

            // Check if the request was successful
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return "";
            }
            else
            {
                
                var json = await response.Content.ReadAsStringAsync();

                var responseToken = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (responseToken != null)
                {
                    return responseToken["uuid"] ?? "";
                }

                return "";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        return "";
    }

    static async Task SendMessage(string accessToken, string fileResponse)
    {
        // Prepare the JSON payload
        var payload = new
        {
            to = "demo@ecourtdate.com",
            content = "Message from [AgencyName] on [DateToday] at [TimeNow] using the eCourtDate API.",
            file = fileResponse,
            mms = 1
        };

        var jsonPayload = JsonConvert.SerializeObject(payload);

        // API endpoint to send a one-off message
        string url = GetRequiredEnvironmentVariable("API_URL"); // API URL

        url += "/v1/messages/oneoffs"; // One-off Message URL

        using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            // Send POST request
            var response = await client.PostAsync(url, content);

            // Check if the request was successful
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            else
            {
                // Read and display the response
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}
