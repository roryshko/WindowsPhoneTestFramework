using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;

using Microsoft.SmartDevice.Connectivity;

using WindowsInput.Native;

using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Results;
using WindowsPhoneTestFramework.Server.Core.Tangibles;
using WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController;

namespace WindowsPhoneTestFramework.Server.AutomationController.WindowsPhone.Emulator
{
    /// <summary>
    /// The win 8 emulator windows phone device controller.
    /// </summary>
    internal class Win8EmulatorWindowsPhoneDeviceController : EmulatorWindowsPhoneDeviceController
    {
        #region Constants

        /// <summary>
        /// The bddhost file path.
        /// </summary>
        private const string BddhostFilePath = "bddhost.txt";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Win8EmulatorWindowsPhoneDeviceController" /> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="port">The port.</param>
        public Win8EmulatorWindowsPhoneDeviceController(string target, string port)
            : base(target)
        {
            DisplayInputController = new Win8EmulatorInputController();
            this.Port = port;
        }

        protected string Port { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The install.
        /// </summary>
        /// <param name="applicationDefinition">
        /// The application definition.
        /// </param>
        /// <returns>
        /// The <see cref="InstallationResult"/>.
        /// </returns>
        public override InstallationResult Install(ApplicationDefinition applicationDefinition)
        {
            InstallationResult result = base.Install(applicationDefinition);

            RemoteIsolatedStorageFile store = GetIsoStorage(applicationDefinition);

            if (store.FileExists(BddhostFilePath))
            {
                store.DeleteFile(BddhostFilePath);
            }

            string hostName = Dns.GetHostEntry("127.0.0.1").HostName + ":" + this.Port;

            File.WriteAllLines(BddhostFilePath, new[] { hostName });

            store.SendFile(BddhostFilePath, BddhostFilePath, true);

            return result;
        }

        #endregion
    }

    /// <summary>
    /// The win 8 emulator input controller.
    /// </summary>
    internal class Win8EmulatorInputController : WindowsPhoneWindowsEmulatorDisplayInputController
    {
        #region Fields

        /// <summary>
        /// The keyboard.
        /// </summary>
        private readonly Lazy<ManagementObject> keyboard = new Lazy<ManagementObject>(GetComputerKeyboard);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get emulator.
        /// </summary>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="ManagementObject"/>.
        /// </returns>
        public static ManagementObject GetEmulator(ManagementScope scope)
        {
            string query = string.Format("select * from Msvm_ComputerSystem where EnabledState=2");

            var searcher = new ManagementObjectSearcher(scope, new ObjectQuery(query));

            ManagementObjectCollection computers = searcher.Get();

            Guid dummy;
            return computers.Cast<ManagementObject>().First(comp => Guid.TryParse(comp["Name"].ToString(), out dummy));
        }

        /// <summary>
        /// The ensure hardware keyboard disabled.
        /// </summary>
        public override void EnsureHardwareKeyboardDisabled()
        {
            SendKeyPress(VirtualKeyCode.PRIOR);
        }

        /// <summary>
        /// The ensure hardware keyboard enabled.
        /// </summary>
        public override void EnsureHardwareKeyboardEnabled()
        {
            SendKeyPress(VirtualKeyCode.NEXT);
        }

        /// <summary>
        /// The send key long press.
        /// </summary>
        /// <param name="virtualKeyCode">
        /// The virtual key code.
        /// </param>
        /// <param name="duration">
        /// The duration.
        /// </param>
        public override void SendKeyLongPress(KeyboardKeyCode virtualKeyCode, TimeSpan duration)
        {
            var keyParams = new Dictionary<string, object> { { "keyCode", virtualKeyCode } };

            this.InvokeMethod("PressKey", keyParams);

            Pause(duration);

            this.InvokeMethod("ReleaseKey", keyParams);

            Pause(this.PauseDurationAfterSendingKeyPress);
        }

        /// <summary>
        /// The send key press.
        /// </summary>
        /// <param name="hardwareButtonToKeyCode">
        /// The hardware button to key code.
        /// </param>
        public override void SendKeyPress(VirtualKeyCode hardwareButtonToKeyCode)
        {
            const string MethodName = "TypeKey";

            var parameters = new Dictionary<string, object> { { "keyCode", hardwareButtonToKeyCode } };

            this.InvokeMethod(MethodName, parameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get computer keyboard.
        /// </summary>
        /// <returns>
        /// The <see cref="ManagementObject"/>.
        /// </returns>
        private static ManagementObject GetComputerKeyboard()
        {
            var scope = new ManagementScope(@"root\virtualization", null);
            ManagementObject vm = GetEmulator(scope);
            return GetComputerKeyboard(vm);
        }

        /// <summary>
        /// The get computer keyboard.
        /// </summary>
        /// <param name="vm">
        /// The vm.
        /// </param>
        /// <returns>
        /// The <see cref="ManagementObject"/>.
        /// </returns>
        private static ManagementObject GetComputerKeyboard(ManagementObject vm)
        {
            ManagementObjectCollection keyboardCollection = vm.GetRelated(
                "Msvm_Keyboard", "Msvm_SystemDevice", null, null, "PartComponent", "GroupComponent", false, null);

            ManagementObject keyboard = null;

            foreach (ManagementObject instance in keyboardCollection)
            {
                keyboard = instance;
                break;
            }

            return keyboard;
        }

        /// <summary>
        /// The invoke method.
        /// </summary>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The <see cref="ManagementBaseObject"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private ManagementBaseObject InvokeMethod(string methodName, Dictionary<string, object> parameters)
        {
            ManagementBaseObject inParams = this.keyboard.Value.GetMethodParameters(methodName);

            foreach (KeyValuePair<string, object> pair in parameters)
            {
                inParams[pair.Key] = pair.Value;
            }

            ManagementBaseObject outParams = this.keyboard.Value.InvokeMethod(methodName, inParams, null);

            if ((UInt32)outParams["ReturnValue"] != 0)
            {
                throw new InvalidOperationException("methodFailed");
            }

            return outParams;
        }

        #endregion
    }
}