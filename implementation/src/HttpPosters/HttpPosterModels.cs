using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpPosters
{
    public enum HttpRequestMethod
    {
        Get,
        Post
    }

    public class HttpPostResponse
    {
        private readonly HttpWebRequest _request;
        private readonly HttpWebResponse _response;

        private string _responseBody;
        public string ResponseBody
        {
            get
            {
                if (_responseBody == null)
                {
                    var stream = _response.GetResponseStream();

                    if (stream != null)
                    {
                        if (_request.Headers[HttpRequestHeader.AcceptEncoding] == "gzip, deflate")
                        {
                            stream = new GZipStream(stream, CompressionMode.Decompress);
                        }
                        using (StreamReader fin = new StreamReader(stream, Encoding.UTF8))
                        {
                            _responseBody = fin.ReadToEnd();
                        }
                    }
                }
                return _responseBody;
            }

        }

        public HttpPostResponse(HttpWebRequest request, HttpWebResponse response)
        {
            if (request == null) throw new ArgumentNullException("request cannot be null.");
            if (response == null) throw new ArgumentNullException("response cannot be null.");
            _request = request;
            _response = response;
        }

    }

    public class PostedEventArgs : EventArgs
    {
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }

        private string _responseBody;
        public string ResponseBody
        {
            get
            {
                if (_responseBody == null)
                {
                    var stream = Response.GetResponseStream();

                    if (stream != null)
                    {
                        if (Request.Headers[HttpRequestHeader.AcceptEncoding] == "gzip, deflate")
                        {
                            stream = new GZipStream(stream, CompressionMode.Decompress);
                        }
                        using (StreamReader fin = new StreamReader(stream, Encoding.UTF8))
                        {
                            _responseBody = fin.ReadToEnd();
                        }
                    }
                }
                return _responseBody;
            }
        }
    }

    public class PostingEventArgs : EventArgs
    {
        public Uri Uri { get; set; }

        public PostingEventArgs(string url)
        {
            Uri = new Uri(url);
        }
    }
}
