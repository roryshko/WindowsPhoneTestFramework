// ----------------------------------------------------------------------
// <copyright file="SetFocusCommand.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.remote.commands;

import com.expensify.testframework.utils.TestFrameworkException;

import android.view.View;

public class SetFocusCommand extends AutomationElementCommandBase {	
	protected void executeImpl() throws TestFrameworkException
    {
		View view = getView();
		if (view == null)
			return;
		if (view.requestFocus())
			sendSuccessResult();
		else
			super.sendActionFailedResult("view did not manage to request focus");
    }
}
