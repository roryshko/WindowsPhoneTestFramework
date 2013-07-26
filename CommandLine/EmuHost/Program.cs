//  ----------------------------------------------------------------------
//  <copyright file="Program.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.IO;
using Args;
using Args.Help;
using Args.Help.Formatters;
using WindowsPhoneTestFramework.CommandLine.CommandLineHost;
using WindowsPhoneTestFramework.CommandLine.CommandLineHost.Commands;
using WindowsPhoneTestFramework.CommandLine.EmuHost.Commands;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.CommandLine.EmuHost
{
    public class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            AppLaunchingCommandLine commandLine;
            IModelBindingDefinition<AppLaunchingCommandLine> modelBindingDefinition = null;
            try
            {
                modelBindingDefinition = Configuration.Configure<AppLaunchingCommandLine>();
                commandLine = modelBindingDefinition.CreateAndBind(args);

                /*
                if (commandLine.ProductId == Guid.Empty)
                {
                    Console.WriteLine("");
                    Console.WriteLine("***Warning*** - no productId supplied");
                    Console.WriteLine("");
                }
                */
            }
            catch (Exception /*exception*/)
            {
                if (modelBindingDefinition != null)
                {
                    var help = new HelpProvider();
                    var formatter = new ConsoleHelpFormatter();

                    var sw = new StringWriter();
                    var text = help.GenerateModelHelp(modelBindingDefinition);
                    formatter.WriteHelp(text, sw);
                    Console.Write(sw.ToString());
                }
                else
                {
                    Console.Write("Sorry - no help available!");
                }
                return;
            }

            try
            {
                Console.WriteLine("AutomationHost starting");
                using (var program = new Program(commandLine))
                {
                    Console.WriteLine("To show help, enter 'help'");
                    program.Run();
                }
            }
            catch (QuitNowPleaseException)
            {
                Console.WriteLine("Goodbye");
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format("Exception seen {0} {1}", exception.GetType().FullName,
                                                exception.Message));
            }
        }

        private readonly AppLaunchingCommandLine _commandLine;
        private IAutomationController _automationController;

        public Program(AppLaunchingCommandLine commandLine)
        {
            _commandLine = commandLine;
            StartEmuAutomationController();

            var driverCommands = new DeviceControllerCommands
                {
                    DeviceController = _automationController.DeviceController,
                    CommandLine = _commandLine
                };
            AddCommands(driverCommands);

            var inputCommands = new DisplayInputCommands
                {
                    DisplayInputController = _automationController.DisplayInputController
                };
            AddCommands(inputCommands);

            var phoneAutomationCommands = new PhoneAutomationCommands
                {
                    ApplicationAutomationController =
                        _automationController.ApplicationAutomationController
                };
            AddCommands(phoneAutomationCommands);
        }

        private void StartEmuAutomationController()
        {
            Console.WriteLine("-> controller will be loaded from: " + _commandLine.Controller);
            Console.WriteLine("-> controller will be initialised with: " + _commandLine.Initialisation);
            Console.WriteLine("-> controller will identify controls using: " + _commandLine.AutomationIdentification);
            _automationController = Server.Core.Loader.LoadFrom(_commandLine.Controller);
            Console.WriteLine("-> controller loaded: " + _automationController.GetType().FullName);
            _automationController.Trace += (sender, args) => Console.WriteLine("-> " + args.Message);
            _automationController.Start(_commandLine.Initialisation, _commandLine.AutomationIdentification);
            Console.WriteLine("-> controller started");
            Console.WriteLine();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_automationController != null)
                {
                    _automationController.Dispose();
                    _automationController = null;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}