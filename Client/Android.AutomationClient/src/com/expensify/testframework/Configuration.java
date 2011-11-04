// ----------------------------------------------------------------------
// <copyright file="Configuration.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework;

import com.jayway.android.robotium.solo.Solo;

public class Configuration {
	private Solo _solo;
	private String _baseAddress;

	public Configuration(Solo solo, String baseAddress)
	{
		_solo = solo;
		_baseAddress = baseAddress;
	}
	
	public PhoneAutomationServiceClient createPhoneAutomationServiceClient()
	{
		PhoneAutomationServiceClient client = new PhoneAutomationServiceClient(_baseAddress);
		return client;
	}
	
	public Solo getSolo()
	{
		return _solo;
	}
}
