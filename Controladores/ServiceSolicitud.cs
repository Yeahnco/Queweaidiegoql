using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServiceSolicitud : AbstractService<Solicitud>
    {
        public override List<Solicitud> GetEntities()
        {
            return nmaEn.Solicitud.ToList<Solicitud>();
        }

        public override Solicitud GetEntity(object key)
        {
            return nmaEn.Solicitud.Where(Solicitud => Solicitud.id_solicitud == (int)key).FirstOrDefault<Solicitud>();
        }
    }
}
