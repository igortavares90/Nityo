using System;
using System.Collections.Generic;
using System.Text;

namespace Nityo.DigitalAccount.Domain.Context.Interfaces
{
    public interface ICommand
    {
        bool IsValid();
    }
}
