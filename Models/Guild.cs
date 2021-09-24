using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_OOP
{
    class Guild
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

        public Guild(string p, string t)
        {
            Path = p;
            Text = t;
        }
    }
}
