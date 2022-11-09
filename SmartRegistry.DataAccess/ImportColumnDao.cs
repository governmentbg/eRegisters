using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class ImportColumnDao : BaseDao<ImportColumn, int>, IImportColumnDao
    {
        public ImportColumnDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public IList<ImportColumn> GetColumnsForHead(ImportHead head)
        {
            return Session.Query<ImportColumn>().Where(x => x.Head == head).ToList();
        }
    }
}
