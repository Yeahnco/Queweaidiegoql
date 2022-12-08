using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class ServiceAsesoria : AbstractService<Asesoria>
    {
        public override List<Asesoria> GetEntities()
        {
            return nmaEn.Asesoria.ToList<Asesoria>();
        }

        public override Asesoria GetEntity(object key)
        {
            return nmaEn.Asesoria.Where(Asesoria => Asesoria.Actividad_id_act == (int)key).FirstOrDefault<Asesoria>();
        }
    }
}
