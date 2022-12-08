using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistenciaBD;

namespace Controladores
{
    public class ServicePlan : AbstractService<Plan>
    {
        public override List<Plan> GetEntities()
        {
            return nmaEn.Plan.ToList<Plan>();
        }

        public override Plan GetEntity(object key)
        {
            return nmaEn.Plan.Where(plan => plan.id_plan == (int)key).FirstOrDefault<Plan>();
        }
    }
}