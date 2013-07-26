//  ----------------------------------------------------------------------
//  <copyright file="ControlContainsImageCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Windows.Controls;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class ControlContainsImageCommand
    {
        protected override void DoImpl()
        {
            var element = GetUIElement(true);
            if (element == null)
            {
                return;
            }


            var image = AutomationElementFinder.GetElementProperty<Image>(element, "Content");

            if (image != null)
            {
                if (image.Name == ImageName)
                {
                    SendSuccessResult();
                }
                else
                {
                    SendNotFoundResult(string.Format("ControlContainsImageCommand: Could not find the image {0}",
                                                     ImageName));
                }
            }
        }
    }
}