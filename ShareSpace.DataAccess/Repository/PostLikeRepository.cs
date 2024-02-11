using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShareSpace.DataAccess.Data;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;
using System.Linq.Expressions;

namespace ShareSpace.DataAccess.Repository
{
    public class PostLikeRepository : Repository<PostLike>, IPostLikeRepository
    {
        private ApplicationDbContext _db;
        public PostLikeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PostLike postLike)
        {
            _db.PostLikes.Update(postLike);
        }
    }
}
