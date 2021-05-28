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
    abstract class Item
    {
        Image it = new Image();
        Rectangle rect = new Rectangle();
        Label stats;
        public bool active = false, equipable = true, click = false;
        public Brush stroke, color;
        public int letter, icolor;
        public double number;
        public string name;
        string imPath;
        BitmapImage bi;
        ImageSourceConverter imgs = new ImageSourceConverter();
        Brush _color()
        {
            switch(icolor)
            {
                case 0:
                    //letter = Convert.ToInt32(letter * 0.5);
                    //number = number * 0.5;
                    return Brushes.Gray;
                case 1:
                    //letter = Convert.ToInt32(letter * 1);
                    //number = number * 1;
                    return Brushes.White;
                case 2:
                    //letter = Convert.ToInt32(letter * 1.5);
                    //number = number * 1.5;
                    return Brushes.Yellow;
                case 3:
                    //letter = Convert.ToInt32(letter * 2);
                    //number = number * 2;
                    return Brushes.Red;
            }
            if (number > 1000)
            {
                number -= 1000;
                letter++;
            }
            return Brushes.White;
        }
        public void Spawn(int _letter, double _number, int _icolor)
        {
            if (equipable)
            {
                letter = _letter;
                number = _number; 
            }
            icolor = _icolor;
            color = _color();
            imPath = AppDomain.CurrentDomain.BaseDirectory + @"Materials\" + name.ToLower() + ".png";
        }
        private void It_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!equipable)
            {
                Equip(null, null, null);
            }
        }
        public void Create(Grid work, int row, int column, bool equiped, Label place, string alphabet)
        {
            rect.Width = 44;
            rect.Height = 44;
            rect.Fill = color;
            Grid.SetColumn(rect, column);
            Grid.SetRow(rect, row);
            work.Children.Add(rect);

            active = true;
            it.Width = 43;           
            it.Height = 43;
            it.Margin = new Thickness(1, 1, 0, 0);      
            it.Source = imgs.ConvertFromString(imPath) as ImageSource;          
            Grid.SetColumn(it, column);
            Grid.SetRow(it, row);
            work.Children.Add(it);
                       
            if (!equiped)
            {
                stats = new Label();
                if (equipable)
                {
                    stats.Content = alphabet;
                    if (number < 100)
                        stats.Content += number.ToString("F2");
                    else
                        stats.Content += number.ToString("F0");
                }
                if (!equipable)
                    stats.Content = name;
                stats.Foreground = Brushes.OrangeRed;
                stats.FontFamily = new FontFamily("Showcard Gothic");
                Grid.SetColumn(stats, column);
                Grid.SetRow(stats, row);
                stats.MouseDown += It_MouseDown;
                work.Children.Add(stats);
            }
            if (equiped)
            {
                place.Content = alphabet;
                if (number < 100)
                    place.Content += number.ToString("F2");
                else
                    place.Content += number.ToString("F0");
                rect.Stroke = color;
            }
        }
        public void Remove(Grid work)
        {
            work.Children.Remove(it);
            work.Children.Remove(rect);
            if (stats != null)
            {
                work.Children.Remove(stats);
            }
            active = false;
        }
        public abstract void Equip(Grid work, Label place, string alphabet);
        public void maxStats(bool max)
        {
            switch(max)
            {
                case true:
                    stroke = Brushes.DarkGreen;
                    break;
                case false:
                    stroke = color;
                    break;
            }
            rect.Stroke = stroke;
        }
    }
    class Scroll : Item
    {
        public string line;
        public double mult;
        public int dur;
        //public int icolor;
        //public DateTime start;
        public DateTime end;
        public Scroll(string _name, string _line, double _mult, int _dur, int _icolor)
        {
            equipable = false;
            name = _name;
            line = _line;
            mult = _mult;
            dur = _dur;
            icolor = _icolor;
        }

        public override void Equip(Grid work, Label place, string alphabet)
        {
            click = true;
            end = DateTime.Now.AddSeconds(dur);
        }
    }

    //
    // Equipable
    //
    class Helmet : Item
    {
        public Helmet()
        {
            //color = Brushes.White;
            name = "Helmet";
        }

        public override void Equip(Grid work, Label place, string alphabet)
        {
            Remove(work);
            Create(work, 2, 1, true, place, alphabet);
        }
    }
    class Breastplate : Item
    {
        public Breastplate()
        {
            //color = Brushes.White;
            name = "Breastplate";
        }
        public override void Equip(Grid work, Label place, string alphabet)
        {
            Remove(work);
            Create(work, 3, 1, true, place, alphabet);
        }
    }
    class Sword : Item
    {
        public Sword()
        {
            //color = Brushes.White;
            name = "Sword";
        }
        public override void Equip(Grid work, Label place, string alphabet)
        {
            Remove(work);
            Create(work, 2, 3, true, place, alphabet);
        }
    }
    class Ring : Item
    {
        public Ring()
        {
            //color = Brushes.White;
            name = "Ring";
        }
        public override void Equip(Grid work, Label place, string alphabet)
        {
            Remove(work);
            Create(work, 3, 3, true, place, alphabet);
        }
    }
    class Gloves : Item
    {
        public Gloves()
        {
            //color = Brushes.White;
            name = "Gloves";
        }
        public override void Equip(Grid work, Label place, string alphabet)
        {
            Remove(work);
            Create(work, 2, 5, true, place, alphabet);
        }
    }
    class Boots : Item
    {
        public Boots()
        {
            //color = Brushes.White;
            name = "Boots";
        }
        public override void Equip(Grid work, Label place, string alphabet)
        {
            Remove(work);
            Create(work, 3, 5, true, place, alphabet);
        }
    }
}
