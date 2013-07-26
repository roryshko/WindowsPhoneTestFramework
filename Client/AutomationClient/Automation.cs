// ----------------------------------------------------------------------
// <copyright file="Automation.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Automation.Peers;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;
using WindowsPhoneTestFramework.Client.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.Client.AutomationClient
{
    public class Automation
    {
        public static readonly Automation Instance = new Automation();

        private bool _initialised;

        public void Initialise(string remoteUrl = "")
        {
            if (_initialised)
                return;

            if (Application.Current.RootVisual == null)
                throw new TestAutomationException("Automation client initialised too early");

            remoteUrl = string.IsNullOrEmpty(remoteUrl) ? BddHostForWindowsPhone8 : remoteUrl;

            var configuration = new Configuration()
                                    {
                                        RemoteUrl =
                                            string.IsNullOrEmpty(remoteUrl) ? Configuration.DefaultRemoteUrl : remoteUrl,
                                        UiDispatcher = Application.Current.RootVisual.Dispatcher
                                    };

            var automationClient = new AutomationClient(configuration);
            automationClient.Start();
            Application.Current.Exit += (sender, args) => automationClient.Stop();

            _initialised = true;
        }

        public void AddStringPropertyNameForTextLookup(string propertyName)
        {
            AutomationElementFinder.StringPropertyNamesToTestForText.Add(propertyName);
        }

        public void AddObjectPropertyNameForTextLookup(string propertyName)
        {
            AutomationElementFinder.ObjectPropertyNamesToTestForText.Add(propertyName);
        }

        public void AddPropertyNameToTestForValue(string propertyName)
        {
            AutomationElementFinder.PropertyNamesToTestForValue.Add(propertyName);
        }


        public void AddValueManipulator(IValueManipulator valueManipulator)
        {
            ValueCommandHelper.AddManipulator(valueManipulator);    
        }

#warning Need to understand where this went :/ Can it be deleted?
        public void AddAutomationPeerHandlerForTapAction(Func<AutomationPeer, bool> handler)
        {
            throw new NotImplementedException("This method was removed within NokiaMusic Merge");
            //IvokeControlTapActionCommand.PatternTesters.Insert(0, handler);
        }

#warning Need to understand where this went :/ Can it be deleted?
        public void AddUIElementHandlerForTapAction(Func<UIElement, bool> handler)
        {
            throw new NotImplementedException("This method was removed within NokiaMusic Merge");
            //InvokeControlTapActionCommand.UIElementTesters.Insert(0, handler); 
        }

        private static string BddHostForWindowsPhone8
        {
            get
            {
                string remoteUrl = null;
                if (System.Environment.OSVersion.Version.Major == 8)
                {
                    try
                    {
                        using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (var isoStream = isoStore.OpenFile("bddhost.txt", FileMode.Open))
                            {
                                using (var streamReader = new System.IO.StreamReader(isoStream))
                                {
                                    string bddhost = streamReader.ReadLine();
                                    remoteUrl = "http://" + bddhost + "/phoneAutomation/automate";
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return remoteUrl;
            }
        }
    }
}