using System;
using System.Collections.Generic;
using PersistenciaBD;

namespace Controladores
{
    public abstract class AbstractService<T>
    {
        protected BD_NMAEntities nmaEn = new BD_NMAEntities();

        public abstract T GetEntity(object key);

        public abstract List<T> GetEntities();
    }
}
