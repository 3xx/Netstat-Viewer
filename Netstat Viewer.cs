using System;
using System.Diagnostics;
using System.Threading;

public class Netstat
{
    public static void Main()
    {
        Console.Title = "Netstat Viewer";

        while (true) 
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "netstat";
                process.StartInfo.Arguments = "-n -o";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.Clear(); 

           
                string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string[] columns = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (columns.Length > 3 && (columns[0] == "TCP" || columns[0] == "UDP"))
                    {
                        string remoteAddress = columns[2]; 
                        string pid = columns[4]; 

          
                        if (!(remoteAddress.EndsWith(":443") || remoteAddress.EndsWith(":80")))
                        {
                            string processName = GetProcessNameById(pid);

                      
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(processName + ", Port: ");

                    
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(remoteAddress);

                           
                            Console.ResetColor();
                            Console.WriteLine(); 
                        }
                    }
                }
            }

            Thread.Sleep(1000);
        }
    }

    private static string GetProcessNameById(string pid)
    {
        try
        {
            Process process = Process.GetProcessById(int.Parse(pid));
            return process.ProcessName;
        }
        catch
        {
            return "Unknown";
        }
    }
}
