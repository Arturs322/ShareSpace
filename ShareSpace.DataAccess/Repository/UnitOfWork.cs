using ShareSpace.DataAccess.Data;
using ShareSpace.DataAccess.Repository.IRepository;

namespace ShareSpace.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IPostRepository Post { get; private set; }
        public IPostLikeRepository PostLike { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Post = new PostRepository(_db);
            PostLike = new PostLikeRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}