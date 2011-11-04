// ----------------------------------------------------------------------
// <copyright file="CommandBase.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.remote.commands;

import java.util.UUID;

import com.expensify.testframework.Configuration;
import com.expensify.testframework.remote.results.*;
import com.expensify.testframework.utils.Base64;
import com.expensify.testframework.utils.ClassUtils;
import com.expensify.testframework.utils.TestFrameworkException;

import android.graphics.Rect;

import com.jayway.android.robotium.solo.Solo;

public abstract class CommandBase {
	public String __type;
	public UUID Id;
	public String Type;

	private Boolean _resultSent;
	private Configuration _configuration;

	public CommandBase()
	{
		_resultSent = false;
		Id = UUID.randomUUID();
		Type =  "WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands." + ClassUtils.getClassName(this);
		__type = ClassUtils.getWCFTypeText(Type);
	}

	public final void setConfiguration(Configuration configuration)
	{
		_configuration = configuration;
	}

	public final void execute() throws TestFrameworkException
	{
		if (_configuration == null)
			throw new TestFrameworkException("Internal logic error - configuration not set");

		try {
			executeImpl();
			sendErrorResultIfNoOtherResultSent();    		 
		}
		catch (Exception e) {
			sendExceptionFailedResult(e);
			//if (exception is ThreadAbortException)
			//    throw;
		}
	}

	protected abstract void executeImpl() throws TestFrameworkException; 

	protected Solo getSolo() {
		return _configuration.getSolo();
	}

	protected void sendExceptionFailedResult(Exception exception) throws TestFrameworkException
	{
		ExceptionFailedResult result = new ExceptionFailedResult();
		result.Id = Id;
		result.ExceptionMessage = exception.getMessage();
		result.ExceptionType = ClassUtils.getFullClassName(exception);
		result.FailureText = String.format("Exception: {0}: {1}", result.ExceptionType, exception.getMessage());
		send(result);
	}

	private void sendErrorResultIfNoOtherResultSent() throws TestFrameworkException
	{
		if (!_resultSent)
		{
			sendExceptionFailedResult(new TestFrameworkException("No result signalled by Command processing : " + this.Type));
		}
	}

	protected void skipResult() throws TestFrameworkException
	{
		ensureAtMostOneResultSent();
	}

	protected void sendSuccessResult() throws TestFrameworkException
	{
		SuccessResult result = new SuccessResult();
		result.Id = Id;
		send(result);
	}

	protected void sendNotFoundResult() throws TestFrameworkException
	{
		NotFoundFailedResult result = new NotFoundFailedResult();
		result.Id = Id;
		send(result);
	}

	protected void sendTextResult(String text) throws TestFrameworkException
	{
		SuccessResult result = new SuccessResult();
		result.Id = Id;
		result.ResultText = text;
		send(result);
	}

	protected void sendPositionResult(Rect visibleRectangle) throws TestFrameworkException
	{
		sendPositionResult(visibleRectangle.left, visibleRectangle.top, visibleRectangle.width(), visibleRectangle.height());
	}

	protected void sendPositionResult(double left, double top, double width, double height) throws TestFrameworkException
	{
		PositionResult result = new PositionResult();
		result.Id = Id;
		result.Left = left;
		result.Top = top;
		result.Width = width;
		result.Height = height;
		send(result);
	}

	protected void sendPictureResult(byte[] bytes) throws TestFrameworkException
	{
		PictureResult result = new PictureResult();
		result.Id = Id;
		result.EncodedPictureBytes = Base64.encodeToString(bytes, Base64.DEFAULT);
		send(result);
	}

	protected void sendActionFailedResult(String message) throws TestFrameworkException
	{
		ActionFailedResult result = new ActionFailedResult();
		result.Id = Id;
		result.FailureText = message;
		send(result);
	}

	private void send(ResultBase result) throws TestFrameworkException
	{
		ensureAtMostOneResultSent();
		try {
			result.send(_configuration);
		} catch (Exception e) {
			throw new TestFrameworkException("exception seen while sending result", e);
		}         
	}

	private void ensureAtMostOneResultSent() throws TestFrameworkException
	{
		if (_resultSent)
		{
			// TODO - log this!
			throw new TestFrameworkException("Tried to send too many results");
		}

		_resultSent = true;
	}
}
