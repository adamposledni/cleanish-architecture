namespace Onion.Application.DataAccess.Repositories
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
    }
}
