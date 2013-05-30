// ----------------------------------------------------------------------
// <copyright file="InvokeControlTapActionCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls.Primitives;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    using System;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;

    /// <summary>
    /// The invoke control tap action command.
    /// </summary>
    public partial class InvokeControlTapActionCommand
    {
        /// <summary>
        /// The do implementation.
        /// </summary>
        /// <exception cref="TestAutomationException">
        /// No automation peer found for  + element.GetType().FullName
        /// or
        /// No invoke pattern found for  + element.GetType().FullName
        /// </exception>
        protected override void DoImpl()
        {
            var element = GetUIElement();
            if (element == null)
            {
                SendNotFoundResult("Couldn't find element " + this.AutomationIdentifier.ElementName);
                return;
            }

            // automate the click
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
            if (peer == null)
            {
                throw new TestAutomationException("No automation peer found for " + element.GetType().FullName);
            }

            if (TryTogglePatternAutomation(peer, element) || TryInvokePatternAutomation(peer))
            {
                SendSuccessResult();
                return;
            }

            throw new TestAutomationException("No invoke pattern found for " + element.GetType().FullName);
        }

        /// <summary>
        /// Try the invoke pattern on the automation peer.
        /// </summary>
        /// <param name="peer">The peer.</param>
        /// <returns>
        /// True if the pattern was available and succeeded, else false.
        /// </returns>
        /// <exception cref="TestAutomationException">Thrown if the pattern will invoke, but throws an exception.</exception>
        private static bool TryInvokePatternAutomation(AutomationPeer peer)
        {
            var pattern = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            if (pattern == null)
            {
                return false;
            }

            try
            {
                pattern.Invoke();
            }
            catch (Exception exception)
            {
                throw new TestAutomationException("Exception while invoking pattern", exception);
            }

            return true;
        }

        /// <summary>
        /// Tries the toggle pattern automation.
        /// </summary>
        /// <param name="peer">The peer.</param>
        /// <param name="element">The element.</param>
        /// <returns>
        /// True if the pattern was available and succeeded, else false.
        /// </returns>
        /// <exception cref="TestAutomationException">Exception while invoking pattern</exception>
        private static bool TryTogglePatternAutomation(AutomationPeer peer, UIElement element)
        {
            var pattern = peer.GetPattern(PatternInterface.Toggle) as IToggleProvider;
            if (pattern == null)
            {
                return false;
            }

            try
            {
                pattern.Toggle();
            
                // Toggle won't fire the command, so do that manually!
                var te = element as ToggleButton;

                if (te != null && te.Command != null)
                {
                    te.Command.Execute(te.CommandParameter);
                }

            }
            catch (Exception exception)
            {
                throw new TestAutomationException("Exception while invoking pattern", exception);
            }

            return true;
        }
    }
}