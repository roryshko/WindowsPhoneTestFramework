// ----------------------------------------------------------------------
// <copyright file="RunSpec.js" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------


function executeTests(config)
{
	WScript.echo("Loading helpers...");

	/* helper methods */

	function twoDigits(input) {
		input = '' + input;
		while (input.length < 2)
			input = '0' + input;
		return input;
	}

	String.prototype.startsWith = function(str) {
		return (this.match("^"+str)==str);
	}

	String.prototype.endsWith = function(str) {
		return (this.match(str+"$")==str);
	}

	Date.prototype.prettyPrint = function () {
		return this.getFullYear() + '-' +
			twoDigits(this.getMonth()+1) + '-' +
			twoDigits(this.getDate())  + '-' +
			twoDigits(this.getHours())  + '-' +
			twoDigits(this.getMinutes())  + '-' +
			twoDigits(this.getSeconds());
	}

	function safeCreateFolder(fso, folderPath) {
		if (!fso.FolderExists(folderPath))
			fso.CreateFolder(folderPath);
	}

	function patchUpReport(reportText) {
		reportText = reportText.replace( /_startEmuShot_/g, '\n          <img src="');
		reportText = reportText.replace( /_endEmuShot_/g, '" alt="" height="200"/>');
		reportText = reportText.replace( / \tTrace:/g, '<span class="emuTraceMessage">');
		reportText = reportText.replace( / \tWarning:/g, '<span class="emuWarningMessage">');
		reportText = reportText.replace( / \tError/g, '<span class="emuErrorMessage">');
		reportText = reportText.replace( / \t\t \t/g, '</span>');
		reportText = reportText.replace( /span.traceMessage/g, 'span.emuTraceMessage\n{ font-style:italic; margin-left: 4em; color: #888888; }\nspan.emuWarningMessage\n{ font-style:italic; margin-left: 4em; color: #FFB00F; }\nspan.emuErrorMessage\n{ font-style:italic; margin-left: 4em; color: #FF3030; }\nspan.traceMessage' );
		return reportText;
	}

	// define file constants
	// Note: if a file exists, using forWriting will set
	// the contents of the file to zero before writing to
	// it. 
	var forReading = 1, forWriting = 2, forAppending = 8;

	function writeStringIntoFile(f, text) {
		// Open the file 
		os = f.OpenAsTextStream( forWriting, 0 );
		
		// write the text
		os.Write(text);
		
		//close the file
		os.Close();
	}

	function readFileIntoString(f) {
		// define array to store lines. 
		rline = new Array();

		// Open the file 
		is = f.OpenAsTextStream( forReading, 0 );
		
		// start and continue to read until we hit
		// the end of the file. 
		var count = 0;
		while( !is.AtEndOfStream ){
		   rline[count] = is.ReadLine();
		   count++;
		}
		// Close the stream 
		is.Close();
		// Place the contents of the array into 
		// a variable. 
		var msg = "";
		for(i = 0; i < rline.length; i++){
		   msg += rline[i] + "\n";
		}
		return msg;
	}

	function runWithEcho(toRun, title, detailedEcho) {
		WScript.echo("======================");
		WScript.echo("Starting: " + title + "...");
		WScript.echo('');
		try
		{
			WScript.echo(detailedEcho);
			toRun();
		}
		catch (error)
		{
			WScript.echo('Error seen: ' + error.description);
		}
		WScript.echo("Completed: " + title);
		WScript.echo('');
	}

	/* end helpers */

	WScript.echo("");
	WScript.echo("======================");
	WScript.echo("ExampleApp Test Runner");
	WScript.echo("======================");
	WScript.echo("");

	WScript.echo("Setting up test results folder");

	var fso = WScript.CreateObject("Scripting.FileSystemObject");
	safeCreateFolder(fso, config.testDirectory);

	var shell = WScript.CreateObject("WScript.Shell");
	shell.CurrentDirectory = config.testDirectory;

	var date =  new Date();
	var datedTestDirectory = config.testDirectory + '/' + date.prettyPrint();
	safeCreateFolder(fso, datedTestDirectory);					
	shell.CurrentDirectory = datedTestDirectory;

	WScript.echo("Test results folder is " + datedTestDirectory);

	WScript.echo('');

	var batchCommand = '"' + config.pathToNUnitConsole + '"'
						+ ' "' + config.pathToTargetFolder + config.targetFileName  + '"'
						+ ' /labels /out=TestResult.txt /xml=TestResult.xml';
	runWithEcho(function() {
						WScript.echo(batchCommand);
						shell.run(batchCommand, 1 /* show normal */, true /* wait for this to finish*/);					
					},
					"Running NUnit",
					"shell>" + batchCommand);				

	var reportCommand = '"' + config.pathToSpecFlow + '"'
						+ ' nunitexecutionreport' 
						+ ' "' + config.pathToClientCsProj + '"';
						
	runWithEcho(function() {
						WScript.echo(reportCommand);
						shell.run(reportCommand, 1 /* show normal */, true /* wait for this to finish*/);
					},
					"Generating report",
					"shell>" + reportCommand);
					
	var imageSpec = config.pathToTargetFolder + "_EmuShot_*.png";
	runWithEcho(function() {
						fso.MoveFile(imageSpec, datedTestDirectory);
					},
				'Moving images',
				'File source spec is: ' + imageSpec);
				
	var reportPath = datedTestDirectory + '/TestResult.html';
	runWithEcho(function() {
						var reportFile = fso.GetFile(reportPath);
						var reportText = readFileIntoString(reportFile);
						reportText = patchUpReport(reportText);
						writeStringIntoFile(reportFile, reportText);
					},
				'Patching report syntax');

	WScript.echo("======================");
	WScript.echo('Complete!');
	WScript.echo('');


	if (config.openHtmlInBrowser) {
		WScript.echo('Opening test result file...');
		shell.run(reportPath);
		WScript.echo('');
		WScript.echo('Report open!');
		WScript.echo('');
	}
}

WScript.echo("Warning - if you are running this using WScript.exe instead of CScript.exe then expect a lot of message boxes!");

// script parameters - exe paths
var relativePathToSpecFlow = '/../packages/SpecFlow.1.8.1/tools/specflow.exe';
var relativePathToNUnitConsole = '/../packages/NUnit.2.5.10.11092/tools/nunit-console-x86.exe'

// script parameters - target
var relativePathToTargetFolder = "/../Bin/debug/";
var targetFileName = 'UnitConverter.Spec.dll';
var relativePathToClientCsProj = "/UnitConverter/UnitConverter.csproj";

// operational parameters
var openHtmlInBrowser = WScript.Arguments.Named.Exists("openHtml") ? Number(WScript.Arguments.Named.Item("openHtml")) : 1;

var shell = WScript.CreateObject("WScript.Shell");
var baseDirectory = shell.CurrentDirectory;

// put everything in one config/settings object
var settings = {
	testDirectory : baseDirectory + '/Test',
	pathToNUnitConsole : baseDirectory + relativePathToNUnitConsole,
	pathToTargetFolder : baseDirectory + relativePathToTargetFolder,
	pathToSpecFlow : baseDirectory + relativePathToSpecFlow,
	pathToClientCsProj : baseDirectory + relativePathToClientCsProj,
	targetFileName : targetFileName,
	openHtmlInBrowser : openHtmlInBrowser
};

// run
executeTests(settings);
