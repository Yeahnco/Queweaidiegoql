using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServiceComprobante : AbstractService<Comprobantes>
    {
        public override List<Comprobantes> GetEntities()
        {
            return nmaEn.Comprobantes.ToList<Comprobantes>();
        }

        public override Comprobantes GetEntity(object key)
        {
            return nmaEn.Comprobantes.Where(comprobantes => comprobantes.id_comprobante == (int)key).FirstOrDefault<Comprobantes>();
        }
    }
}
