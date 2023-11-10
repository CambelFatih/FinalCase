using BSYS.Application.Abstractions.Hubs;
using System.Security.Claims;
using static BSYS.Domain.Base.Chat.ChatInfo;

namespace BSYS.SignalR.HubServices;

public class ChatHubService : IChatHubService
{
    private  List<Admin> _admins;

    public async void RemoveAdminAsync(string userName)
    {
        if (_admins != null)
        {
            // UserName ile aynı isimde admin var mı kontrolü
            var adminToRemove = _admins.FirstOrDefault(admin => admin.UserName == userName);

            if (adminToRemove != null)
            {
                // Admin'i listeden kaldır
                _admins.Remove(adminToRemove);
            }
        }
    }
    public async Task<string> GetAdminConnectionIdByBayiUserName(string bayiUserName)
    {
        var admin = _admins.FirstOrDefault(a => a.Bayiler?.Any(b => b.UserName == bayiUserName) == true);

        return admin?.ConnectionId;
    }

    public async Task<bool> HasActiveAdmin()
    {
        return _admins.Any();
    }
    public async Task<string> AddBayiToMinLoadAdmin(string bayiUserName, string bayiConnectionId)
    {
        // Yükü en az olan admini bul
        var minLoadAdmin = _admins.OrderBy(admin => admin.Load).FirstOrDefault();

        if (minLoadAdmin != null)
        {
            // Bayiyi yükü en az olan adminin listesine ekle
            minLoadAdmin.Bayiler ??= new List<Bayi>();
            minLoadAdmin.Bayiler.Add(new Bayi { UserName = bayiUserName, ConnectionId = bayiConnectionId });

            // Adminin yükünü ve bayi sayısını güncelle
            minLoadAdmin.Load++;

            // Bayiyi eklediğimiz adminin connectionId'sini döndür
            return minLoadAdmin.ConnectionId;
        }

        // Yükü en az olan admin bulunamazsa null döndür
        return null;
    }
    public async void RemoveBayiAndDecrementLoad(string bayiUserName)
    {
        if (_admins != null && _admins.Any())
        {
            foreach (var admin in _admins)
            {
                var bayiToRemove = admin.Bayiler.FirstOrDefault(bayi => bayi.UserName == bayiUserName);

                if (bayiToRemove != null)
                {
                    admin.Bayiler.Remove(bayiToRemove);
                    admin.Load--; // Bayi silindiğinde adminin yükünü azalt
                    break; // Bayi bulundu, işlem tamamlandı
                }
            }
        }
    }

    public async Task<bool> DoesBayiExistInAdmins(string bayiUserName)
    {
        return _admins.Any(admin => admin.Bayiler.Any(bayi => bayi.UserName == bayiUserName));
    }
    public async void CreateAdmin(string userName, string connectionId)
    {
        if (_admins == null)
        {
            _admins = new List<Admin>();
        }

        // UserName ile aynı isimde admin var mı kontrolü
        if (_admins.Any(admin => admin.UserName == userName))
        {
            // Eğer aynı isimde admin varsa hiçbir işlem yapma
            return;
        }

        var newAdmin = new Admin
        {
            UserName = userName,
            ConnectionId = connectionId,
            Load = 0,
            Bayiler = new List<Bayi>()
        };

        _admins.Add(newAdmin); // Oluşturulan admin'i listeye ekle
    }

    public async Task<bool> HasBayiRole(List<Claim> rolesClaims)
    {
        return rolesClaims.Any(roleClaim => roleClaim.Value.Equals("Bayi", StringComparison.OrdinalIgnoreCase));
    }
    public async Task<bool> HasAdminRole(List<Claim> rolesClaims)
    {
        return rolesClaims.Any(roleClaim => roleClaim.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));
    }
}

