using Alumni.Models;

namespace Alumni.Services;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User? GetById(int id);
    User Create(CreateUserRequest request);
    User? Update(int id, UpdateUserRequest request);
    User? Patch(int id, PatchUserRequest request);
    bool Delete(int id);
}
