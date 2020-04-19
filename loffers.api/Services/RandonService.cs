using System;

namespace loffers.api.Services
{
    public class RandonService
    {
        static Random random;
        static object syncObj = new object();

        public int GenerateRandomNumber(int min, int max)
        {
            lock (syncObj)
            {
                return random.Next(min, max);
            }
        }

        static RandonService()
        {
            Instance = new RandonService();
            random = new Random(10000);
        }

        private RandonService()
        {

        }

        private static RandonService _instance;
        public static RandonService Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
    }
}