using NHibernate;
using NHibernate.Criterion;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class RegistersDao : BaseDao<Register, int>, IRegistersDao
    {
        public RegistersDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        protected ICriteria ApplyRegisterFilterToCriteria(ICriteria criteria, RegisterFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Uri))
            {
                if (filter.Uri.StartsWith("%") || filter.Uri.EndsWith("%"))
                {
                    criteria.Add(Restrictions.Like("URI", filter.Uri));
                }
                else
                {
                    criteria.Add(Restrictions.Eq("URI", filter.Uri));
                }
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                if (filter.Name.StartsWith("%") || filter.Name.EndsWith("%"))
                {
                    criteria.Add(Restrictions.Like("Name", filter.Name));
                }
                else
                {
                    criteria.Add(Restrictions.Eq("Name", filter.Name));
                }
            }
            if (!string.IsNullOrEmpty(filter.AdminBodyName))
            {
                var critAdminBodies = criteria.CreateCriteria("AdministrativeBody");
                if (filter.AdminBodyName.StartsWith("%") || filter.AdminBodyName.EndsWith("%"))
                {
                    critAdminBodies.Add(Restrictions.Like("Name", filter.AdminBodyName));
                }
                else
                {
                    critAdminBodies.Add(Restrictions.Eq("Name", filter.AdminBodyName));
                }
            }
            if (filter.AdminBody != null)
            {
                criteria.Add(Restrictions.Eq("AdministrativeBody", filter.AdminBody));
            }

            if ((filter.CheckForPermission != null) && (filter.CurrentUser != null))
            {
                DetachedCriteria detCrit = DetachedCriteria.For<User>("Usr")
                    .Add(Restrictions.Eq("Id", filter.CurrentUser.Id))
                    .CreateAlias("UserGroups", "UsrGrp")
                    .CreateAlias("UsrGrp.RegisterRights", "RegRight")
                    .Add(Restrictions.Eq("RegRight.Permission", filter.CheckForPermission))
                    .SetProjection(Projections.Distinct(Projections.Property("RegRight.Register.Id")));

                criteria.Add(Subqueries.PropertyIn("Id", detCrit));
            }
            if (filter.Area != null) {
                criteria.CreateAlias("RegisterAreas", "RegisterArea");
                criteria.CreateAlias("RegisterArea.Area", "Area");
                criteria.Add(Restrictions.Eq("Area.Id", filter.Area));
               // criteria.Add(Restrictions.Eq("RegisterArea.Id", filter.Area));
            }

            if (filter.RegistersArea != null) {
                criteria.CreateAlias("RegisterAreas", "RegisterArea");
                criteria.CreateAlias("RegisterArea.Area", "Area");
                criteria.CreateAlias("Area.AreaGroup", "AreaGroups");
                criteria.Add(Restrictions.Or(Restrictions.Like("AreaGroups.Name", filter.RegistersArea), Restrictions.Like("Area.Name", filter.RegistersArea)));
               
            }

            if (filter.OrderByColumn == "AdminBody")
            {
                criteria.CreateAlias("AdministrativeBody", "AdministrativeBody");
            }

            if (filter.OrderByColumn == "Status")
            {
                filter.OrderByColumn = "IsActive";
            }

            if (filter.IsActive != null)
            {
                criteria.Add(Restrictions.Eq("IsActive", filter.IsActive));
            }


            return criteria;
        }

        public PagedResult<Register> GetAll(RegisterFilter filter)
        {
            var criteria = Session.CreateCriteria<Register>();
            if (filter != null)
            {
                criteria = ApplyRegisterFilterToCriteria(criteria, filter);
            }

            ApplyOrderToCriteria(criteria, filter);

            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        public int GetCount(RegisterFilter filter)
        {        
            var criteria = Session.CreateCriteria<Register>();
            if (filter != null)
            {
                criteria = ApplyRegisterFilterToCriteria(criteria, filter);
            }

            var countCriteria = CriteriaTransformer.TransformToRowCount(criteria);
            var count = countCriteria.UniqueResult<int>();
            return count;
        }

        public int GetDistinctAdminBodyCount()
        {
            ICriteria criteria2 = Session.CreateCriteria(typeof(Register));
            criteria2.SetProjection(
                Projections.Distinct(Projections.ProjectionList()
                    .Add(Projections.Alias(Projections.Property("AdministrativeBody"), "AdministrativeBody"))));
            criteria2.SetResultTransformer(
                new NHibernate.Transform.AliasToBeanResultTransformer(typeof(Register)));
            IList<Register> registers = criteria2.List<Register>();

            return registers.Count();
        }

        
    }
}
