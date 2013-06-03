// -----------------------------------------------------------------------
// <copyright file="ControlContainsImageCommand.cs" company="NOKIA">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.Serialization;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class ControlContainsImageCommand: AutomationElementCommandBase
    {
         [DataMember]
        public string ImageName { get; set; }
    }
}
