using MongoDB.Driver;
using Backend.Models;

namespace Backend.Services;

public interface IDbClient
{
    IMongoCollection<Menu> MenuCollection();
    IMongoCollection<Blog> BlogCollection();
    IMongoCollection<Announcement> AnnouncementCollection();
}