using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServiceGerente : AbstractService<Gerente>
    {
        public override List<Gerente> GetEntities()
        {
            return nmaEn.Gerente.ToList<Gerente>();
        }

        public override Gerente GetEntity(object key)
        {
            return nmaEn.Gerente.Where(gerente => gerente.id_gerente == (int)key).FirstOrDefault<Gerente>();
        }
    }
}
