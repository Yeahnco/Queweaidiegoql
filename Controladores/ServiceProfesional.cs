using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServiceProfesional : AbstractService<Profesional>
    {
        public override List<Profesional> GetEntities()
        {
            return nmaEn.Profesional.ToList<Profesional>();
        }

        public override Profesional GetEntity(object key)
        {
            return nmaEn.Profesional.Where(profesional => profesional.id_prof == (int)key).FirstOrDefault<Profesional>();
        }
    }
}
