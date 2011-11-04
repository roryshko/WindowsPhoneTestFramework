// ----------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Threading;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class CommandBase
    {
        private bool _resultSent;

        public IConfiguration Configuration { get; set; }

        public void Do()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration must be set before Do() is called");

            try
            {
                DoImpl();
                SendErrorResultIfNoOtherResultSent();
            }
            catch (Exception exception)
            {
                SendExceptionFailedResult(exception);
                if (exception is ThreadAbortException)
                    throw;
            }
        }

        protected virtual void DoImpl()
        {
            // base class DoImpl() should never be called.
            throw new InvalidOperationException("DoImpl should never be called in CommandBase");
        }

        protected void SendExceptionFailedResult(Exception exception)
        {
            var result = new ExceptionFailedResult()
                             {
                                 Id = Id,
                                 ExceptionMessage = exception.Message,
                                 ExceptionType = exception.GetType().FullName,
                                 FailureText = string.Format("Exception: {0}: {1}", exception.GetType().Name, exception.Message)
                             };
            Send(result);
        }

        protected void SkipResult()
        {
            EnsureAtMostOneResultSent();
        }

        protected void SendSuccessResult()
        {
            var result = new SuccessResult() { Id = Id };
            Send(result);
        }

        protected void SendNotFoundResult()
        {
            var result = new NotFoundFailedResult() { Id = Id };
            Send(result);
        }

        protected void SendTextResult(string text)
        {
            var result = new SuccessResult() { Id = Id, ResultText = text};
            Send(result);
        }

        protected void SendPositionResult(double left, double top, double width, double height)
        {
            var result = new PositionResult()
                             {
                                 Id = Id,
                                 Left = left,
                                 Top = top,
                                 Width = width,
                                 Height = height,
                             };
            Send(result);
        }

        protected void SendPictureResult(byte[] bytes)
        {
            var result = new PictureResult()
            {
                Id = Id,
                EncodedPictureBytes = Convert.ToBase64String(bytes),
            };
            Send(result);
        }

        private void SendErrorResultIfNoOtherResultSent()
        {
            if (!_resultSent)
                SendExceptionFailedResult(
                    new InvalidOperationException("No result signalled by Command processing : " +
                                                  this.GetType().FullName));
        }

        private void Send(ResultBase result)
        {
            EnsureAtMostOneResultSent();
            result.Send(Configuration);
        }

        private void EnsureAtMostOneResultSent()
        {
            if (_resultSent)
            {
                // TODO - log this!
                throw new InvalidOperationException("Tried to send too many results");
            }

            _resultSent = true;
        }
    }
}