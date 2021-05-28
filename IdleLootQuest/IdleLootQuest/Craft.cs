using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IdleLootQuest
{
    class Craft
    {
        public int[] helms = new int[] { 0, 0 };
        public int[] swords = new int[] { 0, 0 };
        public int[] rings = new int[] { 0, 0 };
        public int[] boots = new int[] { 0, 0 };
        public int[] breasts = new int[] { 0, 0 };
        public int[] glothess = new int[] { 0, 0 };
        public bool isCrafted = false, isSpawned = false;
        public Item crafted;
        Label[] labels = new Label[6];
        Button[] buttons = new Button[6];
        Image[] images = new Image[6];
        ImageSourceConverter imgs = new ImageSourceConverter();
        int choose = 0;
        string letter;
        Button[] works = new Button[2];
        public void Crafting(Grid work, int lvl)
        {
            // row 7 - 15 column 0 - 9
            img_create("Helmet", 8, 0, 0, work);
            lab_create(helms, 8, 1, 0, work);
            but_create(8, 3, 0, work);

            img_create("Breastplate", 9, 0, 1, work);
            lab_create(breasts, 9, 1, 1, work);
            but_create(9, 3, 1, work);

            img_create("Sword", 10, 0, 2, work);
            lab_create(swords, 10, 1, 2, work);
            but_create(10, 3, 2, work);
            if (lvl > 0)
            {
                img_create("Ring", 11, 0, 3, work);
                lab_create(rings, 11, 1, 3, work);
                but_create(11, 3, 3, work);
            }
            if (lvl > 1)
            {
                img_create("Gloves", 12, 0, 4, work);
                lab_create(glothess, 12, 1, 4, work);
                but_create(12, 3, 4, work);

                img_create("Boots", 13, 0, 5, work);
                lab_create(boots, 13, 1, 5, work);
                but_create(13, 3, 5, work);
            }
            //if(isCrafted & !isSpawned)
            works[0] = new Button();
            works[0].Content = "Translate\n2000 = 1";
            Grid.SetColumn(works[0], 5);
            Grid.SetRow(works[0], 8);
            Grid.SetColumnSpan(works[0], 2);
            works[0].Tag = 6;
            works[0].Click += Button_Click;
            work.Children.Add(works[0]);

            works[1] = new Button();
            works[1].Content = "Craft";
            Grid.SetColumn(works[1], 8);
            Grid.SetRow(works[1], 8);
            Grid.SetColumnSpan(works[1], 2);
            works[1].Tag = 7;
            works[1].Click += Button_Click;
            work.Children.Add(works[1]);
            // row 7 - 15 column 0 - 9
        }
        public void Parce(Item it, int numdiv)
        {
            int number = 0;
            int letter = 0;
            if (it.number < 100)
                number += Convert.ToInt32(it.number);
            else
            {
                int num = (it.icolor > 0) ? numdiv / it.icolor : numdiv * 2;
                number += Convert.ToInt32(it.number / num);
            }
            letter += it.letter / 2;
            switch (it.name)
            {
                case "Helmet":
                    helms[0] += letter;
                    helms[1] += number;
                    break;
                case "Breastplate":
                    breasts[0] += letter;
                    breasts[1] += number;
                    break;
                case "Sword":
                    swords[0] += letter;
                    swords[1] += number;
                    break;
                case "Ring":
                    rings[0] += letter;
                    rings[1] += number;
                    break;
                case "Gloves":
                    glothess[0] += letter;
                    glothess[1] += number;
                    break;
                case "Boots":
                    boots[0] += letter;
                    boots[1] += number;
                    break;
            }
            //letter number icolor
        }
        public void Update()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null)
                {
                    if (choose == i)
                    {
                        buttons[i].Content = "Use";
                    }
                    else
                        buttons[i].Content = "";
                }
                if(labels[i] != null)
                    labels[i].Content = items(i)[0] + "       " + items(i)[1];
            }
        }
        public void Remove(Grid work)
        {
            for (int i = 0; i < images.Length; i++)
            {
                work.Children.Remove(labels[i]);
                work.Children.Remove(buttons[i]);
                work.Children.Remove(images[i]);
                if (i < 2)
                    work.Children.Remove(works[i]);
            }
            if (crafted != null)
            {
                crafted.Remove(work);
                isSpawned = false;
            }
            //work.Children.Remove(crafted);
        }
        public void itSPWN(Grid work, ItemsManager im)
        {
            letter = im.letter(crafted.letter);
            crafted.Create(work, 9, 7, false, null, letter);
            isSpawned = true;
        }
        public void crftd_Parse(Grid work, int numdiv)
        {
            Parce(crafted, numdiv);
            crafted.Remove(work);
            crafted = null;
            isCrafted = false;
            isSpawned = false;
        }
        public int crftd_Equip(Grid work, Item[] equips, Label[] labels)
        {
            bool lettermore = (equips[choose].letter < crafted.letter) ? true : false;
            bool lettersim = (equips[choose].letter == crafted.letter & equips[choose].number < crafted.number) ? true : false;
            if(lettermore | lettersim)
            {
                crafted.Equip(work, labels[choose], letter);
                //crafted = null;
                isCrafted = false;
                isSpawned = false;
                return choose;
                //crafted = null;
                //letter = null;
            }
            return -1;
        }
        Item it_choose(int num)
        {
            switch (num)
            {
                case 0:
                    return new Helmet();
                case 1:
                    return new Breastplate();
                case 2:
                    return new Sword();
                case 3:
                    return new Ring();
                case 4:
                    return new Gloves();
                case 5:
                    return new Boots();
            }
            return null;
        }
        int[] items(int num)
        {
            switch(num)
            {
                case 0:
                    return helms;
                case 1:
                    return breasts;
                case 2:
                    return swords;
                case 3:
                    return rings;
                case 4:
                    return glothess;
                case 5:
                    return boots;
            }
            return null;
        }
        void img_create(string name, int row, int column, int i, Grid work)
        {
            Image it = new Image();
            string imPath = AppDomain.CurrentDomain.BaseDirectory + @"Materials\" + name.ToLower() + ".png";
            it.Source = imgs.ConvertFromString(imPath) as ImageSource;
            Grid.SetColumn(it, column);
            Grid.SetRow(it, row);
            images[i] = it;
            work.Children.Add(images[i]);
        }
        void lab_create(int[] items, int row, int column, int tag, Grid work)
        {
            Label lab = new Label();
            lab.Content = items[0] + "       " + items[1];
            Grid.SetColumn(lab, column);
            Grid.SetRow(lab, row);
            Grid.SetColumnSpan(lab, 2);
            labels[tag] = lab;
            work.Children.Add(labels[tag]);
        }
        void but_create(int row, int column, int tag, Grid work)
        {
            Button but = new Button();
            //but.Content = fames[tag];
            but.Tag = tag;
            Grid.SetColumn(but, column);
            Grid.SetRow(but, row);
            //Grid.SetColumnSpan(but, 2);
            but.Click += Button_Click;
            buttons[tag] = but;
            work.Children.Add(buttons[tag]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if(Convert.ToInt32(b.Tag) < 6)
                choose = Convert.ToInt32(b.Tag);
            else
            {
                int num = Convert.ToInt32(b.Tag);
                switch (num)
                {
                    case 6:
                        {
                            if(items(choose)[1] > 2000)
                            {
                                items(choose)[1] -= 2000;
                                items(choose)[0]++;
                            }
                            break;
                        }
                    case 7:
                        {
                            if (!isCrafted)
                            {
                                crafted = it_choose(choose);
                                crafted.Spawn(items(choose)[0], (items(choose)[1] > 999) ? 999 : items(choose)[1], 1);
                                items(choose)[0] = 0;
                                items(choose)[1] = 0;
                                isCrafted = true;
                            }
                            break;
                        }
                }
            }
        }

        
    }
}
