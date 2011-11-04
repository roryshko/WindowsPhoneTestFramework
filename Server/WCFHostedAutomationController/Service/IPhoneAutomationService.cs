// ----------------------------------------------------------------------
// <copyright file="IPhoneAutomationService.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Utils;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Service
{
    [ServiceContract]
    public interface IPhoneAutomationService
    {
        [OperationContract]
        [ServiceKnownType("GetKnownTypes", typeof(KnownTypeProvider))]
        [WebInvoke(Method = "POST", UriTemplate = "getNextCommand")]
        CommandBase GetNextCommand(int timeoutInMilliseconds);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "continueProcessing")]
        bool ContinueProcessing(Guid commandId);

        [OperationContract]
        [ServiceKnownType("GetKnownTypes", typeof(KnownTypeProvider))]
        [WebInvoke(Method = "POST", UriTemplate = "submitResult")]
        void SubmitResult(ResultBase result);
		
        [OperationContract]
        [ServiceKnownType("GetKnownTypes", typeof(KnownTypeProvider))]
        [WebInvoke(Method = "POST", UriTemplate = "rawSubmitResult")]
        bool RawSubmitResult(String jsonResult);
    }
}
