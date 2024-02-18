using ShareSpace.Models;

namespace ShareSpace.DataAccess.Repository.IRepository
{
    public interface IFollowRepository : IRepository<Follow>
    {
        void Update(Follow follow);
    }
}
