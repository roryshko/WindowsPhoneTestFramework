// ----------------------------------------------------------------------
// <copyright file="GetNextCommandResultContainer.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework;

import com.expensify.testframework.remote.commands.CommandBase;
import com.expensify.testframework.utils.CommandDeserializer;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

public class GetNextCommandResultContainer {
	public CommandBase GetNextCommandResult;
	
	public static CommandBase parse(String json)
	{
		GsonBuilder gsonBuilder= new GsonBuilder();
	    CommandDeserializer deserializer = new CommandDeserializer();
	    deserializer.registerCommonCommands();
	    gsonBuilder.registerTypeAdapter(CommandBase.class, deserializer);
		Gson gson = gsonBuilder.create();
		GetNextCommandResultContainer commandContainer = gson.fromJson(json, GetNextCommandResultContainer.class);
		
		return commandContainer.GetNextCommandResult;		
	}
}
