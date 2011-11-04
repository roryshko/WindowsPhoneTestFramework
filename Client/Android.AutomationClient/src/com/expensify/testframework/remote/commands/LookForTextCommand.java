// ----------------------------------------------------------------------
// <copyright file="LookForTextCommand.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.remote.commands;

import com.expensify.testframework.remote.interfaces.AutomationIdentifier;
import com.expensify.testframework.utils.TestFrameworkException;

import android.view.View;

public class LookForTextCommand extends CommandBase {	
	public String Text;
	protected void executeImpl() throws TestFrameworkException
    {
    	AutomationIdentifier identifier = new AutomationIdentifier();
    	identifier.DisplayedText = Text;
    	
    	View view = identifier.getViewByDisplayedText(getSolo());
    	if (view != null)
    		sendSuccessResult();
    	else
    		sendNotFoundResult();
    }
}
