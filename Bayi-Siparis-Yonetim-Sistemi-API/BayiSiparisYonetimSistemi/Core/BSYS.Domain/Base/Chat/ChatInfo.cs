
namespace BSYS.Domain.Base.Chat;

public class ChatInfo
{
    public class Bayi
    {
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
    }
    public class Admin
    {
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public int Load { get; set; } // Admin üzerindeki yük sayacı
        public List<Bayi> Bayiler { get; set; }
    }
}
