using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WorkFlowAppServer.WorkflowServerService {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new WorkFlowAppServerWorkflowServer() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
