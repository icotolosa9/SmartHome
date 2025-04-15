using IDataAccess;
using DataAccess.Context;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly SmartHomeContext _smartHomeContext;

        public SessionRepository(SmartHomeContext smartHomeContext)
        {
            _smartHomeContext = smartHomeContext;
        }

        public void Save(Session session)
        {
            _smartHomeContext.Sessions.Add(session);
            _smartHomeContext.SaveChanges();
        }

        public Session GetByToken(Guid token)
        {
            return _smartHomeContext.Sessions.SingleOrDefault(session => session.Token == token);
        }
    }
}
