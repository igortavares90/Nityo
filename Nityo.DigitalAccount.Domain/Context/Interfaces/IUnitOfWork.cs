using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nityo.DigitalAccount.Domain.Context.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Begin();
        void Commit();
        void Rollback();
        IDbConnection Connection { get; }

    }
}
