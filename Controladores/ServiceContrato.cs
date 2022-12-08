using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServiceContrato : AbstractService<Contrato>
    {
        public override List<Contrato> GetEntities()
        {
            return nmaEn.Contrato.ToList<Contrato>();
        }

        public override Contrato GetEntity(object key)
        {
            return nmaEn.Contrato.Where(contrato => contrato.Gerente_id_gerente == (int)key).FirstOrDefault<Contrato>();
        }
    }
}
