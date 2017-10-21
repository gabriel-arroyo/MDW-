using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace MDW_Print.Connectivity
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }
        public string token { get; set; }
        public string contentType { get; set; }
        public string postData { get; set; }

        public RestClient(httpVerb _method, string _endPoint, string _contentType, string _token = null)
        {
            httpMethod = _method;
            endPoint = _endPoint;
            contentType = _contentType;
            token = _token;
            postData = null;
        }
        public RestClient(httpVerb _method, string _endPoint, string _postData, string _contentType, string _token = null)
        {
            httpMethod = _method;
            endPoint = _endPoint;
            contentType = _contentType;
            token = _token;
            postData = _postData;
        }

        internal static string getId()
        {
            string line = "";
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (StreamReader file = new StreamReader(path + "\\id.txt"))
                line = file.ReadLine() ?? "";
            return line;
        }

        internal static string getToken()
        {
            string line = "";
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (StreamReader file = new StreamReader(path + "\\token.txt"))
                line = file.ReadLine() ?? "";
            return line;
        }
    

        public string makeRequest()
        {
            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint + postData ?? "");

            request.Method = httpMethod.ToString();
            request.ContentType = contentType;
            if (token != null)
            {
                request.Headers.Add("Authorization", "Bearer " + token);
            }
            if (httpMethod == httpVerb.POST)
            {
                postData = postData ?? "";
                var data = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ApplicationException("error code:" + response.StatusCode);

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }

            return strResponseValue;
        }

        public T Deserialize<T>(string json)
        {
            T r = JsonConvert.DeserializeObject<T>(json);
            return r;
        }


    }
}
