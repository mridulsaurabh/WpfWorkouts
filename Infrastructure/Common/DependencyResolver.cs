using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class DependencyResolver
    {
        private ISimpleIocContainer _container;
        public DependencyResolver(ISimpleIocContainer container)
        {
            this._container = container;
        }

        public object Resolve(Type typeToResolve)
        {
            return this._container.Resolve(typeToResolve);
        }
    }
}
