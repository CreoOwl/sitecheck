using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using Interface;

namespace SiteCheck
{
    public class SiteCheck : ICheck
    {
        public string CheckSite(string site)
        {
            if (Regex.IsMatch(site, @"http://\w+\.\w+", RegexOptions.IgnoreCase))
                return site;
            else if (Regex.IsMatch(site, @"www\.\w+\.\w+", RegexOptions.IgnoreCase))
            {
                return site.Replace("www.", @"http://");
            }
            else if (Regex.IsMatch(site, @"\w+\.\w+", RegexOptions.IgnoreCase))
            {
                return site.Insert(0, @"http://");
            }
            else
            {
                Console.WriteLine("Неверный адрес сайта.");
                return null;
            }
        }

        public bool Check(string site)
        {
            site = CheckSite(site);
            if(site != null)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(site);
                request.Method = "HEAD";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        return true;
                    }
                }
                catch (WebException)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
