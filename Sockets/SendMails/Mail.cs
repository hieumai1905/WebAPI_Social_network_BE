using System.Text;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Web_Social_network_BE.Sockets.SendMails;

public class Mail
{
    static string CREDENTIALS_PATH = "bin/Debug/net6.0/credentials.json";
    static string SUBJECT = "😃 Your verification code!";

    private static string CreateContent(string user, string content, string code)
    {
        string path = "Sockets/SendMails/Content/";
        string header = File.ReadAllText(path + "Head.html");
        string head_body = File.ReadAllText(
            path + "Body.html");
        string conent_code = File.ReadAllText(
            path + "Center.html");
        string footer = File.ReadAllText(
            path + "Foot.html");
        return header + user + head_body + content + conent_code + code + footer;
    }

    private static Message CreateMessage(string sendTo, string code, string name, string content)
    {
        var message = new Message();
        string encodedSubject = "=?UTF-8?B?" + Convert.ToBase64String(Encoding.UTF8.GetBytes(SUBJECT)) + "?=";
        message.Raw = Base64UrlEncode("To: " + sendTo + "\r\n" +
                                      "Subject: " + encodedSubject + "\r\n" +
                                      "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                                      CreateContent(name, content, code)
        );
        return message;
    }

    public static void SendMail(string sendTo, string content, string code, string name)
    {
        var service = AuthenticateService();
        var result = SendMessage(service, "me", CreateMessage(sendTo, code, name, content));
        Console.WriteLine("Message Id: " + result.Id);
    }

    [Obsolete("Obsolete")]
    static GmailService AuthenticateService()
    {
        UserCredential credential;

        using (var stream = new FileStream(CREDENTIALS_PATH, FileMode.Open, FileAccess.Read))
        {
            string credPath = "bin/Debug/net6.0/token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] { GmailService.Scope.GmailSend },
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        var service = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Send code for user"
        });

        return service;
    }

    static Message SendMessage(GmailService service, string userId, Message message)
    {
        try
        {
            var request = service.Users.Messages.Send(message, userId);
            return request.Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred: " + ex.Message);
            return null!;
        }
    }

    static string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(inputBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
    }
}