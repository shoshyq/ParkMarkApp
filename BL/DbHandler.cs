using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BL
{
    public class DbHandler
    {
        static DBConnection dbConnection = new DBConnection();
        #region Database handling
        public DbHandler()
        {
            
        }
        public static int AddSet<T>(T entity) where T : class
        {
            dbConnection.Execute(entity, DBConnection.ExecuteActions.Insert);
            return 1;
            //dBConnection.GetDbSet<entity>();

        }
        public static void UpdateSet<T>(T entity) where T : class
        {
            dbConnection.Execute(entity, DBConnection.ExecuteActions.Update);
        }
        public static void DeleteSet<T>(T entity) where T : class
        {
            dbConnection.Execute(entity, DBConnection.ExecuteActions.Delete);

        }
        public static List<T> GetAll<T>() where T : class
        {
            return dbConnection.GetDbSet<T>();
        }
        public static List<ParkingSpotSearch> GetAllPSS()
        {
            return dbConnection.GetPSSDbSet();
        }

        #endregion
    }
}
