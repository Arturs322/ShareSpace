using ShareSpace.DataAccess.Data;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;

namespace ShareSpace.DataAccess.Repository
{
    public class PostCommentRepository : Repository<PostComment>, IPostCommentRepository
    {
        private ApplicationDbContext _db;
        public PostCommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PostComment comment)
        {
            _db.PostComments.Update(comment);
        }
    }
}
