using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class ServiceCapacitacion : AbstractService<Capacitacion>
    {
        public override List<Capacitacion> GetEntities()
        {
            return nmaEn.Capacitacion.ToList<Capacitacion>();
        }

        public override Capacitacion GetEntity(object key)
        {
            return nmaEn.Capacitacion.Where(Capacitacion => Capacitacion.Actividad_id_act == (int)key).FirstOrDefault<Capacitacion>();
        }
    }
}
