using System;

namespace Models
{
    internal static class DatabaseContextInitializer
    {
        static DatabaseContextInitializer()
        {

        }

        internal static void Seed(DatabaseContext databaseContext)
        {
            //InitialRoles(databaseContext);
            //InitialInputDetailStatuses(databaseContext);
            InitialManageConfiguration(databaseContext);
        }

        #region Role
        public static void InitialRoles(DatabaseContext databaseContext)
        {
            InsertRole("397cfc9f-251c-48b9-91d7-1836ec64c28e", "superAdministrator", "راهبر ویژه", databaseContext);
            InsertRole("22b9c114-9b17-43df-9cfa-d925ff30a74e", "administrator", "راهبر", databaseContext);
            InsertRole("c6b7338a-f0bc-4296-9d33-4fb719242d8a", "operator", "اپراتور", databaseContext);
        }

        public static void InsertRole(string roleId, string roleName, string roleTitle, DatabaseContext databaseContext)
        {
            Guid id = new Guid(roleId);
            Role role = new Role();
            role.Id = id;
            role.Title = roleTitle;
            role.Name = roleName;
            role.CreationDate = DateTime.Now;
            role.IsActive = true;
            role.IsDeleted = false;

            databaseContext.Roles.Add(role);
            databaseContext.SaveChanges();
        }

        #endregion



        #region InputDetailStatus
        public static void InitialInputDetailStatuses(DatabaseContext databaseContext)
        {
            InsertInputDetailStatus("d470cf2b-b34b-4043-b5a6-4ac6e84ce314",1, "موجود", databaseContext);
            InsertInputDetailStatus("2abc7440-f669-4d8c-8852-7c54f394a830", 2, "در حال بارگیری", databaseContext);
            InsertInputDetailStatus("67c73dd8-d83f-4cdc-8d63-cebbab636510", 3, "ناموجود", databaseContext);
        }

        public static void InsertInputDetailStatus(string id, int code, string title, DatabaseContext databaseContext)
        {
            Guid idGuid = new Guid(id);
            InputDetailStatus inputDetailStatus = new InputDetailStatus()
            {
                CreationDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                Id = idGuid,
                Title = title,
                Code = code,
        };
             

            databaseContext.InputDetailStatuses.Add(inputDetailStatus);
            databaseContext.SaveChanges();
        }

        #endregion

        #region InitialManageConfiguration
        public static void InitialManageConfiguration(DatabaseContext databaseContext)
        {
            InsertManageConfiguration("FF0DCDB7-0D46-4753-A397-C6539F532CB3",9,40000, databaseContext);
        }
        public static void InsertManageConfiguration(string id,int vat,int amount,DatabaseContext databaseContext)
        {
            Guid idGuid = new Guid(id);
            ManageConfiguration manageConfiguration = new ManageConfiguration()
            {
                CreationDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                Id = idGuid,
                VAT = vat,
                Amount = amount,
            };
            databaseContext.ManageConfigurations.Add(manageConfiguration);
            databaseContext.SaveChanges();
        }
        #endregion InitialManageConfiguration
    }
}
