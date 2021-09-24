using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KP_OOP
{
    class User
    {
        private string path;
        private string text;
        private Color isHeadIndicator;
        private bool isHead;

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

        public bool IsHead
        {
            get { return isHead; }
            set
            {
                isHead = value;
                if(value)
                    isHeadIndicator = Colors.Red;
                else
                    isHeadIndicator = Colors.Transparent;
            }
        }

        public Color IsHeadIndecator
        {
            get { return isHeadIndicator; }
            set
            {
                isHeadIndicator = value;
            }
        }
        
        public User(string p, string t, bool isadmin = false)
        {
            Path = p;
            Text = t;
            IsHead = isadmin;
        }
    }
}
