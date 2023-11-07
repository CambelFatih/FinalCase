using System.Collections.Concurrent;
using System.Text;
using BSYS.Application.Abstractions.Hubs;

namespace BSYS.SignalR.HubServices;

public class ChatHubService : IChatHubService
{
    private readonly ConcurrentDictionary<string, string> activeAdmins = new ConcurrentDictionary<string, string>();
    private readonly ConcurrentDictionary<string, string> activeUsers = new ConcurrentDictionary<string, string>();
    private readonly ConcurrentDictionary<string, Tuple<string, string>> customerToAdminMapping = new ConcurrentDictionary<string, Tuple<string, string>>();
    private readonly ConcurrentDictionary<string, int> adminCustomerCount = new ConcurrentDictionary<string, int>();


    public async Task<string> GetAdminConnectionId()
    {
        string singleAdminConnectionId = activeAdmins.Keys.SingleOrDefault();
        if (!string.IsNullOrEmpty(singleAdminConnectionId))
        {
            return singleAdminConnectionId;
        }
        else
        {
            return "Null";
        }
    }
    public async Task<string> AssignAdminToCustomer(string customerId)
    {
        // En az yükü olan admini bulma mantığı
        var adminId =  await FindLeastBusyAdmin();
        if (adminId == null)
        {
            return null; // Eğer uygun admin yoksa null döndür
        }

        // Müşteriyi adminle eşleştir ve grup ismi olarak adminId'yi kullan
        customerToAdminMapping[customerId] = new Tuple<string, string>(adminId, adminId); // Grup ismi olarak adminId kullanılıyor
        IncrementAdminCustomerCount(adminId); // Adminin yükünü artır

        return adminId; // Grup ismini döndür
    }

    public async void DisconnectCustomerFromAdmin(string customerId)
    {
        // Eğer müşteri eşleştirilmişse, bağlantıyı kes
        Tuple<string, string> adminId;
        if (customerToAdminMapping.TryRemove(customerId, out adminId))
        {
            DecrementAdminCustomerCount(adminId.Item1); // Adminin yükünü azalt
        }
    }

    public async Task<string> GetCustomerConnectionId(string customerId)
    {
        // Müşterinin bağlantı ID'sini döndürür
        Tuple<string, string> adminToCustomer;
        if (customerToAdminMapping.TryGetValue(customerId, out adminToCustomer))
        {
            return adminToCustomer.Item2; // Müşterinin connectionId'si
        }
        return null;
    }

    public async Task<bool> IsAdminActive(string userId)
    {
        return activeAdmins.Values.Any(id => id == userId);
    }

    public async void RemoveInactiveAdmin(string connectionId)
    {
        // Eğer kullanıcı activeAdmins sözlüğünde yer alıyorsa kullanıcıyı aktif admin listesinden çıkarır.
        activeAdmins.TryRemove(connectionId, out _); // 'out _' kullanarak geri dönen değeri yok sayabiliriz.
    }

    public async void AddActiveAdmin(string connectionId, string userId)
    {
        // Eğer connectionId mevcut değilse yeni bir kayıt ekler, varsa mevcut kaydı günceller.
        activeAdmins.AddOrUpdate(connectionId, userId, (key, oldValue) => userId);
    }

    public async Task<bool> IsUserActive(string userId)
    {
        return activeUsers.Values.Any(id => id == userId);
    }

    public async void RemoveInactiveUser(string connectionId)
    {
        // Eğer kullanıcı activeUsers sözlüğünde yer alıyorsa kullanıcıyı aktif kullanıcı listesinden çıkarır.
        activeUsers.TryRemove(connectionId, out _); // 'out _' kullanarak geri dönen değeri yok sayabiliriz.
    }

    public async void AddActiveUser(string connectionId, string userId)
    {
        // Eğer connectionId mevcut değilse yeni bir kayıt ekler, varsa mevcut kaydı günceller.
        activeUsers.AddOrUpdate(connectionId, userId, (key, oldValue) => userId);
    }

    private async Task<string> FindLeastBusyAdmin()
    {
        // Adminler arasından en az yükü olanı bulma mantığı
        return adminCustomerCount.OrderBy(kvp => kvp.Value).FirstOrDefault().Key;
    }

    private async void IncrementAdminCustomerCount(string adminId)
    {
        adminCustomerCount.AddOrUpdate(adminId, 1, (key, oldValue) => oldValue + 1);
    }

    private async void DecrementAdminCustomerCount(string adminId)
    {
        int currentCount;
        if (adminCustomerCount.TryGetValue(adminId, out currentCount))
        {
            adminCustomerCount.TryUpdate(adminId, currentCount - 1, currentCount);
        }
    }

}

