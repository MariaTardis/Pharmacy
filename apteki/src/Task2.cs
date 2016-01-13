using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace apteki.src {

	public class Task2 {

		public class Coordinates {
			public double Longitude;
			public double Latitude;
			public Coordinates(double longitude, double latitude){
				Longitude = longitude; Latitude = latitude;
			}
		}
		public class InputData { 
			public String Filename; 
			public Coordinates InputCoord;
		}
		public class Pharmacy {
			public String Name; 
			public String Adress;
			public Coordinates PharmCoord;
			public double Distance;
			public Pharmacy(String name, String adress, Coordinates coord) {
				Name = name; Adress = adress; PharmCoord = coord;
				Distance = 0.0;
			}
		}

		private bool _initialized = false;
		private bool _initializedInput = false;

		private List<Pharmacy> _pharmacyData = new List<Pharmacy>();
		private InputData _data = new InputData();

		public bool SetInputData(String filename) {
			_initializedInput = false;
			try {
				StreamReader sr = new StreamReader(filename);
				while (!sr.EndOfStream) {
					var line = sr.ReadLine();
					var lines = line.Split(new char[] { ' ' });	
		
					_data.Filename = Convert.ToString(lines[0]);
					_data.InputCoord = new Coordinates(Convert.ToDouble(lines[1].Replace('.', ',')), Convert.ToDouble(lines[2].Replace('.', ',')));
				}
				sr.Close();
				_initializedInput = true;

				return true;
			}
			catch (Exception) { return false; }
		}
		public bool SetPharmData() {
			if (_initializedInput) {
				_initialized = false;
				_pharmacyData.Clear();
				try {
					StreamReader sr = new StreamReader(_data.Filename);
					int index = 0;
					while (!sr.EndOfStream) {
						var line = sr.ReadLine();
						var lines = line.Split(new char[] { '|' });

						if (index == 0) { index++; continue; }
						var name = Convert.ToString(lines[0]);
						var addr = Convert.ToString(lines[1]);
						var coord = new Coordinates(Convert.ToDouble(lines[2].Replace('.', ',')), Convert.ToDouble(lines[3].Replace('.', ',')));
						_pharmacyData.Add(new Pharmacy(name, addr, coord));
						index++;
					}
					sr.Close();
					_initialized = true;

					return true;
				}
				catch (Exception) { return false; }
			}
			else return false;
		}

		private double euclideanNorm(Coordinates x, Coordinates y) {
			var a = Math.Pow((x.Longitude - y.Longitude), 2.0);
			var b = Math.Pow((x.Latitude - y.Latitude), 2.0);
			return Math.Sqrt(a + b);
		}
		private void calculateDist() {
			for (int i = 0; i < _pharmacyData.Count; i++) 
				_pharmacyData[i].Distance = euclideanNorm(_data.InputCoord, _pharmacyData[i].PharmCoord);
		}

		private bool orderPharmsByDist() {
			if (_initialized) {
				if (_pharmacyData.Count < 3) return false;
				calculateDist();
				_pharmacyData = _pharmacyData.OrderBy(x => x.Distance).ToList();
				return true;
			}
			else return false;
		}

		public bool GetAndWriteToFileThreeClosePharms(String filename) {
			if (orderPharmsByDist()) {
				StreamWriter sw = new StreamWriter(filename);
				for (int i = 0; i < 3; i++)
					sw.WriteLine(_pharmacyData[i].Name + "|" + _pharmacyData[i].Adress);
				sw.Close();
				return true;
			}
			else return false;
		}
	}
}

