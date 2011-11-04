package com.expensify.testframework;

import com.expensify.testframework.remote.commands.CommandBase;
import com.expensify.testframework.remote.results.ResultBase;

public class PhoneAutomationServiceClient {
	
	private String _baseAddress;
	
    public PhoneAutomationServiceClient(String baseAddress)
    {
    	_baseAddress = baseAddress;
    }

	public CommandBase getNextCommand(int timeoutInMilliseconds) throws Exception {
		String body = "{\"timeoutInMilliseconds\":1000}";
		String response = execute("getNextCommand", body);
		CommandBase command = GetNextCommandResultContainer.parse(response);
		TestLog.Log("command fetched, type: " + command.Type);
		return command;
	}

	public void submitResult(ResultBase result) throws Exception {
	    String body = RawSubmitResultContainer.WrapAndStringify(result);
		String response = execute("rawSubmitResult", body);
		// ignore response
		TestLog.Log("result sent, response:" + response);
	}
	
	private String execute(String command, String body) throws Exception {
		RestClient client = createRestClient(command, body);
		client.execute(RestClient.RequestMethod.POST);
		return client.getResponse();	
	}
	
	private RestClient createRestClient(String command, String body) {
		RestClient client = new RestClient(_baseAddress + command);
		client.addHeader("Content-Type", "application/json");
		client.setContentBody(body);
		return client;
	}
}
