// ----------------------------------------------------------------------
// <copyright file="AutomationIdentifier.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework.remote.interfaces;

import java.util.ArrayList;

import android.view.View;
import android.widget.TextView;

import com.jayway.android.robotium.solo.Solo;

public class AutomationIdentifier {

	private final static String NastyHackyAutoName = "auto:";

	public String AutomationName;
	public String ElementName;
	public String DisplayedText;

	public Boolean isEmpty() {
		return ((AutomationName == null || AutomationName.equals(""))
				&& (ElementName == null || ElementName.equals("")) 
				&& (DisplayedText == null || DisplayedText.equals("")));
	}

	public View getViewByAutomationName(Solo solo) {
		if (AutomationName == null || AutomationName.equals(""))
			return null;

		String weAreLookingFor = NastyHackyAutoName + AutomationName;
		ArrayList<View> allViews = solo.getCurrentViews();
		for (View view : allViews) {
			Object tag = view.getTag();
			if (tag != null) {
				String tagText = tag.toString();
				if (tagText.equals(weAreLookingFor))
					return view;
			}
		}

		return null;
	}

	public View getViewByElementName(Solo solo) {
		if (ElementName == null || ElementName.equals(""))
			return null;

		ArrayList<View> allViews = solo.getCurrentViews();
		for (View view : allViews) {
			Object contentDescription = view.getContentDescription();
			if (contentDescription != null) {
				if (contentDescription.toString().equals(ElementName)) {
					return view;
				}
			}
		}

		return null;
	}

	public View getViewByDisplayedText(Solo solo) {
		if (DisplayedText == null || DisplayedText.equals(""))
			return null;

		ArrayList<View> allViews = solo.getCurrentViews();
		for (View view : allViews) {
			String text = getDisplayedTextFromView(view);
			if (DisplayedText.equals(text))
				return view;
		}

		return null;
	}

	private static String getDisplayedTextFromView(View view) {
		// TextView includes things like Button and EditText -
		// http://developer.android.com/reference/android/widget/Button.html
		if (view instanceof TextView) {
			TextView textView = (TextView) view;
			String text = textView.getText().toString();
			//TestLog.Log(text);
			return text;
		}

		return null;
	}
}
