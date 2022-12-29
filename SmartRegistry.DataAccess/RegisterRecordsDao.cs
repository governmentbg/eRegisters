using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
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
    public class RegisterRecordsDao : BaseDao<RegisterRecord,long>, IRegisterRecordsDao
    {
        public RegisterRecordsDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public PagedResult<RegisterRecord> GetRecords(RegisterRecordFilter filter)
        {
            var criteria = Session.CreateCriteria<RegisterRecord>("RegRecord");
            if (filter != null)
            {
                criteria = ApplyRegisterRecordFilterToCriteria(criteria, filter);
            }

         //   ApplyOrderToCriteria(criteria, filter);

            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        private ICriteria ApplyRegisterRecordFilterToCriteria(NHibernate.ICriteria criteria, RegisterRecordFilter filter)
        {
            if (filter.Register != null)
            {
                criteria = criteria.Add(Restrictions.Eq("Register",filter.Register));
            }

            int aliasCnt = 0;
            foreach (var attrFilter in filter.AttributeFilters)
            {
                aliasCnt++;
                switch (attrFilter.Attribute.UnifiedData.TableType)
                {
                    case UnifiedDataTableType.Varchar:
                        criteria = ApplyVarcharAttributeFilter(criteria, attrFilter, aliasCnt);
                        break;
                    case UnifiedDataTableType.Text:
                        criteria = ApplyTextAttributeFilter(criteria, attrFilter, aliasCnt);
                        break;
                    case UnifiedDataTableType.Integer:
                        criteria = ApplyIntegerAttributeFilter(criteria, attrFilter, aliasCnt);
                        break;
                    case UnifiedDataTableType.Numeric:
                        criteria = ApplyDecimalAttributeFilter(criteria, attrFilter, aliasCnt);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(filter.URI))
            {
                if (filter.URI.StartsWith("%") || filter.URI.EndsWith("%"))
                {
                    criteria.Add(Restrictions.Like("URI", filter.URI));
                }
                else
                {
                    criteria.Add(Restrictions.Eq("URI", filter.URI));
                }
            }

            if (!string.IsNullOrEmpty(filter.State))
            {
                if (filter.State.StartsWith("%") || filter.State.EndsWith("%"))
                {
                    var critStates = criteria.CreateCriteria("RegisterState");
                    critStates.Add(Restrictions.Like("Name", filter.State));                    
                }
            }

            if (!string.IsNullOrEmpty(filter.ExternalId))
            {                
                    criteria.Add(Restrictions.Eq("ExternalId", filter.ExternalId));
            }

            return criteria;
        }

        private ICriteria ApplyIntegerAttributeFilter(ICriteria criteria, RegisterAttributeFilterBase attrFilter, int aliasCnt)
        {
            var intFilter = (attrFilter as RegisterAttributeIntFilter);
            if (intFilter == null) return criteria;

            var aliasName = "AttrAlias_" + aliasCnt.ToString();
            var detCrit = DetachedCriteria.For<RegisterRecordValueInt>(aliasName)
                .SetProjection(Projections.Property($"{aliasName}.RegisterRecord.Id"))
                .Add(Restrictions.Eq("Attribute", intFilter.Attribute))
                .Add(Restrictions.EqProperty($"{aliasName}.RegisterRecord.Id", "RegRecord.Id"))
                .Add(Restrictions.Eq("Value", intFilter.FilterValue));

            criteria = criteria.Add(Subqueries.Exists(detCrit));

            return criteria;
        }

        private ICriteria ApplyDecimalAttributeFilter(ICriteria criteria, RegisterAttributeFilterBase attrFilter, int aliasCnt)
        {
            var intFilter = (attrFilter as RegisterAttributeDecimalFilter);
            if (intFilter == null) return criteria;

            var aliasName = "AttrAlias_" + aliasCnt.ToString();
            var detCrit = DetachedCriteria.For<RegisterRecordValueNumeric>(aliasName)
                .SetProjection(Projections.Property($"{aliasName}.RegisterRecord.Id"))
                .Add(Restrictions.Eq("Attribute", intFilter.Attribute))
                .Add(Restrictions.EqProperty($"{aliasName}.RegisterRecord.Id", "RegRecord.Id"))
                .Add(Restrictions.Eq("Value", intFilter.FilterValue));

            criteria = criteria.Add(Subqueries.Exists(detCrit));

            return criteria;
        }

        private ICriteria ApplyVarcharAttributeFilter(ICriteria criteria, RegisterAttributeFilterBase attrFilter, int aliasCnt)
        {
            var textFilter = (attrFilter as RegisterAttributeTextFilter);
            if (textFilter == null) return criteria;

            var aliasName = "AttrAlias_" + aliasCnt.ToString();
            var detCrit = DetachedCriteria.For<RegisterRecordValueNVarchar>(aliasName)
                .SetProjection(Projections.Property($"{aliasName}.RegisterRecord.Id"))
                .Add(Restrictions.Eq("Attribute",textFilter.Attribute))
                .Add(Restrictions.EqProperty($"{aliasName}.RegisterRecord.Id", "RegRecord.Id"));

            ApplyStringFilterToDetachedCriteria(detCrit, "ValueDB", textFilter.FilterValue);

            criteria = criteria.Add(Subqueries.Exists(detCrit));

            return criteria;
        }

        private ICriteria ApplyTextAttributeFilter(ICriteria criteria, RegisterAttributeFilterBase attrFilter, int aliasCnt)
        {
            var textFilter = (attrFilter as RegisterAttributeTextFilter);
            if (textFilter == null) return criteria;

            var aliasName = "AttrAlias_" + aliasCnt.ToString();
            var detCrit = DetachedCriteria.For<RegisterRecordValueText>(aliasName)
                .SetProjection(Projections.Property($"{aliasName}.RegisterRecord.Id"))
                .Add(Restrictions.Eq("Attribute", textFilter.Attribute))
                .Add(Restrictions.EqProperty($"{aliasName}.RegisterRecord.Id", "RegRecord.Id"));

            ApplyStringFilterToDetachedCriteria(detCrit, "ValueDB", textFilter.FilterValue);

            criteria = criteria.Add(Subqueries.Exists(detCrit));

            return criteria;
        }

        public void ClearRegisterRecords(Register reg)
        {

            //manual delete
            //var listNvarchar = Session.Query<RegisterRecordValueNVarchar>()
            //.Where(x => x.RegisterRecord.Register == reg).ToList();
            //foreach (RegisterRecordValueNVarchar nvar in listNvarchar)
            //{
            //    Session.Delete(nvar);
            //}

            Session.Query<RegisterRecord>()
               .Where(x => x.Register == reg)
               .Delete();
        }
    }
}
