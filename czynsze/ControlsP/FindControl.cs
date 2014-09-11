using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;

namespace czynsze.ControlsP
{
    public static class FindControl
    {
        public static Control Recursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID)
                return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn = Recursive(controlToSearch, controlID);

                if (controlToReturn != null)
                    return controlToReturn;
            }

            return null;
        }
    }
}