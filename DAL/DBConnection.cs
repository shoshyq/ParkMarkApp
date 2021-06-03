using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class DBConnection
    {
        ParkMarkEntitiesDAL parkMarkmodels;
        public DBConnection()
        {
            parkMarkmodels = new ParkMarkEntitiesDAL();
        }
        #region generic functions for all tables
        public List<T> GetDbSet<T>() where T : class
        {
            using (ParkMarkEntitiesDAL pmm = new ParkMarkEntitiesDAL())
            {
                return pmm.Set<T>().ToList();
            }
        }
  
        public enum ExecuteActions
        {
            Insert,
            Update,
            Delete
        }

        public void Execute<T>(T entity, ExecuteActions action) where T : class
        {
            using (ParkMarkEntitiesDAL pmm = new ParkMarkEntitiesDAL())
            {
                var model = pmm.Set<T>();
                switch (action)
                {
                    case ExecuteActions.Insert:
                        model.Add(entity);
                        break;
                    case ExecuteActions.Update:
                        model.Attach(entity);
                        pmm.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                        break;
                    case ExecuteActions.Delete:
                        model.Remove(entity);
                        pmm.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                        break;
                    default:
                        break;
                }
                pmm.SaveChanges();
            }
        }
        #endregion

        public int GetUserCode(string userName, string password)
        {
            if (GetDbSet<User>().Any(u => u.Username == userName && u.UserPassword == password))
                return GetDbSet<User>().First(u => u.Username == userName && u.UserPassword == password).Code;
            return 0;
        }

    }

}
