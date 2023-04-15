using MongoDB.Driver;
using Backend.Models;

namespace Backend.Services;

public interface IDbClient
{
    public IMongoCollection<Menu> MenuCollection();
    public IMongoCollection<User> UserCollection();
    public IMongoCollection<Blog> BlogCollection();
    public IMongoCollection<Announcement> AnnouncementCollection();
}