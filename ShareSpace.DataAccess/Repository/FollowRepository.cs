using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShareSpace.DataAccess.Data;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;
using System.Linq.Expressions;

namespace ShareSpace.DataAccess.Repository
{
    public class FollowRepository : Repository<Follow>, IFollowRepository
    {
        private ApplicationDbContext _db;
        public FollowRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Follow follow)
        {
            _db.Follows.Update(follow);
        }
    }
}
