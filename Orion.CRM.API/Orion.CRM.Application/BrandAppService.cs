using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class BrandAppService
    {
        private BrandDataAdapter adapter = new BrandDataAdapter();
        public int InsertBrand(Entity.Brand brand)
        {
            return adapter.InsertBrand(brand);
        }

        public bool UpdateBrand(Entity.Brand brand)
        {
            return adapter.UpdateBrand(brand);
        }

        public bool DeleteBrand(int id)
        {
            return adapter.DeleteBrand(id);
        }

        public IEnumerable<Entity.Brand> GetAllBrands()
        {
            return adapter.GetAllBrands();
        }

        public IEnumerable<Entity.Brand> GetBrandsByProjectId(int projectId)
        {
            return adapter.GetBrandsByProjectId(projectId);
        }
    }
}
