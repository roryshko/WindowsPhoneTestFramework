// ----------------------------------------------------------------------
// <copyright file="ResultBase.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.remote.results;

import java.util.UUID;

import com.expensify.testframework.Configuration;
import com.expensify.testframework.utils.ClassUtils;

public class ResultBase {
	public String __type;
	public UUID Id;
	
	public String GetResultType()
	{
        return "WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results." + ClassUtils.getClassName(this);
	}

     public ResultBase()
     {
         Id = UUID.randomUUID();
         __type = ClassUtils.getWCFTypeText(GetResultType());
     }

	public void send(Configuration configuration) throws Exception 
	{
		configuration.createPhoneAutomationServiceClient().submitResult(this);
	}
}
