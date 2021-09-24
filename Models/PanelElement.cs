using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KP_OOP
{
    public class PanelElement
    {
        private string path;
        private string text;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
            }
        }

        public PanelElement(string p, string t)
        {
            Path = p;
            Text = t;
        }
    }
}
