using System.Collections.Concurrent;
using BSYS.Application.Abstractions.Hubs;

namespace BSYS.SignalR.HubServices;

public class AdminHubService : IAdminHubService
{
    private readonly ConcurrentDictionary<string, string> activeAdmins = new ConcurrentDictionary<string, string>();
    private readonly ConcurrentDictionary<string, string> activeUsers = new ConcurrentDictionary<string, string>();
    private readonly ConcurrentDictionary<string, Tuple<string, string>> customerToAdminMapping = new ConcurrentDictionary<string, Tuple<string, string>>();
    private readonly ConcurrentDictionary<string, int> adminCustomerCount = new ConcurrentDictionary<string, int>();

    public string AssignAdminToCustomer(string customerId)
    {
        // En az yükü olan admini bulma mantığı
        var adminId = FindLeastBusyAdmin();
        if (adminId == null)
        {
            return null; // Eğer uygun admin yoksa null döndür
        }

        // Müşteriyi adminle eşleştir ve grup ismi olarak adminId'yi kullan
        customerToAdminMapping[customerId] = new Tuple<string, string>(adminId, adminId); // Grup ismi olarak adminId kullanılıyor
        IncrementAdminCustomerCount(adminId); // Adminin yükünü artır

        return adminId; // Grup ismini döndür
    }

    public void DisconnectCustomerFromAdmin(string customerId)
    {
        // Eğer müşteri eşleştirilmişse, bağlantıyı kes
        Tuple<string, string> adminId;
        if (customerToAdminMapping.TryRemove(customerId, out adminId))
        {
            DecrementAdminCustomerCount(adminId.Item1); // Adminin yükünü azalt
        }
    }

    public string GetCustomerConnectionId(string customerId)
    {
        // Müşterinin bağlantı ID'sini döndürür
        Tuple<string, string> adminToCustomer;
        if (customerToAdminMapping.TryGetValue(customerId, out adminToCustomer))
        {
            return adminToCustomer.Item2; // Müşterinin connectionId'si
        }
        return null;
    }

    public bool IsAdminActive(string userId)
    {
        return activeAdmins.Values.Any(id => id == userId);
    }

    public void RemoveInactiveAdmin(string connectionId)
    {
        // Eğer kullanıcı activeAdmins sözlüğünde yer alıyorsa kullanıcıyı aktif admin listesinden çıkarır.
        activeAdmins.TryRemove(connectionId, out _); // 'out _' kullanarak geri dönen değeri yok sayabiliriz.
    }

    public void AddActiveAdmin(string connectionId, string userId)
    {
        // Eğer connectionId mevcut değilse yeni bir kayıt ekler, varsa mevcut kaydı günceller.
        activeAdmins.AddOrUpdate(connectionId, userId, (key, oldValue) => userId);
    }

    public bool IsUserActive(string userId)
    {
        return activeUsers.Values.Any(id => id == userId);
    }

    public void RemoveInactiveUser(string connectionId)
    {
        // Eğer kullanıcı activeUsers sözlüğünde yer alıyorsa kullanıcıyı aktif kullanıcı listesinden çıkarır.
        activeUsers.TryRemove(connectionId, out _); // 'out _' kullanarak geri dönen değeri yok sayabiliriz.
    }

    public void AddActiveUser(string connectionId, string userId)
    {
        // Eğer connectionId mevcut değilse yeni bir kayıt ekler, varsa mevcut kaydı günceller.
        activeUsers.AddOrUpdate(connectionId, userId, (key, oldValue) => userId);
    }

    private string FindLeastBusyAdmin()
    {
        // Adminler arasından en az yükü olanı bulma mantığı
        return adminCustomerCount.OrderBy(kvp => kvp.Value).FirstOrDefault().Key;
    }

    private void IncrementAdminCustomerCount(string adminId)
    {
        adminCustomerCount.AddOrUpdate(adminId, 1, (key, oldValue) => oldValue + 1);
    }

    private void DecrementAdminCustomerCount(string adminId)
    {
        int currentCount;
        if (adminCustomerCount.TryGetValue(adminId, out currentCount))
        {
            adminCustomerCount.TryUpdate(adminId, currentCount - 1, currentCount);
        }
    }

}
