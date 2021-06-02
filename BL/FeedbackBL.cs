using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using Feedback = DAL.Feedback;


namespace BL
{
    public class FeedbackBL
    {
        UserBL ubl;
        DBConnection DBCon;

        public FeedbackBL()
        {
            DBCon = new DBConnection();
            ubl = new UserBL();
        }
        // adding new feedback. returns code if succeeds
        public int AddFeedback(Entities.Feedback f)
        {
            DbHandler.AddSet(f);
            ubl.checkUserAvRating((int)f.DescriptedUserCode);
            if (DbHandler.GetAll<Feedback>().Any(g => g.Code == f.Code))
                return DbHandler.GetAll<Feedback>().First(g => g.Code == f.Code).Code;
            return 0;


        }
        // updating a feedback. returns code if succeeds
        public int UpdateFeedback(Entities.Feedback f)
        {
            DBCon.Execute(f, DBConnection.ExecuteActions.Update);
            return DbHandler.GetAll<Feedback>().First(g => g.Code == f.Code).Code;
        }
        // deleting descripted users' feedbacks by usercode
        public int DeleteFeedbacksByUser(int usercode)
        {
            var flist = DbHandler.GetAll<Feedback>();
            if (flist != null)
            {
                foreach (var item in flist)
                {
                    if (item.DescriptedUserCode == usercode)
                    {
                        DbHandler.DeleteSet(item);
                    }
                }

            }
            return 1;
        }
        //deletes feedback
        public int DeleteFeedback(Feedback f)
        {

            var l = DbHandler.GetAll<Feedback>().First(i => i.Code == f.Code);
            DbHandler.DeleteSet(l);
            return 1;
        }
        //gets all descripted users' feedbacks by usercode
        public List<DAL.Feedback> GetAllFeedbacksByUser(int usercode)
        {
            return DbHandler.GetAll<Feedback>().Where(f => f.DescriptedUserCode == usercode).ToList();
        }


    }
}