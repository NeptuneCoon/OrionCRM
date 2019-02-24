using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class BrandDataAdapter : DataAdapter
    {
        public int InsertBrand(Entity.Brand brand)
        {
            if (brand == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@BrandName", brand.BrandName),
                new SqlParameter("@Description", CheckNull(brand.Description)),
                new SqlParameter("@ProjectId", brand.ProjectId)
            };

            int id = SqlMapHelper.ExecuteSqlMapScalar<int>("Brand", "InsertBrand", paramArr);
            return id;
        }

        public bool UpdateBrand(Entity.Brand brand)
        {
            if (brand == null || brand.Id <= 0) return false;


            SqlParameter[] paramArr = {
                new SqlParameter("@Id", brand.Id),
                new SqlParameter("@BrandName", brand.BrandName),
                new SqlParameter("@Description", CheckNull(brand.Description)),
                new SqlParameter("@ProjectId", brand.ProjectId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("Brand", "UpdateBrand", paramArr);
            return count > 0;
        }

        public bool DeleteBrand(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("Brand", "DeleteBrand", param);
            return count > 0;
        }

        public IEnumerable<Entity.Brand> GetAllBrands()
        {
            var brands = SqlMapHelper.GetSqlMapResult<Entity.Brand>("Brand", "GetAllBrands");

            return brands;
        }

        public IEnumerable<Entity.Brand> GetBrandsByProjectId(int projectId)
        {
            SqlParameter parameter = new SqlParameter("@ProjectId", projectId);
            var brands = SqlMapHelper.GetSqlMapResult<Entity.Brand>("Brand", "GetBrandsByProjectId", parameter);

            return brands;
        }
    }
}
