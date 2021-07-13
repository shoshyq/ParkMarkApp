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
        List<Feedback> flst = DAL.Convert.FeedbackConvert.ConvertFeedbacksListToEntity((IEnumerable<DAL.Feedback>)GetAll<Feedback>());
        public FeedbackBL()//
        {
            ubl = new UserBL();
        }
        // adding new feedback. returns code if succeeds
        public int AddFeedback(Feedback f)
        {
            AddSet(DAL.Convert.FeedbackConvert.ConvertFeedbackToEF(f));//
            ubl.checkUserAvRating((int)f.DescriptedUserCode);
            if (flst.Any(g => g.Code == f.Code))
                return flst.First(g => g.Code == f.Code).Code;
            return 0;


        }
        // updating a feedback. returns code if succeeds
        public int UpdateFeedback(int usercode,Feedback f)
        {

            UpdateSet(DAL.Convert.FeedbackConvert.ConvertFeedbackToEF(f));//
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
                        DeleteSet(DAL.Convert.FeedbackConvert.ConvertFeedbackToEF(item));
                    }
                }

            }
            return 0;
        }
        //deletes feedback
        public int DeleteFeedback(Feedback f)
        {

            var l  = flst.First(i => i.Code == f.Code);
            DeleteSet(DAL.Convert.FeedbackConvert.ConvertFeedbackToEF(l));
            return 0;
        }
        //gets all descripted users' feedbacks by usercode
        public List<Feedback> GetAllFeedbacksByUser(int usercode)
        {
            return flst.Where(f => f.DescriptedUserCode == usercode).ToList();
        }


    }
}