using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServicePago : AbstractService<Pago>
    {

        public override List<Pago> GetEntities()
        {
            return nmaEn.Pago.ToList<Pago>();
        }

        public override Pago GetEntity(object key)
        {
            return nmaEn.Pago.Where(pago => pago.id_pago == (int)key).FirstOrDefault<Pago>();
        }
    }
}
