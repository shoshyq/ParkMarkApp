using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Convert
{
    public static class FeedbackConvert
    {
        public static DAL.Feedback ConvertFeedbackToEF(Entities.Feedback f)
        {
            return new DAL.Feedback
            {
                Code = f.Code,
                DescriberUserCode = f.DescriberUserCode,
                DescriptedUserCode = f.DescriptedUserCode,
                Feedback1 = f.Feedback1,
                Rating = f.Rating

            };
        }
        public static Entities.Feedback ConvertFeedbackToEntity(DAL.Feedback f)
        {
            return new Entities.Feedback
            {
                Code = f.Code,
                DescriberUserCode = f.DescriberUserCode,
                DescriptedUserCode = f.DescriptedUserCode,
                Feedback1 = f.Feedback1,
                Rating = f.Rating
            };
        }
        public static List<Entities.Feedback> ConvertFeedbacksListToEntity(IEnumerable<DAL.Feedback> feedbacks)
        {
            return feedbacks.Select(p => ConvertFeedbackToEntity(p)).OrderBy(n => n.Code).ToList();
        }

    }
}
