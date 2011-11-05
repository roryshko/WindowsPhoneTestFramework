// ----------------------------------------------------------------------
// <copyright file="CommandDeserializer.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.utils;

import java.lang.reflect.Type;
import java.util.HashMap;
import java.util.Map;

import com.expensify.testframework.remote.commands.*;

import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParseException;

public class CommandDeserializer implements JsonDeserializer<CommandBase>
{
	private static final String DefaultCommandElementName = "__type";
	
	String commandElementName;
	Gson gson;
	Map<String, Class<? extends CommandBase>> commandRegistry;

	public CommandDeserializer()
	{
		this(DefaultCommandElementName);
	}
	
	public CommandDeserializer(String commandElementName)
	{
		this.commandElementName = commandElementName;
		GsonBuilder gsonBuilder = new GsonBuilder();
		gsonBuilder.setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE);
		gson = gsonBuilder.create();
		commandRegistry = new HashMap<String, Class<? extends CommandBase>>();
	}

	public void registerCommonCommands()
	{
		registerCommand(new ConfirmAliveCommand());
		registerCommand(new GetPositionCommand());
		registerCommand(new GetTextCommand());
		registerCommand(new GetValueCommand());
		registerCommand(new InvokeControlTapActionCommand());
		registerCommand(new LookForTextCommand());
		registerCommand(new NullCommand());
		registerCommand(new SetFocusCommand());
		registerCommand(new SetTextCommand());
		registerCommand(new TakePictureCommand());
	}
	
	void registerCommand(CommandBase command)
	{
		registerCommand(command.__type, command.getClass());
	}
	
	void registerCommand(String command, Class<? extends CommandBase> commandInstanceClass)
	{
		commandRegistry.put(command, commandInstanceClass);
	}

	@Override
	public CommandBase deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context)
	throws JsonParseException
	{
		try
		{
			JsonObject commandObject = json.getAsJsonObject();
			JsonElement commandTypeElement = commandObject.get(commandElementName);
			Class<? extends CommandBase> commandInstanceClass = commandRegistry.get(commandTypeElement.getAsString());
			CommandBase command = gson.fromJson(json, commandInstanceClass);
			return command;
		}
		catch (Exception e)
		{
			throw new RuntimeException(e);
		}
	}
}