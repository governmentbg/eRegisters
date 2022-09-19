using NHibernate;
using NHibernate.Criterion;
using Orak.Utils;
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
    public class SystemLogDao : BaseDao<SystemLogEvent, long>, ISystemLog
    {

        public SystemLogDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public PagedResult<SystemLogEvent> GetAll(SystemLogFilter filter)
        {
            var criteria = Session.CreateCriteria<SystemLogEvent>();
            if (filter != null)
            {
                criteria = ApplyFilterToCriteria(criteria, filter);
            }

            if (filter.OrderByColumn == "Type")
            {
                var proj = Projections.Constant(string.Empty);
                var dataTypesOrdDict = EnumUtils.GetValues<SystemLogEventTypeEnum>();

                foreach (var dt in dataTypesOrdDict)
                {
                    proj = Projections.Conditional(
                        Restrictions.Eq("Type", dt),
                        Projections.Constant(EnumUtils.GetDisplayName(dt)),
                        proj
                        );
                }

                if (filter.OrderByDirection == OrderDbEnum.Asc)
                {
                    criteria.AddOrder(Order.Asc(proj));
                }
                else
                {
                    criteria.AddOrder(Order.Desc(proj));
                }
            }
            else
            {
                ApplyOrderToCriteria(criteria, filter);
            }
           
            var result = GetPagedResultForCriteria(criteria, filter);

            return result;

        }

        protected ICriteria ApplyFilterToCriteria(ICriteria criteria, SystemLogFilter filter)
        {
           
            if (!string.IsNullOrEmpty(filter.Name))
            {
                if (filter.Name.StartsWith("%") || filter.Name.EndsWith("%"))
                {
                  //  criteria.Add(Restrictions.Like("Name", filter.Name));
                    criteria.CreateAlias("CurrentUser", "User");
                    criteria.Add(Restrictions.Like("User.Name", filter.Name));
                }
                else
                {
                  //  criteria.Add(Restrictions.Eq("Name", filter.Name));
                    criteria.CreateAlias("CurrentUser", "User");
                    criteria.Add(Restrictions.Eq("User.Name", filter.Name));
                }
            }

            if (!string.IsNullOrEmpty(filter.AdminBodyName))
            {
                if (filter.AdminBodyName.StartsWith("%") || filter.AdminBodyName.EndsWith("%"))
                {
                    criteria.CreateAlias("AdministrativeBody", "AdministrativeBody");
                    criteria.Add(Restrictions.Like("AdministrativeBody.Name", filter.AdminBodyName));
                }
                else
                {
                    criteria.CreateAlias("AdministrativeBody", "AdministrativeBody");
                    criteria.Add(Restrictions.Eq("AdministrativeBody.Name", filter.AdminBodyName));
                }
            }

            if (filter.Type != 0)
            {
                criteria.Add(Restrictions.Eq("Type", filter.Type));
            }

            if (filter.OrderByColumn != null)
            {
                criteria.CreateAlias("AdministrativeBody", "AdministrativeBody");
            }
            else {
                var ord = Order.Desc("EventTime");
                criteria.AddOrder(ord);
            }
           
            return criteria;
        }



        public void LogEvent(ISmartRegistryContext context, SystemLogEventTypeEnum eventType, string logMessage, long objectId)
        {

            try
            {
                SystemLogEvent slog = new SystemLogEvent();
                slog.EventTime = DateTime.Now;
                slog.CurrentUser = context.CurrentUser;               
                slog.Type = eventType;
                slog.ObjectId = objectId;
                slog.InfoMessage = logMessage;
                slog.AdministrativeBody = context.CurrentAdminBody;
               
                Session.Save(slog);
            }
            catch (Exception ex) {

            }
        }
    }
}
