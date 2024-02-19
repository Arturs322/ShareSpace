using ShareSpace.DataAccess.Data;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;

namespace ShareSpace.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IPostRepository Post { get; private set; }
        public IPostLikeRepository PostLike { get; private set; }
        public IFollowRepository Follow { get; private set; }
        public IPostCommentRepository PostComment { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Post = new PostRepository(_db);
            PostLike = new PostLikeRepository(_db);
            Follow = new FollowRepository(_db);
            PostComment = new PostCommentRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}