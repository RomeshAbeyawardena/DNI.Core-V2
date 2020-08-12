using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    public interface IModelEncryptionProvider
    {
        TDestination Encrypt<T, TDestination>(T model);
        TDestination Decrypt<T, TDestination>(T model);
    }
}
