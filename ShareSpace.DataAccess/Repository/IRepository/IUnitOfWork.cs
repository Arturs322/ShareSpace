using ShareSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSpace.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPostRepository Post { get; }
        IPostLikeRepository PostLike { get; }
        IFollowRepository Follow { get; }
        IPostCommentRepository PostComment { get; }
        void Save();
    }
}
