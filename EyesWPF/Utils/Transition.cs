using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EyesWPF.Model;

namespace EyesWPF.Utils
{
    class Transition
    {
        public static Frame MainFrame { get; set; }

        private static AgentsEntities _context;
        public static AgentsEntities Context
        { 
            get
            {
                if (_context == null)
                    _context = new AgentsEntities();

                return _context;
            }
        }
    }
}
