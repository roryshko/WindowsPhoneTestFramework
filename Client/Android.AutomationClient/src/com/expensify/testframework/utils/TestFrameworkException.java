// ----------------------------------------------------------------------
// <copyright file="TestFrameworkException.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.utils;

public class TestFrameworkException extends Exception {
	private static final long serialVersionUID = 1L;

	public TestFrameworkException(String message)
	{
		super(message);
	}

	public TestFrameworkException(String message, Exception e) {
		super(message, e);
	}
}
