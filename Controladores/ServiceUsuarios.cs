using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class ServiceUsuarios : AbstractService<Usuarios>
    {
        public override List<Usuarios> GetEntities()
        {
            return nmaEn.Usuarios.ToList<Usuarios>();
        }

        public override Usuarios GetEntity(object key)
        {
            return nmaEn.Usuarios.Where(usuarios => usuarios.id == (int)key).FirstOrDefault<Usuarios>();
        }
    }
}
