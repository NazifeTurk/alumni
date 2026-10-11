using Alumni.Models;

namespace Alumni.Services;

public interface IAnnouncementService
{
    IEnumerable<Announcement> GetAll();
    Announcement? GetById(int id);
    Announcement Create(CreateAnnouncementRequest request);
    Announcement? Update(int id, UpdateAnnouncementRequest request);
    bool Delete(int id);
}
