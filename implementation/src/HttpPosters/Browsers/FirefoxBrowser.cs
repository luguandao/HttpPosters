using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpPosters.Browsers
{
    public class FirefoxBrowser : WebBrowser
    {
        public FirefoxVersion Version { get; set; }

        public override string Accept => "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

        public override string AcceptEncoding => "gzip, deflate";

        public override string AcceptLanguage => "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3";

        public override string UserAgent => "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0";
    }

    public enum FirefoxVersion
    {
        Version36
    }
}
