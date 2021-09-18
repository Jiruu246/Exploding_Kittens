using System;

namespace ExplodingKittenLib.Numbers
{
    [Serializable]
    public class Numbers
    {
        private int _i;

        public Numbers(int i)
        {
            _i = i;
        }

        public int Get
        {
            get
            {
                return _i;
            }
            set
            {
                _i = value;
            }
        }
    }
}
