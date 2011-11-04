// ----------------------------------------------------------------------
// <copyright file="AutomationElementCommandBase.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.remote.commands;


import com.expensify.testframework.TestLog;
import com.expensify.testframework.remote.interfaces.AutomationIdentifier;
import com.expensify.testframework.utils.ClassUtils;
import com.expensify.testframework.utils.TestFrameworkException;


import android.app.Activity;
import android.view.View;
import android.view.Window;

public abstract class AutomationElementCommandBase extends CommandBase {	    
	public AutomationIdentifier AutomationIdentifier;     
		
    protected Boolean isAutomationIdEmpty() {
    	return AutomationIdentifier == null || AutomationIdentifier.isEmpty();
    }

    // implementation based on http://stackoverflow.com/questions/4486034/android-how-to-get-root-view-from-current-activity
    protected View getRootView() {
    	Activity activity = getSolo().getCurrentActivity();
    	if (activity == null)
    		return null;
    	
    	Window window = activity.getWindow();
    	if (window == null)
    		return null;
    	
    	View view = window.findViewById(android.R.id.content);
    	if (view == null)
    		return null;
    	
    	return view.getRootView();
    }
	protected <T extends View> T getSpecialistView() throws TestFrameworkException {
		return getSpecialistView(true);
    }
    
	// Jon Skeet says this suppress is a necessary evil - http://stackoverflow.com/questions/2592642/type-safety-unchecked-cast-from-object
	@SuppressWarnings("unchecked")
	protected <T extends View> T getSpecialistView(Boolean sendNotFoundResultOnFail) throws TestFrameworkException {
		View view = getView();
		if (view == null)
			return null;
		
		try {
			return (T)view;
		} catch (ClassCastException e) {
			TestLog.Log("Unable to get element of expected type - instead found " + ClassUtils.getFullClassName(view));
			if (sendNotFoundResultOnFail)
				sendNotFoundResult();
			return null;
		}
    }
        
    protected View getView() throws TestFrameworkException {
    	return getView(true);
    }
    
    protected View getView(Boolean sendNotFoundResultOnFail) throws TestFrameworkException {
    	View view = getViewImplementation();
    	if (view == null && sendNotFoundResultOnFail)
    		sendNotFoundResult();
    	
    	return view;
    }
    
    private View getViewImplementation() {
    	if (isAutomationIdEmpty())
    		return null;
    	
    	View toReturn;
    	toReturn = getViewByAutomationName();
    	if (toReturn != null)
    		return toReturn;
    	
    	toReturn = getViewByElementName();
    	if (toReturn != null)
    		return toReturn;
    	
    	toReturn = getViewByDisplayedText();
    	if (toReturn != null)
    		return toReturn;
    	
    	return null;    	
    }
    
    private View getViewByAutomationName() {
    	return AutomationIdentifier.getViewByAutomationName(getSolo());
    }
    
    private View getViewByElementName() {
    	return AutomationIdentifier.getViewByElementName(getSolo());
    }
    
    private View getViewByDisplayedText() {
    	return AutomationIdentifier.getViewByDisplayedText(getSolo());    
    }    
}
