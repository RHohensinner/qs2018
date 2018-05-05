using System;

namespace SocialActivity
{
    public static class SocialActivityFunction
    {
        public static double getSocialActivityFactor(int follows, int likes, int posts)
        {
            if (follows < 0 || likes < 0 || posts < 0 || follows > 20 || likes > 200 || posts > 13)
            {
                ArgumentLogger.addEntryToList(follows, likes, posts, -1);
                throw new ArgumentException("Invalid arguments supplied!");
            }

            double hoursPerDay = 8.0;
            double daysPerWeek = 5.0;
            double followFactor = (follows + 20) / 40.0;
            double clickFactor = likes * hoursPerDay * followFactor;
            double clickFactorPerDay = clickFactor / daysPerWeek;
            double postsWithPostFactor = posts * 37;

            double result = (clickFactorPerDay + postsWithPostFactor) / hoursPerDay;

            ArgumentLogger.addEntryToList(follows, likes, posts, result);
            return result;
        }
    }
}
