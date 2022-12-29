using NHibernate;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class NHibernateDbContext : IDbContext
    {
        private ISession _session;
        public ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = SessionFactoryManager.Instance.SessionFactory.OpenSession();
                    _session.BeginTransaction();
                }
                return _session;
            }
        }

        private RegisterRightsDao _registerRightsDao;
        public IRegisterRightsDao RegisterRightsDao
        {
            get
            {
                if (_registerRightsDao == null)
                {
                    _registerRightsDao = new RegisterRightsDao(this);
                }
                return _registerRightsDao;
            }

        }

        public NHibernateDbContext()
        {
        }

        public void CommitTransaction()
        {
            if (_session == null) return;
            if (_session.GetCurrentTransaction() == null) return;

            _session.GetCurrentTransaction().Commit() ;
        }

        public void RollbackTransaction()
        {
            if (_session == null) return;
            if (_session.GetCurrentTransaction() == null) return;

            _session.GetCurrentTransaction().Rollback();
        }

        public void StartTransaction()
        {
            if (_session == null) return;

            _session.BeginTransaction();
        }

        public void Dispose()
        {
            if (_session == null) return;
            if (_session.GetCurrentTransaction() != null)
            {
                _session.GetCurrentTransaction().Rollback();
            }
            _session.Dispose();
        }

        public IAdministrativeBodiesDao GetAdministrativeBodiesDao()
        {
            AdministrativeBodiesDao admBodDao = new AdministrativeBodiesDao(this);
            return admBodDao;
        }

        public IRegistrySettingsDao GetRegistrySettingsDao()
        {        
            RegistrySettingsDao regDao = new RegistrySettingsDao(this);
            return regDao;
        }

        public IUsersDao GetUsersDao()
        {         
            UsersDao usrDao = new UsersDao(this);
            return usrDao;
        }

        private IUserGroupsDao _userGroupsDao;
        public IUserGroupsDao GetUserGroupsDao()
        {
            if (_userGroupsDao == null) _userGroupsDao = new UserGroupsDao(this);
            return _userGroupsDao;
        }

        public IUserIdentificatorTypesDao GetUserIdentificatorTypesDao()
        {
            UserIdentificatorTypesDao usrTypeDao = new UserIdentificatorTypesDao(this);
            return usrTypeDao;
        }

        public IUserIdentificatorDao GetUserIdentificatorDao()
        {
            UserIdentificatorDao usrIdDao = new UserIdentificatorDao(this);
            return usrIdDao;
        }

        public IUserGroupRightsDao GetUserGroupRightsDao()
        {
            UserGroupRightsDao userGroupRightsDao = new UserGroupRightsDao(this);
            return userGroupRightsDao;
        }

        public IRegistersDao GetRegistersDao()
        {
            RegistersDao registersDao = new RegistersDao(this);
            return registersDao;
        }

        public IWebServicesDao WebServicesDao
        {
            get
            {
                WebServicesDao webServicesDao = new WebServicesDao(this);
                return webServicesDao;
            }
        }

        public IRegisterRecordsDao RegisterRecordsDao
        {
            get
            {
                RegisterRecordsDao registerRecordsDao = new RegisterRecordsDao(this);
                return registerRecordsDao;
            }
        }

        public IImportRowDao ImportRowDao
        {
            get
            {
                ImportRowDao importRowDao = new ImportRowDao(this);
                return importRowDao;
            }
        }

        public IImportHeadDao ImportHeadDao
        {
            get
            {
                ImportHeadDao importHeadDao = new ImportHeadDao(this);
                return importHeadDao;
            }
        }

        public IImportColumnDao ImportColumnDao
        {
            get
            {
                ImportColumnDao importColumnDao = new ImportColumnDao(this);
                return importColumnDao;
            }
        }

        public IRegisterPermissionsDao GetRegisterPermissionsDao()
        {
            return new RegisterPermissionsDao(this);
        }

        public IUnifiedDataDao GetUnifiedDataDao()
        {
            UnifiedDataDao unifiedDataDao = new UnifiedDataDao(this);
            return unifiedDataDao;
        }

        public IUnifiedDataCompositeDao GetUnifiedDataCompositeDao()
        {
            UnifiedDataCompositeDao unifiedDataCompDao = new UnifiedDataCompositeDao(this);
            return unifiedDataCompDao;
        }

        public IRegisterAttributeHeadDao GetRegisterAttributeHeadDao()
        {
            RegisterAttributeHeadDao registerHeadDao = new RegisterAttributeHeadDao(this);
            return registerHeadDao;
        }

        public IRegisterAttributeDao GetRegisterAttributeDao()
        {
            RegisterAttributeDao registerAttributeDao = new RegisterAttributeDao(this);
            return registerAttributeDao;
        }

        public IRegisterAreaDao GetRegisterAreaDao()
        {
            RegisterAreaDao registerAreaDao = new RegisterAreaDao(this);
            return registerAreaDao;
        }

        public IAreasDao GetAreasDao()
        {
            AreasDao areasDao = new AreasDao(this);
            return areasDao;
        }

        public IAreaGroupsDao GetAreaGroupsDao()
        {
            AreaGroupsDao areaGrpDao = new AreaGroupsDao(this);
            return areaGrpDao;
        }

        public ISystemLog GetSystemLogDao() {
            SystemLogDao systemLogDao = new SystemLogDao(this);
            return systemLogDao;
        }

        public IServiceLog GetServiceLogDao()
        {
            ServiceLogDao serviceLogDao = new ServiceLogDao(this);
            return serviceLogDao;
        }

        public IRegisterStatesDao GetRegisterStatesDao()
        {
            RegisterStatesDao regStatesDao = new RegisterStatesDao(this);
            return regStatesDao;
        }
        public IRegisterStatesAttributeDao GetRegisterStatesAttributeDao()
        {
            RegisterStatesAttributeDao regStatesAttrDao = new RegisterStatesAttributeDao(this);
            return regStatesAttrDao;
        }
        public IRegisterTransitionsDao GetRegisterTransitionDao()
        {
            RegisterTransitionsDao regTransDao = new RegisterTransitionsDao(this);
            return regTransDao;
        }

        public IRegisterTransitionRightsDao GetRegisterTransitionRightsDao()
        {
            RegisterTransitionRightsDao regTransRightsDao = new RegisterTransitionRightsDao(this);
            return regTransRightsDao;
        }

        public IWebServicesClientsDao WebServicesClientsDao()
        {            
             WebServicesClientsDao webServicesClientsDao = new WebServicesClientsDao(this);
             return webServicesClientsDao;            
        }
    }

}
