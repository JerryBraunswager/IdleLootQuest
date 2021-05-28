using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IdleLootQuest
{
    class Model
    {
        double goldPercent = 0, locPercent = 0;
        double questSpeedMult = 1.00, rarityMult = 1.00, mfMult = 1.00, goldMult = 1.00;
        int locStage = 1, locDo = 0;
        string agility, persuation, power;
        string[] locDoes = new string[] { "Fighting", "Searching", "Looting" };
        public bool inv = true, train = false, craft = false, autosell = false, autoequip = false;
        Randomizer rand = new Randomizer();
        Train t = new Train();
        Scrolls s = new Scrolls();
        ItemsManager im = new ItemsManager();
        List<Item> items = new List<Item>();
        Constants c = new Constants();
        Craft cr = new Craft();
        Grid work;
        Item[] equips = new Item[] { new Helmet(), new Breastplate(), new Sword(), new Ring(), new Gloves(), new Boots()};
        // sword = POW breastplate = PER ring = RAR helmet = AGI
        public void GoldAdd()
        {
            t.gold += 200;
        }
        public Model(Grid _work)
        {
            work = _work;
        }
        public string[] Write()
        {
            string[] result = new string[11];
            for(int i = 0; i < equips.Length; i++)
            {
                result[i] = equips[i].letter + " " + equips[i].number + ";" + equips[i].icolor;
            }
            for(int i = 0; i < t.fames.Length; i++)
            {
                result[equips.Length + i] = t.fames[i].ToString();
            }
            result[9] = t.level.ToString();
            result[10] = t.gold.ToString();
            return result;
            
        }
        void equipsWrite(int i, int letter, double number, int icolor, Label labelHelmet, Label labelBreastplate, Label labelSword, Label labelRing, Label labelGlothes, Label labelBoots)
        {
            if (number != 0)
            {
                Label[] labels = new Label[] { labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots };
                equips[i].Spawn(letter, number, icolor);
                equips[i].Equip(work, labels[i], im.letter(letter));
            }
        }
        public void Read(List<string> read, Label labelHelmet, Label labelBreastplate, Label labelSword, Label labelRing, Label labelGlothes, Label labelBoots)
        {
            for(int i = 0; i < read.Count; i++)
            {
                if(i < equips.Length)
                {
                    string[] stats = read[i].Split(' ');
                    string[] stats2 = stats[1].Split(';');
                    int num = 0;
                    double num1 = 0;
                    if(int.TryParse(stats[0], out num) & double.TryParse(stats2[0], out num1))
                        equipsWrite(i, Convert.ToInt32(stats[0]), Convert.ToDouble(stats2[0]), Convert.ToInt32(stats2[1]), labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots);
                }
                int temp = 0;
                if(i >= equips.Length & i < equips.Length + t.fames.Length & int.TryParse(read[i], out temp))
                {
                    t.fames[i - equips.Length] = Convert.ToInt32(read[i]);
                    t.mult[i - equips.Length] = Math.Sqrt(Convert.ToDouble(t.fames[i - equips.Length]) / 10000);
                }
                if (i == equips.Length + t.fames.Length)
                    t.level = Convert.ToInt32(read[9]);
                if(i == 10)
                    t.gold = Convert.ToInt32(read[10]);
                //throw new NotImplementedException();
            }
        }
        //
        //Buttons
        //
        public bool Equip(Label labelHelmet, Label labelBreastplate, Label labelSword, Label labelRing, Label labelGlothes, Label labelBoots)
        {
            Label[] labels = new Label[] { labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots };
            bool eqp = false;
            for (int i = 0; i < labels.Length; i++)
            {
                Item it = im.Equip(items, work, labels[i], equips[i].name);
                eqp = itCheck(it, i);
            }
            return eqp;
        }
        public bool Equip_craft(Label labelHelmet, Label labelBreastplate, Label labelSword, Label labelRing, Label labelGlothes, Label labelBoots)
        {
            Label[] labels = new Label[] { labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots };
            int num = cr.crftd_Equip(work, equips, labels);
            bool eqp = false;
            if (num > -1)
            {
                equips[num].Remove(work);
                equips[num] = cr.crafted;
                cr.crafted = null;
                eqp = true;
            }
            return eqp;
        }
        public void Train()
        {
            t.Training(work);
            cr.Remove(work);
        }
        public void Loot()
        {
            t.Remove(work);
            cr.Remove(work);
        }
        public void Sell()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].equipable == true & items[i].stroke == items[i].color)
                {
                    items[i].Remove(work);
                    if(t.craft)
                        cr.Parce(items[i], c.craftnumberdiv);
                    else
                        goldAdd(8);
                    items.RemoveAt(i);
                    
                }
            }
            if (inv)
            {
                Remove();
                Inventory();
            }
        }
        public void Sell_craft()
        {
            cr.crftd_Parse(work, c.craftnumberdiv);
        }
        public void QuickLoot()
        {
            if (t.gold > 0)
            {
                itemDrop(true, 0);
                t.gold -= 1;
            }
        }
        public void Craft()
        {
            cr.Crafting(work, t.level);
            t.Remove(work);
        }
        //
        //Inventory
        //
        public void Inventory()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].active & inv)
                {
                    int row = 7;
                    int num = i;
                    while (num > 9)
                    {
                        num -= 10;
                        row++;
                    }
                    items[i].Create(work, row, num, false, null, im.letter(items[i].letter));
                    items[i].active = true;
                }
            }
            im.MaxCheck(items, equips[0], equips[1], equips[2], equips[3], equips[4], equips[5]);
        }
        public void Add(Item i)
        {
            if (items.Count < 100)            
                items.Add(i);           
        }
        public void Remove()
        {
            for(int i = 0; i < items.Count; i++)
            {
                items[i].Remove(work);
            }           
        }
        public void scrollActivating(Label status)
        {
            s.Update(status);
            for(int i = 0; i < items.Count; i++)
            {
                if(items[i].click)
                {
                    s.Activate((Scroll)items[i]);
                    items[i].Remove(work);
                    //throw new NotImplementedException();
                    items.RemoveAt(i);
                    //throw new NotImplementedException();
                    Remove();
                    Inventory();
                }
            }
        }
        //
        //Equip
        //
        bool itCheck(Item it, int i)
        {
            if (it != null)
            {
                items.Remove(it);
                equips[i].Remove(work);
                equips[i] = it;
                if (t.golddrop & s.golddrop)
                    t.gold = t.startgold;
                return true;
            }
            return false;
        }
        //
        //Drop
        //
        void itemDrop(bool newLoc, double chance_reduction)
        {
            int chance = (newLoc) ? 100 : c.itemdropchance;
            while (rand.Chance(chance, 100))
            {
                int num = rand.r.Next(t.it_count);
                Add(im.randItem(num, t.gold, rarityMult, rand, c.itdropmult));
                chance = Convert.ToInt32(chance * chance_reduction);
            }

            int scrl_chance = 1 + Convert.ToInt32(c.scrolldropmult * mfMult);
            while(rand.Chance(scrl_chance, 200) & t.scrolls & newLoc)
            {
                Scroll scr = s.scrSpawn(rand);
                scr.Spawn(0, 0, scr.icolor);
                //throw new NotImplementedException();
                Add(scr);
                scrl_chance -= 2;
            }
            Remove();
            Inventory();
        }
        //
        //Stats update
        //
        public void locWork()
        {
            int[] num_agility = im.statsCheck(equips[0]);
            int[] num_persuation = im.statsCheck(equips[1]);
            int[] num_power = im.statsCheck(equips[2]);
            //
            agility = "" + im.letter(num_agility[0]) + num_agility[1]; // helmet           
            persuation = "" + im.letter(num_persuation[0]) + num_persuation[1]; // breastplate            
            power = "" + im.letter(num_power[0]) + num_power[1]; // sword
            //
            // Rarity
            //
            rarityMult = (equips[3].letter * 1000 + equips[3].number) / 10000 + t.mult[2]; // ring
            rarityMult = (rarityMult < 1) ? (rarityMult + 1) * s.rcm : rarityMult * s.rcm;
            //
            // QuestSpeed
            //
            questSpeedMult = (equips[5].letter * 1000 + equips[5].number) / 10000 +  t.mult[1]; // boots
            questSpeedMult = (questSpeedMult < 1) ? (questSpeedMult + 1) * s.qsm : questSpeedMult * s.qsm;
            //
            // MagicFind
            //
            mfMult = (equips[4].letter * 1000 + equips[4].number) / 10000; // gloves
            mfMult = (mfMult < 1) ? (mfMult + 1) * s.mfm : mfMult * s.mfm;

            goldMult = (1 + t.mult[0]) * s.gbm;
            //throw new NotImplementedException();
            double stats_sum = 900;
            switch(locDo)
            {
                case 0:
                    stats_sum = num_power[0] * 1000 + num_power[1];
                    break;
                case 1:
                    stats_sum = num_persuation[0] * 1000 + num_persuation[1];
                    break;
                case 2:
                    stats_sum = num_agility[0] * 1000 + num_agility[1];
                    break;
            }
            t.startgold = Convert.ToInt32((stats_sum - c.numlocplus * c.startgoldmult) / (c.numlocmult * c.startgoldmult));
            //throw new NotImplementedException();
            // minimum 0.01 maximum 10
            double numLoc = (stats_sum / (c.numlocmult * t.gold + c.numlocplus) + 0.01) * questSpeedMult;
            
            // minimum 3 maximum 60
            double numGold = (stats_sum / (c.numgoldmult * (1 + t.mult[0]) * t.gold + c.numgoldplus)) * s.gbm;
            //throw new NotImplementedException();
            locProgressing(numLoc, numGold);
            if (t.level >= 4)
                s.scr_count = 4;
        }
        //
        //Label work
        //
        public bool _craft()
        {
            return t.craft;
        }
        public void trainWork(Label labelHelmet, Label labelBreastplate, Label labelSword, Label labelRing, Label labelGlothes, Label labelBoots)
        {
            if (train)
            {
                t.Update(c);
                if (t.isTrained)
                {
                    Label[] labels = new Label[] { labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots };
                    
                    for (int i = 0; i < equips.Length; i++)
                    {
                        equips[i].Remove(work);
                        labels[i].Content = "";
                    }
                    equips = new Item[] { new Helmet(), new Breastplate(), new Sword(), new Ring(), new Gloves(), new Boots() };
                    t.isTrained = false;
                }
            }
        }
        public void chanceUpdate(Label labelGold, Label labelAgi, Label labelPow, Label labelPer, Label labelRarity, Label LabelQSpeed, Label labelMF)
        {
            labelGold.Content = "x" + goldMult.ToString("F2");
            labelRarity.Content = "x" + rarityMult.ToString("F2");
            LabelQSpeed.Content = "x" + questSpeedMult.ToString("F2");
            labelMF.Content = "x" + mfMult.ToString("F2");
            labelAgi.Content = agility;
            labelPer.Content = persuation;
            labelPow.Content = power;
            t.lvlCount();
            
            if (craft)
            {
                cr.Update();
                if(cr.isCrafted & !cr.isSpawned)
                {
                    cr.itSPWN(work, im);
                }
            }       
        }
        public void progressbarWork(Rectangle goldLine, Rectangle locLine, Label labelFame, Label labelLocProgress, Label labelLocDoing, Grid work)
        {
            locWork();                  
            goldLine.Width = 243 * (goldPercent / 100);
            locLine.Width = 243 * (locPercent / 100);
            labelFame.Content = t.gold;
            labelLocProgress.Content = locStage + "/5";
            labelLocDoing.Content = locDoes[locDo];
        }
        //
        // time flow
        //
        void locProgressing(double persent1, double persent2)
        {
            if (locPercent <= 100)
            {
                locPercent += persent1;
            }
            if (locPercent > 100)
            {
                locPercent -= 100;
                locStage++;
                locDo = rand.r.Next(0, locDoes.Length);
                goldAdd(persent2);
                if (locStage > 5)
                {
                    itemDrop(true, 0.40);
                    locStage = 1;
                }
                else
                    itemDrop(false, 0.40);
            }
        }
        void goldAdd(double percent)
        {
            if (goldPercent <= 100)
                goldPercent += percent;
            if (goldPercent > 100)
            {
                goldPercent -= 100;
                t.gold++;
            }
        }
    }
}
