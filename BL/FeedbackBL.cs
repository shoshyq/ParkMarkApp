using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;



namespace BL
{
    public class FeedbackBL : DbHandler
    {
        UserBL ubl;
        List<Feedback> flst = (GetAll<Feedback>());
        public FeedbackBL()//DAL.Converts.FeedbackConvert.ConvertFeedbacksListToEntity
        {
            ubl = new UserBL();
        }
        // adding new feedback. returns code if succeeds
        public int AddFeedback(Feedback f)
        {
            AddSet((f));//DAL.Converts.FeedbackConvert.ConvertFeedbackToEF
            ubl.checkUserAvRating((int)f.DescriptedUserCode);
            if (flst.Any(g => g.Code == f.Code))
                return flst.First(g => g.Code == f.Code).Code;
            return 0;


        }
        // updating a feedback. returns code if succeeds
        public int UpdateFeedback(int usercode,Feedback f)
        {

            UpdateSet(f);//(DAL.Converts.FeedbackConvert.ConvertFeedbackToEF
            return flst.First(g => g.Code == f.Code).Code;
        }
        // deleting descripted users' feedbacks by usercode
        public int DeleteFeedbacksByUser(int usercode)
        {
            if (flst != null)
            {
                foreach (var item in flst)
                {
                    if (item.DescriptedUserCode == usercode)
                    {
                        DeleteSet((item));//DAL.Converts.FeedbackConvert.ConvertFeedbackToEF
                    }
                }

            }
            return 0;
        }
        //deletes feedback
        public int DeleteFeedback(Feedback f)
        {

            var l  = flst.First(i => i.Code == f.Code);
            DeleteSet((l));//DAL.Converts.FeedbackConvert.ConvertFeedbackToEF
            return 0;
        }
        //gets all descripted users' feedbacks by usercode
        public List<Feedback> GetAllFeedbacksByUser(int usercode)
        {
            return flst.Where(f => f.DescriptedUserCode == usercode).ToList();
        }


    }
}