using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_OOP
{
    class LibraryElem
    {
        string text;
        string path;

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

        public LibraryElem(string p, string t)
        {
            Text = t;
            Path = p;
        }
         
    }
}
