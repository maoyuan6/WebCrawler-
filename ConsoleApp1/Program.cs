
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

HttpClient client = new HttpClient();
// 设置 API 密钥
string apiKey = "sk-35bi5KI1syf9RdsJEnJrf4d2HPpCKAkPix8sO1S2cfHrSrwk"; // 替换为您的 API 密钥
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey); 

Console.WriteLine("输入问题，输入 'exit' 退出。");

while (true)
{
    Console.Write("您: ");
    string userInput = Console.ReadLine();

    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    var requestBody = new
    {
        model = "gpt-3.5-turbo", // 或者使用其他模型
        messages = new[]
        {
            new { role = "user", content = userInput }
        }
    };

    var json = JsonConvert.SerializeObject(requestBody);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    try
    {
        var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
            string chatGptResponse = jsonResponse.choices[0].message.content;
            Console.WriteLine("ChatGPT: " + chatGptResponse);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"请求失败: {response.StatusCode}, 错误信息: {errorResponse}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"发生异常: {ex.Message}");
    }
}

Console.WriteLine("程序已退出。");