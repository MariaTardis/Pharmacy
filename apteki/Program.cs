using System.IO;
using System;
using Pharmacy = apteki.src.Task2.Pharmacy;
namespace apteki.src
{
	class Program {
		static void Main(string[] args) {
			Task2 task = new Task2();
			var inFile = "input.txt";
			var outFile = "output.txt";

			try {
				if (task.SetInputData(inFile))
					task.ThreeClosePharmsToFile(outFile);
			}
			catch (Exception ex) {
				StreamWriter sw = new StreamWriter(outFile);
				sw.Write(ex.Message);
				sw.Close();
			}		
		}	
	}
}
