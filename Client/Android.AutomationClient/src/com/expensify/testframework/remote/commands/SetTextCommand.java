// ----------------------------------------------------------------------
// <copyright file="SetTextCommand.java" company="Expensify">
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

import android.widget.TextView;

public class SetTextCommand extends AutomationElementCommandBase {	
	public String Text;
	protected void executeImpl() throws TestFrameworkException
    {
		final TextView textView = getSpecialistView();
		if (textView == null)
			return;
		textView.post(new Runnable() {
			@Override
			public void run() {
				textView.setText(Text);
			}
		});
		sendSuccessResult();
    }
}
