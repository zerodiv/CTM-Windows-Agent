using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Continuum_Windows_Testing_Agent
{
    class CTM_Java_Server
    {
        private String appPath;
        private Boolean isDisplayed;
        private String logFile;
        private Process javaServerContainer;
        private String jarFile;

        public CTM_Java_Server(String appPath, Boolean isDisplayed)
        {
            // Current release.
            // this.jarFile = "selenium-server-standalone-2.0b1.jar";
            
            // CTM Release
            this.jarFile = "selenium-server-CTM-20110125.jar";

            this.appPath = appPath;
            this.isDisplayed = isDisplayed;

            try
            {

                this.initProcess();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        ~CTM_Java_Server()
        {
            this.shutdownProcess();
        }

        public string getJavaCommandLine()
        {
            this.logFile = System.IO.Path.GetTempFileName();

            StringBuilder commandLineParams = new StringBuilder();
            commandLineParams.Append(" -jar \"" + this.appPath + "\\" + this.jarFile + "\"");
            commandLineParams.Append(" -trustAllSSLCertificates");
            commandLineParams.Append(" -log \"" + this.logFile + "\"" );
            // commandLineParams.Append(" -debug");
            return commandLineParams.ToString();
        }

        private void initProcess()
        {

            if (isDisplayed == false)
            {
                // We do a log file instead of output to the screen.
            }

            try
            {
                ProcessStartInfo jsServerInfo = new ProcessStartInfo();

                jsServerInfo.FileName = "java";
                jsServerInfo.Arguments = this.getJavaCommandLine();

                // You must use useshell execute false to redir stdout.
                /*
                jsServerInfo.UseShellExecute = false;
                jsServerInfo.RedirectStandardOutput = true;
                */

                // jsSeverInfo.CreateNoWindow = true;
                // jsSeverInfo.WindowStyle = ProcessWindowStyle.Hidden;

                this.javaServerContainer = new Process();
                this.javaServerContainer.StartInfo = jsServerInfo;

                this.javaServerContainer.Start();

                // JEO: If anyone knows the 'right' way to do this let me know.
                // Guess I need to make my own poll agent to see if the server is available.
                Boolean startedServer = false;
                int startTimeout = 0;
                while (startedServer == false && startTimeout < 60)
                {
                    try
                    {
                        if (this.isServerAvailable())
                        {
                            startedServer = true;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1000);
                            startTimeout++;
                        }
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(1000);
                        startTimeout++;
                    }

                }

                if (startedServer == false)
                {
                    throw new Exception("Failed to start java selenium server component within 60s");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to start java server", ex);
            }

        }

        public Boolean isServerAvailable()
        {
            try
            {

                IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, 4444);
                Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(ipe);

                if (s.Connected)
                {
                    s.Shutdown(SocketShutdown.Both);
                    s.Disconnect(true);
                    return true;
                }
                s.Shutdown(SocketShutdown.Both);
                s.Disconnect(true);
            }
            catch
            {
                return false;
            }
            return false;
        }

        public string getLog()
        {

            this.shutdownProcess();

            StringBuilder javaLog = new StringBuilder();
            foreach (String logLine in File.ReadAllLines(this.logFile))
            {
                javaLog.AppendLine(logLine);
            }

            return javaLog.ToString();

/*
            String log = this.javaServerContainer.StandardOutput.ReadToEnd();
                        
            return log;
            */
        }

        public void shutdownProcess()
        {
           
                if (this.javaServerContainer.HasExited == false)
                {
                    this.javaServerContainer.Kill();
                    this.javaServerContainer.WaitForExit();
                }
            
        }

    }
}
