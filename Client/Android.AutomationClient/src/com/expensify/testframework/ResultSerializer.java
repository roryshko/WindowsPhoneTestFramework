// ----------------------------------------------------------------------
// <copyright file="ResultSerializer.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework;

import java.lang.reflect.Field;
import java.lang.reflect.Type;

import com.expensify.testframework.remote.results.ResultBase;

import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonSerializationContext;
import com.google.gson.JsonSerializer;

public class ResultSerializer implements JsonSerializer<ResultBase> {
	private static final String TYPE_FIELD_NAME = "__type";
	
	public JsonElement serialize(ResultBase source, Type typeOfSrc, JsonSerializationContext context) {
		JsonObject jsonObject = new JsonObject();
		Class<? extends ResultBase> theClass = source.getClass();
		
		// process the __type field first
		try {
			Field typeField = theClass.getField(TYPE_FIELD_NAME);
			processField(source, jsonObject, typeField);
		} catch (SecurityException e1) {
			// ignore for now
		} catch (NoSuchFieldException e1) {
			// ignore for now
		}
		
		// now process the other fields
		Field[] fields = theClass.getFields();
		for (Field field : fields) {
			if (field.getName() != TYPE_FIELD_NAME)
				processField(source, jsonObject, field);
		}	
		
		return jsonObject;
	}

	private void processField(ResultBase source, 
								JsonObject jsonObject,
								Field field) {		
		if (field == null)
			return;
		
		if (!field.isAccessible())
			return;

		try {
			Class<? extends Object> fieldClass = field.getClass();
			if (fieldClass.isPrimitive()) {
				jsonObject.addProperty(field.getName(), field.getDouble(source));
			} else {
				Object o =  field.get(source);
				jsonObject.addProperty(field.getName(), o.toString());
			}
		} catch (IllegalAccessException e) {
			// just ignore for now...
		}
	}
}
