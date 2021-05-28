using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleLootQuest
{
    class Scrolls
    {
        public List<Scroll> states = new List<Scroll>();
        public bool golddrop = true;
        public double mfm = 1.00, qsm = 1.00, gbm = 1.00, rcm = 1.00;
        public int scr_count = 5;
        DateTime timer = DateTime.Now;
        public void Activate(Scroll scr)
        {
            switch(scr.line)
            {
                case "Magic Finding":
                    states.Add(scr);
                    mfm = mfm * scr.mult;
                    break;
                case "Questing Speed":
                    states.Add(scr);
                    qsm = qsm * scr.mult;
                    break;
                case "Gold Bonus":
                    states.Add(scr);
                    gbm = gbm * scr.mult;
                    break;
                case "Rarity Chance":
                    states.Add(scr);
                    rcm = rcm * scr.mult;
                    break;
                case "No Gold Drop":
                    //throw new NotImplementedException();
                    states.Add(scr);
                    golddrop = false;
                    break;
            }
        }
        public void Deactivate(Scroll scr)
        {
            switch (scr.line)
            {
                case "Magic Finding":
                    mfm = mfm / scr.mult;
                    break;
                case "Questing Speed":
                    qsm = qsm / scr.mult;
                    break;
                case "Gold Bonus":
                    gbm = gbm / scr.mult;
                    break;
                case "Rarity Chance":
                    rcm = rcm / scr.mult;
                    break;
                case "No Gold Drop":
                    golddrop = true;
                    break;
            }
        }
        public void Update(System.Windows.Controls.Label status)
        {
            //status.Content = "x1.2 Magic Finding for 60";

            //throw new NotImplementedException();
            labelUPD(status);
            if (DateTime.Now >= timer.AddSeconds(1))
            {
                timer = DateTime.Now;
                
                //status.Content = "x1.2 Magic Finding for 60";
                for (int i = 0; i < states.Count; i++)
                {
                    states[i].dur -= 1;
                    //throw new NotImplementedException();
                    if (states[i].dur < 1)
                    {
                        Deactivate(states[i]);
                        states.RemoveAt(i);
                    }
                    
                }
                
            }
        }
        void labelUPD(System.Windows.Controls.Label status)
        {
            status.Content = "";
            for (int i = 0; i < states.Count; i++)
            {
                string start = (i > 0) ? "\n" : "";
                string multip = (states[i].mult > 0) ? "x" + states[i].mult : "";
                double durat = Math.Truncate(states[i].dur / 60.0 * 10) / 10;
                string dur = (states[i].dur > 60) ? durat.ToString("F1") + " min" : states[i].dur + " sec";
                status.Content += start + multip + " " + states[i].line + " for " + dur;
            }
        }
        public Scroll scrSpawn(Randomizer rand)
        {
            //
            // Mult and dur
            //
            string scr_line = "", scr_name = "";
            double mult = 1.15;
            int dur = 60;
            int rarity = 1;
            if (rand.Chance(13, 100))
            {
                mult = 1.05;
                dur = 180;
                rarity = 3;
            }
            else
            {
                if (rand.Chance(25, 100))
                {
                    mult = 1.1;
                    dur = 120;
                    rarity = 2;
                }
                else
                {
                    if(rand.Chance(50, 100))
                    {
                        mult = 1.15;
                        dur = 60;
                        rarity = 1;
                    }
                    else
                    {
                        mult = 1.2;
                        dur = 30;
                        rarity = 0;
                    }
                }
            }
            // 1.05 180|| 1.2 30 || 1.15 60 || 1.1 120
            //
            // Scroll
            //
            int num_scr = rand.Choose(scr_count);
            switch (num_scr)
            {
                case 0:
                    scr_line = "Magic Finding";
                    scr_name = "MF";
                    break;
                case 1:
                    scr_line = "Questing Speed";
                    scr_name = "QS";
                    break;
                case 2:
                    scr_line = "Gold Bonus";
                    scr_name = "Gold";
                    break;
                case 3:
                    scr_line = "Rarity Chance";
                    scr_name = "Rar";
                    break;
                case 4:
                    scr_line = "No Gold Drop";
                    scr_name = "Ngd";
                    mult = 0;
                    break;
            }
            return new Scroll(scr_name, scr_line, mult, dur, rarity);
        }
    }
}
