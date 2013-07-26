//  ----------------------------------------------------------------------
//  <copyright file="ServiceHostController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Utils;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Service;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController
{
    public class ServiceHostController : TraceBase, IDisposable
    {
        private ServiceHost _serviceHost;
        private IApplicationAutomationController _automationController;

        public AutomationIdentification AutomationIdentification { get; set; }

        public IApplicationAutomationController AutomationController
        {
            get
            {
                if (_automationController == null)
                    throw new InvalidOperationException("AutomationController not available");

                return _automationController;
            }
        }

        public ServiceHostController()
        {
            AutomationIdentification = AutomationIdentification.TryEverything;
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Stop();
            }
        }

        public void Start(Uri bindingAddress)
        {
            if (_serviceHost != null)
                throw new InvalidOperationException("_serviceHost already started");

            if (_automationController != null)
                throw new InvalidOperationException("_automationController already created");

            InvokeTrace("building host...");

            // build the service
            var phoneAutomationService = new PhoneAutomationService();
            phoneAutomationService.Trace += (sender, args) => InvokeTrace(args);
            var serviceHost = new ServiceHost(phoneAutomationService, bindingAddress);

            if (!IsRunningOnMono())
            {
                // Enable metadata publishing
                var smb = new ServiceMetadataBehavior
                    {
                        HttpGetEnabled = true,
                        MetadataExporter = new WsdlExporter 
#if !MONO
                            {PolicyVersion = PolicyVersion.Policy15}
#endif
                        //!MONO
                    };
                serviceHost.Description.Behaviors.Add(smb);
            }

            if (!IsRunningOnMono())
            {
                // build SOAP ServiceEndpoint
                serviceHost.AddServiceEndpoint(
                    typeof (IPhoneAutomationService),
                    GetHttpBinding(),
                    bindingAddress + "/automate");
            }

            // build JSON ServiceEndpoint
            var jsonServiceEndpoint = serviceHost.AddServiceEndpoint(
                typeof (IPhoneAutomationService),
                GetWebHttpBinding(),
                bindingAddress + "/jsonAutomate");
            var webHttpBehavior = new WebHttpBehavior
                {
                    DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                    DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                    DefaultBodyStyle = WebMessageBodyStyle.Wrapped
                };
            jsonServiceEndpoint.Behaviors.Add(webHttpBehavior);

            // open the host
            InvokeTrace("opening host...");
            serviceHost.Open();
            InvokeTrace("host open");

            _automationController = new ApplicationAutomationController(phoneAutomationService, AutomationIdentification);
            _serviceHost = serviceHost;
        }

        public void Stop()
        {
            _automationController = null;
            if (_serviceHost != null)
            {
                InvokeTrace("closing host");
                _serviceHost.Close();
                InvokeTrace("host closed");
                _serviceHost = null;
            }
        }

        private static Binding GetHttpBinding()
        {
            var binding = new BasicHttpBinding
                {
                    Name = "SOAP",
                    MaxReceivedMessageSize = 1000000,
                    MaxBufferSize = 1000000,
                    ReaderQuotas = {MaxStringContentLength = 1000000},
                    HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                    Security = {Mode = BasicHttpSecurityMode.None}
                };
            return binding;
        }

        private static Binding GetWebHttpBinding()
        {
            var binding = new WebHttpBinding
                {
                    Name = "JSON",
                    MaxReceivedMessageSize = 1000000,
                    MaxBufferSize = 1000000,
                    ReaderQuotas = {MaxStringContentLength = 1000000},
                    HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                    Security = {Mode = WebHttpSecurityMode.None}
                };
            return binding;
        }
    }
}