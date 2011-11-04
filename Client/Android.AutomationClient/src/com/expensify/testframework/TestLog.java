// ----------------------------------------------------------------------
// <copyright file="TestLog.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework;

public class TestLog {
	private static final String LogTag = "TestFramework";
	
	public static void Log(String message) {
		android.util.Log.i(LogTag, message);
	}
}
