using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShareSpace.DataAccess.Data;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;
using System.Linq.Expressions;

namespace ShareSpace.DataAccess.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private ApplicationDbContext _db;
        public PostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Post post)
        {
            _db.Posts.Update(post);
        }
    }
}
