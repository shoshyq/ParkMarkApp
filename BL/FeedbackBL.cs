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
    public class FeedbackBL : DbHandler
    {
        UserBL ubl;
        List<Entities.Feedback> flst = DAL.Converts.FeedbackConvert.ConvertFeedbacksListToEntity(GetAll<Feedback>());
        public FeedbackBL()
        {
            ubl = new UserBL();
        }
        // adding new feedback. returns code if succeeds
        public int AddFeedback(Entities.Feedback f)
        {
            AddSet(DAL.Converts.FeedbackConvert.ConvertFeedbackToEF(f));
            ubl.checkUserAvRating((int)f.DescriptedUserCode);
            if (flst.Any(g => g.Code == f.Code))
                return flst.First(g => g.Code == f.Code).Code;
            return 0;


        }
        // updating a feedback. returns code if succeeds
        public int UpdateFeedback(Entities.Feedback f)
        {
            UpdateSet(DAL.Converts.FeedbackConvert.ConvertFeedbackToEF(f));
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
                        DeleteSet(DAL.Converts.FeedbackConvert.ConvertFeedbackToEF(item));
                    }
                }

            }
            return 1;
        }
        //deletes feedback
        public int DeleteFeedback(Feedback f)
        {

            var l  = flst.First(i => i.Code == f.Code);
            DeleteSet(DAL.Converts.FeedbackConvert.ConvertFeedbackToEF(l));
            return 1;
        }
        //gets all descripted users' feedbacks by usercode
        public List<Entities.Feedback> GetAllFeedbacksByUser(int usercode)
        {
            return flst.Where(f => f.DescriptedUserCode == usercode).ToList();
        }


    }
}