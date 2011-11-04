// ----------------------------------------------------------------------
// <copyright file="AutomationClient.java" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

package com.expensify.testframework;

import java.util.concurrent.Semaphore;

import com.expensify.testframework.remote.commands.*;

import com.jayway.android.robotium.solo.Solo;

public class AutomationClient {
	private static final String DefaultAddressBase = "http://10.0.2.2:8085/phoneAutomation/jsonAutomate/"; 
	
    private static final int GetNextCommandTimeoutInMilliseconds = 2000;
    private static final int ErrorSleepTimeoutInMilliseconds = 500;
    //private static final int CheckServerSleepTimeoutInMilliseconds = 500;
    private static final int NullCommandSleepTimeoutInMilliseconds = 100;

    private Semaphore _stopPlease;
    private Thread _thread;
    private Configuration _configuration;
        
    public AutomationClient(Solo solo)
    {
    	_configuration = new Configuration(solo, DefaultAddressBase);
    	_stopPlease = new Semaphore(1);
    }    

    protected void finalize() throws Throwable {        
        try 
        {           
            stop();            
        }
        catch(Exception e) 
        {
        	// should I do something here?
        }        
        finally 
        {
            super.finalize();
        }
    }
    
    public void start()
    {
        _thread =  new Thread(new Runnable() {
			//@Override
			public void run() {
				try {
					runMain();
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		});        
		_thread.start();
    }

    public void stop() throws InterruptedException
    {
        if (_thread != null)
        {
            _stopPlease.acquire();
        	_thread.join();
        	_thread = null;
            _stopPlease.release();
        }
    }
    
    private void runMain() throws InterruptedException
    {
        Boolean isServerAvailable = true; // default to try to connect first time...
        while (_stopPlease.availablePermits() > 0)
        {
            try
            {
                if (isServerAvailable)
                    getAndProcessNextCommand();
/*                
#if USE_CONNECTION_CHECK_BEFORE_STARTING
                else
                    isServerAvailable = _configuration.TestIfRemoteAvailable();

                if (!isServerAvailable)
                    Thread.Sleep(TimeSpan.FromMilliseconds(CheckServerSleepTimeoutInMilliseconds));
#endif //USE_CONNECTION_CHECK_BEFORE_STARTING
*/
            }
            catch (InterruptedException exception)
            {
            	throw exception;
            }
            catch (Exception exception)
            {
                // probably means server not present - so sleep for a second
                // TODO - improve this...
/*
                  Debug.WriteLine(string.Format("Exception seen {0} {1}", 
 
                                              exception.GetType().FullName,
                                              exception.Message));
*/                           
            	Thread.sleep(ErrorSleepTimeoutInMilliseconds);
/*            	
#if USE_CONNECTION_CHECK_BEFORE_STARTING
                isServerAvailable = false;
#endif //USE_CONNECTION_CHECK_BEFORE_STARTING
*/
            }
        }
    }

    private void getAndProcessNextCommand() throws Exception
    {
    	PhoneAutomationServiceClient serviceClient = _configuration.createPhoneAutomationServiceClient();

        CommandBase command = null;
		try {
			command = serviceClient.getNextCommand(GetNextCommandTimeoutInMilliseconds);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			// TODO more!
			e.printStackTrace();
		}
        if (command != null) {
			processNextCommand(command);
        }
    }

    private void processNextCommand(final CommandBase command) throws Exception
    {
        if (command == null)
        {
            // TODO - log this!
            return;
        }

        // a bit worried this won't all be called on the UI thread... it will go bang :/
        command.setConfiguration(_configuration);
        command.execute();
        /*
        final Semaphore semaphore = new Semaphore(1);
        semaphore.acquire();
        Runnable runnable = new Runnable() {        	
			@Override
			public void run() {
				try {
		        command.setConfiguration(_configuration);
		        command.execute();
				} catch (Exception exception) {
					// mask this exception!
				}
				finally
				{
					semaphore.release();
				}
			};
        };
        _configuration.getSolo().getCurrentActivity().runOnUiThread(runnable);
        semaphore.acquire();
        */
        if (command instanceof NullCommand)
        {
            Thread.sleep(NullCommandSleepTimeoutInMilliseconds);
            return;
        }
    }    
}
