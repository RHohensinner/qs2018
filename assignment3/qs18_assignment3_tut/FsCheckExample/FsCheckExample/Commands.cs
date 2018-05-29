using FsCheck;

namespace FsCheckExample
{
    /// <summary>
    /// Command that increments the counter by 1
    /// </summary>
    public class Inc : Command<Counter, int>
    {
        /// <summary>
        /// exercises the SUT
        /// </summary>
        /// <param name="c">current SUT</param>
        /// <returns>new SUT</returns>
        public override Counter RunActual(Counter c)
        {
            //TODO: uncomment to see what happens if SUT behaves wrong
            //if (c.Value < 10)
            c.Inc();
            return c;
        }

        /// <summary>
        /// exercise the model
        /// </summary>
        /// <param name="m">current model value</param>
        /// <returns>new model value</returns>
        public override int RunModel(int m)
        {
            return m + 1;
        }

        /// <summary>
        /// checks the post condition after exercising model and SUT
        /// </summary>
        /// <param name="c">SUT</param>
        /// <param name="m">model</param>
        /// <returns>Post condition as Property</returns>
        public override Property Post(Counter c, int m)
        {
            //post condition has to return Property(therefore convert bool to Property for returning
            return (m == c.Value).ToProperty();
        }

        public override string ToString()
        {
            return "inc"; //used to pretty print command in testoutput
        }
    }

    /// <summary>
    /// Command that decrements the counter by 1
    /// </summary>
    public class Dec : Command<Counter, int>
    {
        //TODO: uncomment to enable precondition(command will not be chosen by Next if Pre is not satisfied
        //public override bool Pre(int m)
        //{
        //    return m > 5;
        //}

        public override Counter RunActual(Counter c)
        {
            //if(c.Value > 10)
            c.Dec();
            return c;
        }

        public override int RunModel(int m)
        {
            return m - 1;
        }

        public override Property Post(Counter c, int m)
        {
            return (m == c.Value).ToProperty();
        }

        public override string ToString()
        {
            return "dec";
        }
    }
}
