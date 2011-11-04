package com.cirrious.exampleApp.test;

import com.expensify.testframework.AutomationClient;

import com.jayway.android.robotium.solo.Solo;

import android.test.ActivityInstrumentationTestCase2;

@SuppressWarnings({ "unchecked" })
public class StubTest extends ActivityInstrumentationTestCase2 {

	private static final String TARGET_PACKAGE_ID="com.cirrious.exampleApp";
	private static final String LAUNCHER_ACTIVITY_FULL_CLASSNAME="com.cirrious.exampleApp.ExampleAppActivity";

	private static Class<?> launcherActivityClass;

	static{
		try
		{						
			launcherActivityClass =Class.forName(LAUNCHER_ACTIVITY_FULL_CLASSNAME);
		}
		catch (ClassNotFoundException e)
		{
			throw new RuntimeException(e);
		}
	}

	private Solo solo;
	
	public StubTest() throws ClassNotFoundException {
		super(TARGET_PACKAGE_ID, launcherActivityClass);
	}

	@Override
	protected void setUp() throws Exception
	{
		solo = new Solo(getInstrumentation(), getActivity());
	}

	public void testHoldApplicationOpen() 
	{
		AutomationClient client = new AutomationClient(solo);
		client.start();
		try {
			Thread.sleep(24 * 60 * 60 * 1000); // one day - should be long enough for most tests...
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		finally {
			try {
				client.stop();
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

	@Override
	public void tearDown() throws Exception
	{
		try
		{
			solo.finalize();
		}
		catch(Throwable e)
		{
			e.printStackTrace();
		}
	
		getActivity().finish();
		super.tearDown();
	}
}
