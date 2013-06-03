using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    /// <summary>
    /// The get visual state command.
    /// </summary>
    public partial class GetCheckedStatusCommand
    {
        #region Methods

        /// <summary>
        /// The do impl.
        /// </summary>
        protected override void DoImpl()
        {
            FrameworkElement element = GetFrameworkElement();

            var checkedProperty = element.GetType().GetProperties().FirstOrDefault(p => p.Name == "IsChecked");

            if (checkedProperty != null)
            {
                var isChecked = checkedProperty.GetValue(element, null);

                try
                {
                    var val = (bool)isChecked;
                    SendTextResult(val.ToString());
                    return;
                }
                catch (InvalidCastException)
                {
                    SendNotFoundResult("Couldn't find the correct property.");
                    return;
                }
            }

            SendNotFoundResult("Couldn't find element.");
        }

        #endregion
    }
}