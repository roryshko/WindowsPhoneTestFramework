// ----------------------------------------------------------------------
// <copyright file="GetPositionCommand.java" company="Expensify">
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

import android.graphics.Rect;
import android.view.View;

public class GetPositionCommand extends AutomationElementCommandBase {	
     
     public Boolean ReturnEmptyIfNotVisible;
     
 	protected void executeImpl() throws TestFrameworkException
    {
 		if (!ReturnEmptyIfNotVisible)
 			throw new TestFrameworkException("Don't know how to handled ReturnEmptyIfNotVisible==false");
 		
    	View view = getView();
    	if (view == null)
    		return;
    	
    	Rect visibleRectangle = new Rect();
    	Boolean success = view.getGlobalVisibleRect(visibleRectangle);
    	if (!success)
    		visibleRectangle = new Rect(0,0,0,0);
    	
    	sendPositionResult(visibleRectangle);
    }
}
