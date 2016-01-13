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
			if (task.SetInputData("input.txt")) {
				task.SetPharmData();
				if (task.GetThreeCloseFarm()){
					StreamWriter sw = new StreamWriter("output.txt");
          foreach (Pharmacy pharm in task.CloseFarm)
						sw.WriteLine(pharm.Name + "|" + pharm.Adress);       
				}
			}	
		}
	}	
}
