using ShareSpace.DataAccess.Data;
using ShareSpace.Models;
using System.Linq.Expressions;

namespace ShareSpace.DataAccess.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        void Update(Post post);
    }
}
