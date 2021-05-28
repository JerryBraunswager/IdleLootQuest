using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace IdleLootQuest
{
    class ItemsManager
    {
        string alphabet = " abcdefghijklmnopqrstuvwxyz";
        
        //public double time = 1;
        //int n_letter;
        
        public Item randItem(int num, int gold, double rarityMult, Randomizer r, double numbermult)
        {
            double[] stats = randStats(gold, rarityMult, r, numbermult);
            Item[] items = new Item[] { new Helmet(), new Breastplate(), new Sword(), new Ring(), new Gloves(), new Boots() };
            Item it = (num < items.Length) ? items[num] : null;
            if (it != null)
            {
                it.Spawn(Convert.ToInt32(stats[0]), stats[1], Convert.ToInt32(stats[2]));
                it.maxStats(false);
            }
            return it;
        }
        public void MaxCheck(List<Item> items, Item helmet, Item breastplate, Item sword, Item ring, Item glothes, Item boots)
        {
            Item[] its = new Item[] { helmet, breastplate, sword, ring, glothes, boots };
            for(int i = 0; i < its.Length; i++)
            {
                Check(its[i].name, items, its[i]);
            }
        }
        void Check(string name, List<Item> items, Item equiped)
        {
            var it_check = from item in items
                        where item.name == name
                        select item;
            Item it = equiped;
            foreach (Item item in it_check)
            {
                item.maxStats(false);
                if (it != null && it.letter == item.letter
                    & it.number < item.number)
                {
                    it = item;
                }
                if (it != null && it.letter < item.letter)
                {
                    it = item;
                }
                if (it == null)
                    it = item;
            }
            if (it != null & it != equiped) 
                it.maxStats(true);
        }
        public int[] statsCheck(Item it, int s_letter = 0, int number = 300, double percent = 0.27)
        {
            if (it != null)
            {
                double preresult = (it.letter * 1000 + it.number) * 0.4;
                double letter_preresult = 0;
                double number_preresult = preresult + s_letter * 1000 + number;
                while(number_preresult > 999)
                {
                    number_preresult -= 1000;
                    letter_preresult++;
                }
                //double number_preresult = Convert.ToInt32(it.number);
                return new int[] { Convert.ToInt32(letter_preresult), Convert.ToInt32(number_preresult)};
            }
            return new int[] { s_letter, number };
        }
        public string letter(int num)
        {
            return alphabet[num].ToString();
        }
        double[] randStats(int gold, double rarityMult, Randomizer r, double numbermult)
        {
            //
            // Stats
            //
            double number = gold / numbermult * r.r.NextDouble();
            int temp = 0;
            while(number >= 1000)
            {
                number -= 1000;
                temp++;
            }
            int letter = (temp > alphabet.Length) ? alphabet.Length : temp;
            //
            // Rarity
            //
            double rarity = 0;
            if (r.Chance(Convert.ToInt32(1 * rarityMult), 100))
            {
                //letter = Convert.ToInt32(letter * 2);
                number = number * 2;
                rarity = 3;
            }
            else
            {
                if(r.Chance(Convert.ToInt32(15 * rarityMult), 100))
                {
                    //letter = Convert.ToInt32(letter * 1.5);
                    number = number * 1.5;
                    rarity = 2;
                }
                else
                {
                    if (r.Chance(Convert.ToInt32(35 * rarityMult), 100))
                    {
                        //letter = Convert.ToInt32(letter * 1);
                        number = number * 1;
                        rarity = 1;
                    }
                    else
                    {
                        //letter = Convert.ToInt32(letter * 0.5);
                        number = number * 0.5;
                        rarity = 0;
                    }
                }
            }
            if(number > 1000)
            {
                number -= 1000;
                letter++;
            }
            letter = (letter > alphabet.Length) ? alphabet.Length : letter;
            double[] result = new double[] { letter, (number < 1000) ? number : 1000, rarity };
            return result;
        }
        //
        // Equip
        //
        public Item Equip(List<Item> items, Grid work, Label label, string name)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].stroke != items[i].color && items[i].name == name)
                {
                    items[i].Equip(work, label, letter(items[i].letter));
                    Item item = items[i];
                    //time = 0;
                    return item;
                }
            }
            return null;
        }
    }
}
