using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class ServiceActMejora : AbstractService<Act_de_mejora>
    {
        public override List<Act_de_mejora> GetEntities()
        {
            return nmaEn.Act_de_mejora.ToList<Act_de_mejora>();
        }

        public override Act_de_mejora GetEntity(object key)
        {
            return nmaEn.Act_de_mejora.Where(Act_de_mejora => Act_de_mejora.Actividad_id_act == (int)key).FirstOrDefault<Act_de_mejora>();
        }
    }
}
