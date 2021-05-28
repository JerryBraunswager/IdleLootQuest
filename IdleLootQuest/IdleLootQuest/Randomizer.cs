using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleLootQuest
{
    class Randomizer
    {
        public Random r = new Random();
        public bool Chance(int chance, int max)
        {
            int num = r.Next(0, max);
            if (num > 0 & num <= chance)
                return true;
            return false;
        }
        public int Choose(int count)
        {
            int num = r.Next(0, 100);
            for(int i = 0; i < count; i++)
            {
                if (i * (100 / count) < num & (i + 1) * (100 / count) > num)
                    return i;
            }
            return 0;
        }
    }
}
