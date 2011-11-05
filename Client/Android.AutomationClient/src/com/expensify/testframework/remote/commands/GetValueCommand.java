// ----------------------------------------------------------------------
// <copyright file="GetValueCommand.java" company="Expensify">
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
import android.widget.CheckBox;

public class GetValueCommand extends AutomationElementCommandBase {	
	protected void executeImpl() throws TestFrameworkException {
		if (tryText())
			return;
		
		if (tryCheckbox())
			return;

		// fail!
	}	
	
	private Boolean tryText() throws TestFrameworkException {
		TextView textView = getSpecialistView();
		if (textView == null)
			return false;
		sendTextResult(textView.getText().toString());
		return true;
	}	
	
	private Boolean tryCheckbox() throws TestFrameworkException {
	    CheckBox checkbox = getSpecialistView();
		if (checkbox == null)
			return false;
		sendTextResult(checkbox.isChecked() ? "true" : "false");
		return true;
	}	
}
