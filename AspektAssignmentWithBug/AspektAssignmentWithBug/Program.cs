using System.ComponentModel;
using System.IO;

namespace AspektAssignmentWithBug
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string directoryPath = @"C:\AspektFolder";

            List<string> txtFiles = new List<string>();
            GetTxtFiles(directoryPath, txtFiles);

            foreach (var file in txtFiles)
            {
                AppendTextToFile(file, "ASPEKT");
            }
        }

        static void GetTxtFiles(string directoryPath, List<string> txtFiles)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.txt");
            txtFiles.AddRange(files);

            string[] subdirectories = Directory.GetDirectories(directoryPath);
            // Bug 1.
            // The first argument of GetTxtFiles(directoryPath, txtFiles) is wrong, therefore there is an infinite recursion
            //foreach (string subdirectory in subdirectories)
            //{
            //    GetTxtFiles(directoryPath, txtFiles);
            //}

            //Solution:
            //Instead of "directoryPath", the first argument is changed to "subdirectory" to correctly recurse into subdirectories,
            //so the GetTxtFiles is recursively called with the subdirectory path and ensures that the method searches for .txt files in each subdirectory.
            foreach (string subdirectory in subdirectories)
            {
                GetTxtFiles(subdirectory, txtFiles);
            }
        }

        static void AppendTextToFile(string filePath, string textToAppend)
        {
            //Bug 2.
            //The connection to the files stays opened after writing to them and the resourses are not disposed 
            //StreamWriter writer = null;
            //writer = File.AppendText(filePath);
            //writer.WriteLine(textToAppend);

            //Solution 1. 
            // The "using" statement is used, to ensure proper cleanup of resourses and closing the connection
            // after the code execution leaves the "using" block, even if an exception happens within the block.
            using StreamWriter writer = File.AppendText(filePath);
            writer.WriteLine(textToAppend);

            //Solution 2.
            //Adding writer.Close() or writer.Dispose() at the end, to close the connection
            //StreamWriter writer = null;
            //writer = File.AppendText(filePath);
            //writer.WriteLine(textToAppend);
            //writer.Close();

            //Solution 3.
            //Wrapping up the code in a try-catch block and closing the connection in the finally block,
            //to ensure that even if exception happens, the connection will be closed.
            //(before closing, it is checked if the writer is not null, so it means that the connection was never oppened)

            //StreamWriter writer = null;
            //try
            //{
            //    writer = File.AppendText(filePath);
            //    writer.WriteLine(textToAppend);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("The process failed: ");
            //}
            //finally {
            //    writer?.Close();
            //}


        }
    }
}
