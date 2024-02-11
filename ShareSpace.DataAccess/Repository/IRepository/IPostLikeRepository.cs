using ShareSpace.Models;

namespace ShareSpace.DataAccess.Repository.IRepository
{
    public interface IPostLikeRepository : IRepository<PostLike>
    {
        void Update(PostLike postLike);
    }
}
