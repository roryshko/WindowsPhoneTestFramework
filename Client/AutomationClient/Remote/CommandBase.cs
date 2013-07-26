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
using System.Collections.Generic;
using System.Text;
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
                                 FailureText = BuildExceptionMessage(exception)
                             };
            Send(result);
        }

        private static string BuildExceptionMessage(Exception exception)
        {
            return string.Format("Exception: {0}: {1} \n\t{2}", exception.GetType().Name, exception.Message, exception.StackTrace)
                   + ((exception.InnerException != null)
                       ? "\n\twith Inner " + BuildExceptionMessage(exception.InnerException)
                       : string.Empty);
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

        protected void SendNotFoundResult(string failureText = null)
        {
            var result = new NotFoundFailedResult() { Id = Id };
            if (!string.IsNullOrWhiteSpace(failureText))
            {
                result.FailureText = failureText;
            }

            Send(result);
        }

        protected void SendDictionaryResult(Dictionary<string, string> dict)
        {
            var result = new DictionaryResult()
                {
                    Id = this.Id,
                    Results = dict
                };

            Send(result);
        }

        protected void SendTextResult(string text)
        {
            var result = new SuccessResult() { Id = Id, ResultText = text };
            Send(result);
        }

        protected void SendColorResult(string colorHex)
        {
            var result = new SuccessResultColor() { Id = Id, ResultColor = colorHex };
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

        protected void SendProgressResult(double min, double max, double current)
        {
            var result = new ProgressResult
            {
                Id = Id,
                Min = min,
                Max = max,
                Current = current
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
                throw new InvalidOperationException("Tried to send results more than once");
            }

            _resultSent = true;
        }
    }
}