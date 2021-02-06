using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProxyChecker.Models
{
    internal class Http
    {
        private readonly HttpClient HttpClient;
        private readonly HttpClientHandler HttpClientHandler;
        private string ip;
        public int LastPing;
        private readonly string target;

        public Http(int timeout, string ip, string target)
        {
            HttpClientHandler = new HttpClientHandler();
            HttpClient = new HttpClient(HttpClientHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };
            SetIp(ip);
            SetWebProxy();
            this.target = target;
        }

        public void SetIp(string ip)
        {
            this.ip = ip;
        }

        public void SetWebProxy()
        {
            var webProxy = new WebProxy
            {
                Address = new Uri("http://" + ip)
            };
            HttpClientHandler.Proxy = webProxy;
            HttpClientHandler.UseProxy = true;
        }

        public async Task<string> Get()
        {
            try
            {
                var st = new Stopwatch();
                st.Start();
                var result = await HttpClient.GetAsync(target);
                var lastResponse = await result.Content.ReadAsStringAsync();
                st.Stop();
                LastPing = (int) st.ElapsedMilliseconds;
                return lastResponse.Length > 1 ? "Ok" : "Error";
            }
            catch (TaskCanceledException e)
            {
                return "Timeout";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }
    }
}