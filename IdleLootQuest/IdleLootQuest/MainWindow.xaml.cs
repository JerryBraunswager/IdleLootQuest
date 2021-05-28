using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace IdleLootQuest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model m;
        DispatcherTimer timer = new DispatcherTimer();
        System.Windows.Forms.NotifyIcon ni;
        string path;
        DateTime start;
        public MainWindow()
        {
            InitializeComponent();
            start = DateTime.Now.AddMinutes(1);
            m = new Model(workspace);
            path = AppDomain.CurrentDomain.BaseDirectory + @"Materials\Save.txt";
            StreamReader sr;
            bool find = true;
            try
            {
                sr = new StreamReader(path, Encoding.Default);
            }
            catch
            {
                find = false;
            }
            if(find)
            {
                sr = new StreamReader(path, Encoding.Default);
                List<string> read = new List<string>();
                string temp = sr.ReadLine();
                while (temp != null)
                {
                    read.Add(temp);
                    temp = sr.ReadLine();
                }
                m.Read(read, labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots);
                sr.Close();
            }
            goldLine.Width = 0;
            locLine.Width = 0;
            NotifyIcon();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        void NotifyIcon()
        {
            ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + @"Materials\Icon.ico");
            ni.Visible = false;
            ni.Click +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    ni.Visible = false;
                };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            m.chanceUpdate(labelGold, labelAgi, labelPow, labelPer, labelRarity, labelQSpeed, labelMF);
            m.trainWork(labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots);
            m.progressbarWork(goldLine, locLine, labelFame, labelLocProgress, labelLocDoing, workspace);
            if (m.autosell)
            {
                m.Sell();
            }
            if(m.autoequip)
                m.Equip(labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots);
            m.scrollActivating(status);
            if (DateTime.Now >= start)
            {
                //throw new NotImplementedException();
                StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
                string[] writing = m.Write();
                for (int i = 0; i < writing.Length; i++)
                    sw.WriteLine(writing[i]);
                sw.Close();
                start = DateTime.Now.AddMinutes(1);
            }
        }

        private void buttonEquip_Click(object sender, RoutedEventArgs e)
        {
            if (m.inv)
            {
                m.Equip(labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots);
            }    
            if(m.craft)
            {
                m.Equip_craft(labelHelmet, labelBreastplate, labelSword, labelRing, labelGlothes, labelBoots);
            }        
        }

        private void buttonNormal_Click(object sender, RoutedEventArgs e)
        {
            if (!m.autoequip)
            {
                m.autoequip = true;
                buttonNormal.Background = Brushes.Red;
            }
            else
            {
                m.autoequip = false;
                buttonNormal.Background = Brushes.Yellow;
            }
        }

        private void buttonEasy_Click(object sender, RoutedEventArgs e)
        {
            if (!m.autosell)
            {
                m.autosell = true;
                buttonEasy.Background = Brushes.Red;
            }
            else
            {
                m.autosell = false;
                buttonEasy.Background = Brushes.Yellow;
            }
        }

        private void buttonSell_Click(object sender, RoutedEventArgs e)
        {
            if (m.inv)
            {
                m.Sell();
                m.Remove();
                m.Inventory();
            }
            if(m.craft)
            {
                m.Sell_craft();
            }
        }

        private void buttonLoot_Click(object sender, RoutedEventArgs e)
        {
            m.Loot();
            m.inv = true;
            m.train = false;
            m.craft = false;
            m.Inventory();
        }

        private void buttonTrain_Click(object sender, RoutedEventArgs e)
        {
            m.inv = false;
            m.train = true;
            m.Remove();
            m.Train();
        }

        private void buttonCraft_Click(object sender, RoutedEventArgs e)
        {
            if (m._craft())
            {
                m.inv = false;
                m.craft = true;
                m.Remove();
                m.Craft();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // helmet breastplate sword ring gloves boots 3 train
            StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
            string[] writing = m.Write();
            for (int i = 0; i < writing.Length; i++)
                sw.WriteLine(writing[i]);
            sw.Close();
        }

        private void buttonQuickLoot_Click(object sender, RoutedEventArgs e)
        {
            m.QuickLoot();
            //m.GoldAdd();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
                ni.Visible = true;
            }
        }

        
    }
}
