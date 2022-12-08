using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class ServiceChecklist : AbstractService<Checklist>
    {
        public override List<Checklist> GetEntities()
        {
            return nmaEn.Checklist.ToList<Checklist>();
        }

        public override Checklist GetEntity(object key)
        {
            return nmaEn.Checklist.Where(Checklist => Checklist.id_checklist == (int)key).FirstOrDefault<Checklist>();
        }
    }
}