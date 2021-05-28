using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace IdleLootQuest
{
    class Train
    {
        public double[] mult = new double[] { 0, 0, 0 };
        public int gold = 1, level = 0, it_count = 3, startgold = 1;
        public bool isTrained = false, craft = false, train = false, scrolls = false, golddrop = true;
        public int[] fames = new int[] { 0, 0, 0 };// goldFame, questSpeedFame, rarityFame;       
        Label[] labels = new Label[4]; //labelGold, labelQuestSpeed, labelRarity, labelLvl;
        Button[] buttons = new Button[4]; //buttonGold, buttonQuestSpeed, buttonRarity;
        Label desc;
        public void Training(Grid work)
        {
            // row 7 - 15 column 0 - 9
            // Gold
            lab_create("GOLD", 11, 3, 0, work);
            but_create(11, 5, 0, work);
            // Quest speed
            lab_create("QUEST SPEED", 13, 3, 1, work);
            but_create(13, 5, 1, work);
            // Rarity
            lab_create("RARITY", 9, 3, 2, work);
            but_create(9, 5, 2, work);
            // Lvl
            //lab_create("LEVEL", 7, 3, 3, work);
            //but_create(7, 5, 3, work);
            //lvlCount();  
            l_lvl(7, 0, work);
            //
            // Desc
            //
            desc = new Label();
            desc.Content = "Level 0: 3 items\nLevel 1: 4 items and\nscrolls\nLevel 2: 6 items\nLevel 3: craft\nLevel 4: train\nLevel 5: no gold drop\nafter equip";
            Grid.SetColumn(desc, 0);
            Grid.SetRow(desc, 9);
            Grid.SetRowSpan(desc, 4);
            Grid.SetColumnSpan(desc, 3);
            work.Children.Add(desc);
        }
        void l_lvl(int row, int column, Grid work)
        {
            Label lab = new Label();
            lab.Content = "LEVEL";
            Grid.SetColumn(lab, column);
            Grid.SetRow(lab, row);
            Grid.SetColumnSpan(lab, 2);
            labels[3] = lab;
            work.Children.Add(labels[3]);

            Button but = new Button();
            but.Content = 1000 + (level * 1000);
            //but.Tag = tag;
            Grid.SetColumn(but, column);
            Grid.SetRow(but, row + 1);
            Grid.SetColumnSpan(but, 2);
            but.Click +=
                delegate (object sender, System.Windows.RoutedEventArgs e)
                {
                    if (gold >= lvlCount() & level < 4)
                    {
                        level++;
                        gold = startgold;
                        isTrained = true;
                    }
                };
            buttons[3] = but;
            work.Children.Add(buttons[3]);
        }
        public int lvlCount()
        {
            int num = (200 * level) * level + 200;
            switch(level)
            {
                case 0:
                    lvling(3, false, false, false, true);
                    //it_count = 3;
                    //train = false;
                    //craft = false;
                    //scrolls = false;
                    //golddrop = true;
                    break;
                case 1:
                    // 1 - ring and scrolls
                    lvling(4, false, false, true, true);
                    //it_count = 4;
                    //train = false;
                    //craft = false;
                    //scrolls = true;
                    //golddrop = true;
                    break;
                case 2:
                    // 2 - all items
                    lvling(6, false, false, true, true);
                    //it_count = 6;
                    //train = false;
                    //craft = false;
                    //scrolls = true;
                    //golddrop = true;
                    break;
                case 3:
                    // 3 - craft
                    lvling(6, false, true, true, true);
                    //it_count = 6;
                    //train = false;
                    //craft = true;
                    //scrolls = true;
                    //golddrop = true;
                    break;
                case 4:
                    // 4 - train
                    lvling(6, true, true, true, true);
                    //it_count = 6;
                    //train = true;
                    //craft = true;
                    //scrolls = true;
                    //golddrop = true;
                    break;
                case 5:
                    // 5 - gold-  
                    lvling(6, true, true, true, false);
                    //it_count = 6;
                    //train = true;
                    //craft = true;
                    //scrolls = true;
                    //golddrop = false;
                    break;  
            }
            return num;
        }
        void lvling(int _it_count, bool _train, bool _craft, bool _scrolls, bool _golddrop)
        {
            it_count = _it_count;
            train = _train;
            craft = _craft;
            scrolls = _scrolls;
            golddrop = _golddrop;
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int tag = Convert.ToInt32(b.Tag);
            if (fames[tag] < gold & level >= 3)
            {
                fames[tag] = gold;
                mult[tag] = Math.Sqrt(Convert.ToDouble(fames[tag]) / 10000);
                gold = startgold;
            }
        }
        public void Update(Constants c)
        {
            for (int i = 0; i < fames.Length; i++)
            {
                buttons[i].Content = fames[i];
                if (fames[i] < gold && train)
                {
                    buttons[i].Background = Brushes.Yellow;                  
                }
                else
                {
                    buttons[i].Background = Brushes.Gray;
                }
            }
            lvlUpd(c);
        }
        void lvlUpd(Constants c)
        {
            labels[3].Content = "Level " + level;
            buttons[3].Content = (level < c.maxlvl) ? lvlCount().ToString() : "maxLvl";
            //throw new NotImplementedException();
            if (gold >= lvlCount())
                buttons[3].Background = Brushes.Yellow;
            else
                buttons[3].Background = Brushes.Gray;
        }
        void lab_create(string name, int row, int column, int tag, Grid work)
        {
            Label lab = new Label();
            lab.Content = name;
            Grid.SetColumn(lab, column);
            Grid.SetRow(lab, row);
            Grid.SetColumnSpan(lab, 2);
            labels[tag] = lab;
            work.Children.Add(labels[tag]);
        }
        void but_create(int row, int column, int tag, Grid work)
        {
            Button but = new Button();
            but.Content = fames[tag];
            but.Tag = tag;
            Grid.SetColumn(but, column);
            Grid.SetRow(but, row);
            Grid.SetColumnSpan(but, 2);
            but.Click += Button_Click;
            buttons[tag] = but;
            work.Children.Add(buttons[tag]);
        }
        public void Remove(Grid work)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                work.Children.Remove(labels[i]);
                work.Children.Remove(buttons[i]);
            }
            work.Children.Remove(desc);
        }
    }
}
