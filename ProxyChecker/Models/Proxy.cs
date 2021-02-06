namespace ProxyChecker.Models
{
    public class Proxy
    {
        public Proxy(int id, string ip, int ping, string status)
        {
            Id = id;
            Ip = ip;
            Ping = ping;
            Status = status;
        }

        public int Id { get; set; }
        public string Ip { get; set; }
        public int Ping { get; set; }
        public string Status { get; set; }
    }
}