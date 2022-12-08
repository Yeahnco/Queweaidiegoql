using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServiceVisita : AbstractService<Visita>
    {
        public override List<Visita> GetEntities()
        {
            return nmaEn.Visita.ToList<Visita>();
        }

        public override Visita GetEntity(object key)
        {
            return nmaEn.Visita.Where(visita => visita.Actividad_id_act == (int)key).FirstOrDefault<Visita>();
        }
    }
}

