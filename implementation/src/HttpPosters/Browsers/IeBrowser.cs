using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpPosters.Browsers
{
    public class IeBrowser : WebBrowser
    {
        public override string UserAgent => "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko";

        public override string Accept => "text/html, application/xhtml+xml, */*";

        public override string AcceptLanguage => "zh-Hans-CN,zh-Hans;q=0.8,en-US;q=0.5,en;q=0.3";

        public override string AcceptEncoding => "gzip, deflate";
       
    }
}
