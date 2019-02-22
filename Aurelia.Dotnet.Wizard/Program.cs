using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurelia.Dotnet.Wizard
{
    class Program
    {
        public static void Main(string[] args)
        {
            var form = new ProjectWizardForm();
            form.ShowDialog();
        }
    }
}
