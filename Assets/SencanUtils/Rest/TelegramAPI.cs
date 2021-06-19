using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SencanUtils.Rest
{
    public static class TelegramAPI
    {
        private const string ChatID = "666070988";
        private const string Token = "1740001391:AAHZCJ7dyp6oKzWK9CgYKlEo1hY4ZcOzPTU";
    
        public static string API_URL => $"https://api.telegram.org/bot{Token}/";

        public static void GetMe()
        {
            WWWForm form = new WWWForm();
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "getMe", form);
            SendRequest(www);
        }

        public static void SendFile(byte[] bytes, string filename, string caption = "")
        {
            WWWForm form = new WWWForm();
            form.AddField("chat_id", ChatID);
            form.AddField("caption", caption);
            form.AddBinaryData("document", bytes, filename, filename);
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "sendDocument?", form);
            SendRequest(www);
        }

        public static void SendPhoto(byte[] bytes, string filename, string caption = "")
        {
            WWWForm form = new WWWForm();
            form.AddField("chat_id", ChatID);
            form.AddField("caption", caption);
            form.AddBinaryData("photo", bytes, filename, filename);
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "sendPhoto?", form);
            SendRequest(www);
        }

        public static void SendMessage(string text)
        {
            WWWForm form = new WWWForm();
            form.AddField("chat_id", ChatID);
            form.AddField("text", text);
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "sendMessage?", form);
            SendRequest(www);
        }

        private static async void SendRequest(UnityWebRequest www)
        {
            await www.SendWebRequest();
        }
    }
}