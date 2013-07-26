//  ----------------------------------------------------------------------
//  <copyright file="AutomationIdentifierExtensions.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using WindowsPhoneTestFramework.Client.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    public static class AutomationIdentifierExtensions
    {
        public static string ToIdOrName(this AutomationIdentifier automationIdentifier)
        {
            return string.Format("AutomationName: {0}, ElementName : {1}, DisplayText: {2}",
                                 automationIdentifier.AutomationName,
                                 automationIdentifier.ElementName,
                                 automationIdentifier.DisplayedText);
        }
    }
}