// ----------------------------------------------------------------------
// <copyright file="TakePictureCommand.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.remote.commands;

import java.io.ByteArrayOutputStream;
import java.util.concurrent.Semaphore;

import com.expensify.testframework.utils.TestFrameworkException;

import android.graphics.Bitmap;
import android.view.View;

public class TakePictureCommand extends AutomationElementCommandBase {	
	 Semaphore finishedFlag = new Semaphore(1);		 
	 String errorMessage = null;
	 byte[] jpegBuffer = null;
	 
	protected void executeImpl() throws TestFrameworkException {
    	View view = getView(false);
    	
    	if (view == null)
   		 	view = getRootView();
    	
    	if (view == null) {
    		sendNotFoundResult();
    		return;
    	}
    	
		takeScreenShot(view);
     }

	 public void takeScreenShot(final View view) throws TestFrameworkException {
		 
    	 try
    	 {
	         view.setDrawingCacheEnabled(true);
	         view.buildDrawingCache();
	         Bitmap bitmap = view.getDrawingCache();
	         if (bitmap == null)
	         {
	        	 sendActionFailedResult("screenshot failed to draw bitmap");
	        	 return;
	         }
        	 ByteArrayOutputStream stream = new ByteArrayOutputStream();
             bitmap.compress(Bitmap.CompressFormat.JPEG, 75, stream);
             stream.close();
             jpegBuffer = stream.toByteArray();
    	 }
    	 catch (Exception exception)
    	 {
    		 throw new TestFrameworkException("exception seen while getting bitmap", exception); 
		 }
		 
		 if (jpegBuffer == null)
		 {
			 sendActionFailedResult(errorMessage);
			 errorMessage = "Unknown error - no screenshot collected";
		 }
		
		 sendPictureResult(jpegBuffer);
	 }
}
