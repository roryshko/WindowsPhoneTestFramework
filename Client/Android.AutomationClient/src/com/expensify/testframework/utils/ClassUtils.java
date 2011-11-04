// ----------------------------------------------------------------------
// <copyright file="ClassUtils.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.utils;

public class ClassUtils {
	
	// returns the class (without the package if any)
	public static String getClassName(Object o) {
		String fullyQualifiedClassName = o.getClass().getName();
		return getClassName(fullyQualifiedClassName);
	}
	
	// returns the class (without the package if any)
	public static String getClassName(String fullyQualifiedClassName) {
		int firstChar;
		String className = fullyQualifiedClassName;
		firstChar = fullyQualifiedClassName.lastIndexOf ('.') + 1;
		if ( firstChar > 0 ) {
			className = className.substring ( firstChar );
		}
		return className;
	}
	
	// returns package and class name
	public static String getFullClassName(Class<? extends Object> c) {
		return  c.getName();
	}

	// returns package and class name
	public static String getFullClassName(Object o) {
		return  getFullClassName(o.getClass());
	}
	
	// returns the package without the classname, empty string if
	// there is no package
	public static String getPackageName(Class<?> c) {
		String fullyQualifiedName = c.getName();
		return getPackageName(fullyQualifiedName);
	}

	// returns the package without the classname, empty string if
	// there is no package
	public static String getPackageName(String fullyQualifiedClassName) {
		int lastDot = fullyQualifiedClassName.lastIndexOf ('.');
		if (lastDot==-1){ return ""; }
		return fullyQualifiedClassName.substring (0, lastDot);
	}

	// returns "Class:#NamespacePath" - used in WCF cross-platform comms
	public static String getWCFTypeText(String fullyQualifiedClassName) {
		return getClassName(fullyQualifiedClassName) + ":#" + getPackageName(fullyQualifiedClassName);
	}		   
}
