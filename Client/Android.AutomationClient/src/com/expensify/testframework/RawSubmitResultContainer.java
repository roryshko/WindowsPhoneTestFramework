// ----------------------------------------------------------------------
// <copyright file="RawSubmitResultContainer.java" company="Expensify">
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

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

public class RawSubmitResultContainer {
	
	public String jsonResult;
	
	// TODO - get the GsonBuilder working for us. Much better than using our own code
	public static String WrapAndStringify(ResultBase result)
	{
		RawSubmitResultContainer toSubmit = new RawSubmitResultContainer();
		toSubmit.jsonResult = RawStringify(result);
		GsonBuilder gsonBuilder= new GsonBuilder();
		Gson gson = gsonBuilder.create();
		return gson.toJson(toSubmit);
	}
		
	// TODO - get the GsonBuilder working for us. Much better than using our own code
	public static String RawStringify(ResultBase result)
	{
		GsonBuilder gsonBuilder= new GsonBuilder();
		//gsonBuilder.serializeNulls();
		//gsonBuilder.setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE);		
		//gsonBuilder.disableHtmlEscaping();
		//gsonBuilder.registerTypeHierarchyAdapter(ResultBase.class, new ResultSerializer());
		Gson gson = gsonBuilder.create();		
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
