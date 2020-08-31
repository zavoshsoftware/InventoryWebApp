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
            InitialRoles(databaseContext);
           
        }

        #region Role
        public static void InitialRoles(DatabaseContext databaseContext)
        {
            InsertRole("397cfc9f-251c-48b9-91d7-1836ec64c28e", "superAdministrator", "راهبر ویژه", databaseContext);
            InsertRole("22b9c114-9b17-43df-9cfa-d925ff30a74e", "administrator", "راهبر", databaseContext);
            InsertRole("c6b7338a-f0bc-4296-9d33-4fb719242d8a", "operator", "اوپراتور", databaseContext);
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

    


    }
}
