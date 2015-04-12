using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HttpPosters.Browsers;

namespace HttpPosters
{
    public class HttpPoster
    {
        public string ContextId { get; set; }

        public WebHeaderCollection Headers { get; set; }

        public HttpRequestMethod Method { get; set; }

        public CookieContainer Cookies { get; set; }

        public WebBrowser Browser { get; set; }

        public bool KeepAlive { get; set; }

        public string Url { get; set; }
        public IList<KeyValuePair<string, string>> ParamsValuePairs { get; set; }

        public HttpPostResponse PostedResponse { get; private set; }

        public string ContentType
        {
            get
            {
                switch (Method)
                {
                    case HttpRequestMethod.Post:
                        return "application/x-www-form-urlencoded";

                }
                return null;
            }
        }

        public HttpPoster()
        {
            ContextId = Guid.NewGuid().ToString();
            Headers = new WebHeaderCollection();
            Cookies = new CookieContainer();
            Method = HttpRequestMethod.Get;
        }

        public HttpPoster(HttpPoster prevVisitor) : this()
        {
            if (prevVisitor != null)
            {
                ContextId = prevVisitor.ContextId;
                Headers = prevVisitor.Headers;
                Cookies = prevVisitor.Cookies;


            }

        }

        #region event

        public event EventHandler<PostingEventArgs> Posting;
        public event EventHandler<PostedEventArgs> Posted;
        public event EventHandler<ErrorEventArgs> ExceptionOccurred;
        #endregion

        protected void OnPosting(string url)
        {
            Posting?.Invoke(this, new PostingEventArgs(url));
        }

        protected void OnPosted(HttpWebRequest request, HttpWebResponse response)
        {
            Posted?.Invoke(this, new PostedEventArgs { Request = request, Response = response });
        }

        protected void OnExceptionOccurred(Exception ex)
        {
            ExceptionOccurred?.Invoke(this, new ErrorEventArgs(ex));
        }

        public void Post()
        {
            OnPosting(Url);
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = WebRequest.CreateHttp(Url);
                request.KeepAlive = KeepAlive;

                request.CookieContainer = Cookies;


                if (Browser != null)
                    MergeHeader(request, Browser.CreateHeaderCollection());

                MergeHeader(request, Headers);
                request.ServerCertificateValidationCallback += CustomXertificateValidation;

                request.Method = Method.ToString();
                request.ContentType = ContentType;
                if (ParamsValuePairs != null)
                {
                    byte[] postData = getDataBytes(ParamsValuePairs);
                    using (Stream sm = request.GetRequestStream())
                    {
                        sm.Write(postData, 0, postData.Length);
                    }
                }
                response = (HttpWebResponse)request.GetResponse();
                PostedResponse = new HttpPostResponse(request, response);
                if (response.Cookies != null && response.Cookies.Count > 0)
                {
                    foreach (Cookie item in response.Cookies)
                    {
                        Cookies.Add(item);
                    }
                }
            }
            catch (WebException e)
            {
                if (ExceptionOccurred != null)
                    OnExceptionOccurred(e);
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null)
                {
                    OnPosted(request, response);
                }
            }

        }

        public void AddData(string key, string value)
        {
            if (ParamsValuePairs == null)
                ParamsValuePairs = new List<KeyValuePair<string, string>>();

            ParamsValuePairs.Add(new KeyValuePair<string, string>(key, value));
        }

        private byte[] getDataBytes(IEnumerable<KeyValuePair<string, string>> paramsValuePairs)
        {
            if (paramsValuePairs == null)
                return new byte[0];

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> valuePair in paramsValuePairs)
            {
                if (valuePair.Value == null) continue;

                if (sb.Length > 0)
                    sb.Append("&");
                sb.Append(valuePair.Key);
                sb.Append("=");
                sb.Append(WebUtility.HtmlEncode(valuePair.Value));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private void MergeHeader(HttpWebRequest request, WebHeaderCollection headers)
        {
            var er = headers.GetEnumerator();
            while (er.MoveNext())
            {
                string key = er.Current.ToString();
                string value = headers[key];
                switch (key.ToLower())
                {
                    case "user-agent":
                        request.UserAgent = value;
                        break;
                    case "host":
                        request.Host = value;
                        break;
                    case "connection":
                        request.Connection = value;
                        break;
                    case "accept":
                        request.Accept = value;
                        break;
                    default:
                        request.Headers.Add(key, value);
                        break;
                }


            }
        }

        public static bool CustomXertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPoicyErrors)
        {
            return true;
            //switch (sslPoicyErrors)
            //{
            //    case System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors:
            //    case System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch:
            //    case System.Net.Security.SslPolicyErrors.RemoteCertificateNotAvailable:
            //        break;
            //}

            //   return clientCert.Verify();  // Perform the Verification and sends the result
        }
    }
}
