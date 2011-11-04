// ----------------------------------------------------------------------
// <copyright file="SubmitResultContainer.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework;

import com.expensify.testframework.remote.results.ResultBase;

import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

public class SubmitResultContainer {
	// TODO - get the GsonBuilder working for us. Much better than using our own code
	public static String WrapAndStringify(ResultBase result, String wrapperName)
	{
		String resultJson = RawStringify(result);
		return "{\""+ wrapperName + "\":"+resultJson +"}";		
	}
		
	// TODO - get the GsonBuilder working for us. Much better than using our own code
	public static String RawStringify(ResultBase result)
	{
		GsonBuilder gsonBuilder= new GsonBuilder();
		Gson gson = gsonBuilder.create();
		gsonBuilder.serializeNulls();
		gsonBuilder.setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE);		
		gsonBuilder.disableHtmlEscaping();
		gsonBuilder.registerTypeHierarchyAdapter(ResultBase.class, new ResultSerializer());
		String resultJson = gson.toJson(result);
		resultJson = hackResultJsonOrder(resultJson);
		return resultJson;		
	}
	
	private static String hackResultJsonOrder(String input) {
		int typeIndex = input.indexOf(",\"__type\":\"");
		if (typeIndex < 0)
			return input;
			
		if (input.indexOf('{') != 0) 	
			return input;
		
		String output = "{" + input.substring(typeIndex + 1, input.length() - 1)+ "," + input.substring(1, typeIndex) + "}";
		return output;
	}	
}
