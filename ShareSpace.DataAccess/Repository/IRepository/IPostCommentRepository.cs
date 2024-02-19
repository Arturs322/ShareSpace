using ShareSpace.Models;

namespace ShareSpace.DataAccess.Repository.IRepository
{
    public interface IPostCommentRepository : IRepository<PostComment>
    {
        void Update(PostComment comment);
    }
}
