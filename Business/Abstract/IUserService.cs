using Core.Business.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService : IServiceRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
        User GetByMail(string email);
    }
}
