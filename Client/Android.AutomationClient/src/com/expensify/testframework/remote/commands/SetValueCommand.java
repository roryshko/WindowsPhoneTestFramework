// ----------------------------------------------------------------------
// <copyright file="SetValueCommand.java" company="Expensify">
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

import android.widget.CheckBox;
import android.widget.TextView;

public class SetValueCommand extends AutomationElementCommandBase {	
	public String TextValue;
	protected void executeImpl() throws TestFrameworkException
    {
		if (trySetText())
			return;
		
		if (trySetIsChecked())
			return;
		
		// TODO - other things... e.g. values of progress bars, radio buttons, etc
    }
	
	private Boolean trySetText() throws TestFrameworkException
	{
		final TextView textView = getSpecialistView();
		if (textView == null)
			return false;
		
		textView.post(new Runnable() {
			@Override
			public void run() {
				textView.setText(TextValue);
			}
		});
		
		sendSuccessResult();
		return true;
    }
	
	private Boolean trySetIsChecked() throws TestFrameworkException
	{
		String lowerCaseTextValue = TextValue.toLowerCase(); 
		// TODO - need to be careful here - what does final really mean here?!
		Boolean boolValue = false;
		if ("true".equals(lowerCaseTextValue))
			boolValue = true;
		else if ("false".equals(lowerCaseTextValue))
			boolValue = false;
		else
			return false;
				
		final CheckBox checkBox = getSpecialistView();
		if (checkBox == null)
			return false;
		
		class SetCheckedTask implements Runnable {
	        Boolean boolValue;
	        SetCheckedTask(Boolean b) { boolValue = b; }
			@Override
			public void run() {
				checkBox.setChecked(boolValue);
			}
	    }
		
		checkBox.post(new SetCheckedTask(boolValue));
		
		sendSuccessResult();
		return true;
    }
}
