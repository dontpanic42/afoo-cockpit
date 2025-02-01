using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Gui
{
    class FormValidationException : Exception
    {
        public FormValidationException(string message) : base(message) { }
    }
}
