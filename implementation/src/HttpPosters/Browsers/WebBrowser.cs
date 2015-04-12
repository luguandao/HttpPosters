using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpPosters.Browsers
{
    public abstract class WebBrowser
    {
        public abstract string UserAgent { get; }
        public abstract string Accept { get; }
        public abstract string AcceptLanguage { get; }
        public abstract string AcceptEncoding { get; }

        public WebHeaderCollection CreateHeaderCollection()
        {
            WebHeaderCollection header = new WebHeaderCollection();

            if (!string.IsNullOrWhiteSpace(UserAgent))
                header.Add("User-Agent", UserAgent);

            if(!string.IsNullOrWhiteSpace(Accept))
                header.Add("Accept",Accept);

            if(!string.IsNullOrWhiteSpace(AcceptLanguage))
                header.Add("Accept-Language",AcceptLanguage);

            if(!string.IsNullOrWhiteSpace(AcceptEncoding))
                header.Add("Accept-Encoding",AcceptEncoding);

            return header;
        }
    }
}
