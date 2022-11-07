using EgovAdministrativeRegister;
using SmartRegistry.DataAccess;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SmartRegistry.CommonWeb
{
    /// <summary>
    /// In this class all the "wiring" happens 
    /// - e.g. which Data Access Layer to use, which Service implementations to use and so on
    /// </summary>
    public class SmartRegistryWebContext : ISmartRegistryContext
    {

        public SmartRegistryWebContext(IDbContext dbContext, int userId, long adminBodyId)
        {
            _currentUserId = userId;
            _adminBodyId = adminBodyId;
            _dbContext = dbContext;
        }

        private IDbContext _dbContext;
        public IDbContext DbContext 
        {
            get
            {
                return _dbContext;
            }
        }

        private long _adminBodyId;
        private AdministrativeBody _adminBody = null;
        public AdministrativeBody CurrentAdminBody
        {
            get
            {
                if (_adminBody == null)
                {
                    if (_adminBodyId != 0)
                    {
                        var adminBodiesDao = DbContext.GetAdministrativeBodiesDao();
                        _adminBody = adminBodiesDao.GetById(_adminBodyId);
                    }
                }
                return _adminBody;
            }
        }

        private int _currentUserId;
        private User _currentUser = null;
        public User CurrentUser 
        {
            get
            {
                if (_currentUser == null)
                {
                    if (_currentUserId != 0)
                    {
                        var userDao = DbContext.GetUsersDao();
                        _currentUser = userDao.GetById(_currentUserId);
                    }
                }
                return _currentUser;
            }
        }


        private ISystemLog _systemLog = null;
        public ISystemLog SystemLog {
            get {
                if (_systemLog == null) {
                    _systemLog =  new SystemLogDao((NHibernateDbContext)_dbContext);
                }
                return _systemLog;
            }

        }

        public AdministrativeBodiesService AdministrativeBodyService
        {
            get
            {
                var service = (AdministrativeBodiesService)HttpContext.Current.Items["AdminBodyService"];
                if (service == null)
                {
                    service = new AdministrativeBodiesService(this);
                    HttpContext.Current.Items["AdminBodyService"] = service;
                }
                return service;
            }
        }

        public RegistersService RegistersService 
        {
            get
            {
                var service = (RegistersService)HttpContext.Current.Items["RegistersService"];
                if (service == null)
                {
                    service = new RegistersService(this);
                    HttpContext.Current.Items["RegistersService"] = service;
                }
                return service;
            }
        }

        public RegistrySettingsService RegistrySettingsService
        {
            get
            {
                var service = (RegistrySettingsService)HttpContext.Current.Items["RegistrySettingsService"];
                if (service == null)
                {
                    service = new RegistrySettingsService(this);
                    HttpContext.Current.Items["RegistrySettingsService"] = service;
                }
                return service;
            }
        }

        public UnifiedDataService UnifiedDataService
        {
            get
            {
                var service = (UnifiedDataService)HttpContext.Current.Items["UnifiedDataService"];
                if (service == null)
                {
                    service = new UnifiedDataService(this);
                    HttpContext.Current.Items["UnifiedDataService"] = service;
                }
                return service;
            }
        }

        public UserGroupsService UserGroupsService
        {
            get
            {
                var service = (UserGroupsService)HttpContext.Current.Items["UserGroupsService"];
                if (service == null)
                {
                    service = new UserGroupsService(this);
                    HttpContext.Current.Items["UserGroupsService"] = service;
                }
                return service;
            }
        }

        public UsersService UsersService
        {
            get
            {
                var service = (UsersService)HttpContext.Current.Items["UsersService"];
                if (service == null)
                {
                    service = new UsersService(this);
                    HttpContext.Current.Items["UsersService"] = service;
                }
                return service;
            }
        }

        public RegisterRecordsService RegisterRecordsService 
        {
            get
            {
                var service = (RegisterRecordsService)HttpContext.Current.Items["RegisterRecordsService"];
                if (service == null)
                {
                    service = new RegisterRecordsService(this);
                    HttpContext.Current.Items["RegisterRecordsService"] = service;
                }
                return service;
            }
        }

        public AreasService AreasService
        {
            get
            {
                var service = (AreasService)HttpContext.Current.Items["AreasService"];
                if (service == null)
                {
                    service = new AreasService(this);
                    HttpContext.Current.Items["AreasService"] = service;
                }
                return service;
            }
        }

        public AreaGroupsService AreaGroupsService
        {
            get
            {
                var service = (AreaGroupsService)HttpContext.Current.Items["AreaGroupsService"];
                if (service == null)
                {
                    service = new AreaGroupsService(this);
                    HttpContext.Current.Items["AreaGroupsService"] = service;
                }
                return service;
            }
        }

        public WebServicesService WebServicesService
        {
            get
            {
                var service = (WebServicesService)HttpContext.Current.Items["WebServicesService"];
                if (service == null)
                {
                    service = new WebServicesService(this);
                    HttpContext.Current.Items["WebServicesService"] = service;
                }
                return service;
            }
        }

        public ImportService ImportService
        {
            get
            {
                var service = (ImportService)HttpContext.Current.Items["ImportService"];
                if (service == null)
                {
                    service = new ImportService(this);
                    HttpContext.Current.Items["ImportService"] = service;
                }
                return service;
            }
        }

        public ISmartRegistryContext CreateNewContext()
        {
            var result = new SmartRegistryWebContext(new NHibernateDbContext(), _currentUserId, _adminBodyId);
            return result;
        }

        public IAdministrativeRegister GetAdministrativeRegister()
        {
            return new AdministrativeRegisterWrapper();
        }

    }
}