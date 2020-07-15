using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudoFileEnricher
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePathIN, filePathOUT, container;
            int jaar = 2015;

            // Arguments
            filePathIN = args[0];   //First argument
            filePathOUT = args[1];  //Second argument

            container = "";

            if (filePathIN.ToUpper().Contains(".SUDO"))
            {
                FileStream stream = null;
                string line;

                try
                {
                    stream = new FileStream(filePathOUT, FileMode.Truncate, FileAccess.ReadWrite, FileShare.None);

                    // Open the text file using a stream reader.
                    using (StreamReader fileReader = new StreamReader(filePathIN))
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        while ((line = fileReader.ReadLine()) != null)
                        {

                            if (line.Substring(0, 4).Trim().Length == 0)
                            {
                                container = container + " " + line.Trim();
                                //Console.WriteLine($"1:{container}");
                            }
                            else
                            {
                                if (container.Length > 0)
                                {
                                    fileWriter.WriteLine(container);

                                    // Increase jaar if needed
                                    if (line.Substring(0, 3) == "Jan" && container.Substring(0, 3) == "Dec")
                                    {
                                        jaar++;
                                    }
                                }

                                // Add missing year
                                if (line.Length > 16 && !int.TryParse(line.Substring(16, 4), out _))
                                {
                                    container = line.Insert(16, $"{jaar} ");
                                }
                                else
                                {
                                    container = line;
                                }
                            }
                        }

                        if (container.Length > 0)
                        {
                            fileWriter.WriteLine(container);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
