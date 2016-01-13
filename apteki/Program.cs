using System.IO;
using System;
using Pharmacy = apteki.src.Task2.Pharmacy;
namespace apteki.src
{
	class Program
	{
		static void Main(string[] args)
		{
			Task2 task = new Task2();
			if (task.SetInputData("input.txt")){
				task.SetPharmData();
				task.GetAndWriteToFileThreeClosePharms("output.txt");
			}
		}	
	}
}
