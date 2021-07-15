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
        ParkMarkDBEntities1 parkMarkmodels;
        public DBConnection()
        {
            parkMarkmodels = new ParkMarkDBEntities1();
        }
        #region generic functions for all tables
        public List<T> GetDbSet<T>() where T : class
        {
            using (ParkMarkDBEntities1 pmm = new ParkMarkDBEntities1())
            {
                return pmm.GetDbSet<T>().ToList();
            }
        
        }
        public List<ParkingSpotSearch> GetPSSDbSet()
        {
            using (ParkMarkDBEntities1 pmm = new ParkMarkDBEntities1())
            {
                return pmm.Set<ParkingSpotSearch>().ToList();
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
            using (ParkMarkDBEntities1 pmm = new ParkMarkDBEntities1())
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

      

    }

}
