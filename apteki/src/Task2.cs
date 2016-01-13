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
			public Coordinates Coord;
			public double Distance;
			public Pharmacy(String name, String adress, Coordinates coord) {
				Name = name; Adress = adress; Coord = coord;
				Distance = 0.0;
			}
		}

		private bool _initialized = false;
		private StreamReader _streamReader = null;

		private List<Pharmacy> _pharmacyData = new List<Pharmacy>();
		private InputData _data = new InputData();

		public void closeStreamReader() {
			if (_streamReader != null) {
				_streamReader.Close();
				_streamReader = null;
			}
		}
		public bool SetInputData(String filename) {
			closeStreamReader();
			_streamReader = new StreamReader(filename);

			while (!_streamReader.EndOfStream) {
				var line = _streamReader.ReadLine();
				var lines = line.Split(new char[] { ' ' });	
		
				_data.Filename = Convert.ToString(lines[0]);
				_data.InputCoord = new Coordinates(Convert.ToDouble(lines[1].Replace('.', ',')), Convert.ToDouble(lines[2].Replace('.', ',')));
			}
			closeStreamReader();
			return SetPharmData();
		}
		private bool SetPharmData() {
			_initialized = false;
			_pharmacyData.Clear();

			_streamReader = new StreamReader(_data.Filename);

			_streamReader.ReadLine();
			while (!_streamReader.EndOfStream) {
				var line = _streamReader.ReadLine();
				var lines = line.Split(new char[] { '|' });

				var name = Convert.ToString(lines[0]);
				var addr = Convert.ToString(lines[1]);
				var coord = new Coordinates(Convert.ToDouble(lines[2].Replace('.', ',')), Convert.ToDouble(lines[3].Replace('.', ',')));
				_pharmacyData.Add(new Pharmacy(name, addr, coord));
			}
			closeStreamReader();
			_initialized = true;

			return true;
		}
		private double euclideanNorm(Coordinates x, Coordinates y) {
			var a = Math.Pow((x.Longitude - y.Longitude), 2.0);
			var b = Math.Pow((x.Latitude - y.Latitude), 2.0);
			return Math.Sqrt(a + b);
		}
		public bool ThreeClosePharmsToFile(String filename){
			if (!_initialized) throw new InvalidProgramException("System is not initialized!");
			if (_pharmacyData.Count < 3) throw new InvalidProgramException("Count of pharmacy data < 3!");
			
			_pharmacyData = _pharmacyData.OrderBy(x => {
				return euclideanNorm(_data.InputCoord, x.Coord); }).ToList();

				StreamWriter sw = new StreamWriter(filename);
				for (int i = 0; i < 3; i++)
					sw.WriteLine(_pharmacyData[i].Name + "|" + _pharmacyData[i].Adress);
				sw.Close();

				return true;
		}
	}
}

