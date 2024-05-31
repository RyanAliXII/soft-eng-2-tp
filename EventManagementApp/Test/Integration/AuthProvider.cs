

using System.Text;
using System.Text.Json;
using EventManagementApp.Areas.Admin.ViewModels;

class AuthProvider
{
    public static async Task<HttpResponseMessage> SignIn(HttpClient client, LoginViewModel data)
    {
        var tokenManager = new AntiforgeryTokenProvider(client);
        var token = await tokenManager.GetTokenFromPageAsync("/admin/login") ?? throw new Exception("Token is null");
        var form = new Dictionary<string, string>();
        form.Add("email", data.Email);
        form.Add("password", data.Password);
        client.DefaultRequestHeaders.Add("RequestVerificationToken", token);
        var response = await client.PostAsync("/Admin/Login", new FormUrlEncodedContent(form));
        return response;
    }
}