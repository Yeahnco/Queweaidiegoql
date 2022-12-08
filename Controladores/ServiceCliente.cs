using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServiceCliente : AbstractService<Cliente>
    {
        public override List<Cliente> GetEntities()
        {
            return nmaEn.Cliente.ToList<Cliente>();
        }

        public override Cliente GetEntity(object key)
        {
            return nmaEn.Cliente.Where(cliente => cliente.id_emp == (int)key).FirstOrDefault<Cliente>();
        }
    }
}
