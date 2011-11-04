// ----------------------------------------------------------------------
// <copyright file="NullCommand.java" company="Expensify">
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

public class NullCommand extends CommandBase {	
	protected void executeImpl() throws TestFrameworkException
    {
    	// this is fine - a null command so nothing to do :)
		skipResult();
    }
}
