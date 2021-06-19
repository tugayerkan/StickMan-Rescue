using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;

namespace SencanUtils.Rest
{
    public static class APIHelper
    {
        public static T GetData<T>(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
                var stream = response.GetResponseStream();
                if (stream == null || stream == Stream.Null)
                    return default;
                StreamReader reader = new StreamReader(stream);
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return default;
            }
        }
    }
}
