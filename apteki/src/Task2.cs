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
			public Pharmacy(String name, String adress, Coordinates coord) {
				Name = name; Adress = adress; PharmCoord = coord;
			}
		}

		public bool Initialized { get { return _initialized; } private set { } }
		public bool InitializedInput { get { return _initializedInput; } private set { } }
		private bool _initialized = false;
		private bool _initializedInput = false;

		private List<Pharmacy> _pharmacyData = new List<Pharmacy>();
		private List<String> _inputData = new List<String>();
		private InputData _data = new InputData();
		private List<double> _distance = new List<double>();
		public List<Pharmacy> CloseFarm = new List<Pharmacy>();

		public bool SetInputData(String filename) {
			_initializedInput = false;
			_inputData.Clear();
			try {
				StreamReader sr = new StreamReader(filename);
				while (!sr.EndOfStream) {
					var line = sr.ReadLine();
					var lines = line.Split(new char[] { ' ' });
					_inputData = lines.ToList();
				}
				_data.Filename = Convert.ToString(_inputData[0]);
				_data.InputCoord = new Coordinates(Convert.ToDouble(_inputData[1].Replace('.', ',')),Convert.ToDouble(_inputData[2].Replace('.', ',')));
				sr.Close();
				_initializedInput = true;

				return true;
			}
			catch (Exception) { return false; }
		}
		public bool SetPharmData() {
			if (_initializedInput)
			{
				_initialized = false;
				_pharmacyData.Clear();
				try
				{
					StreamReader sr = new StreamReader(_data.Filename);
					int index = 0;
					while (!sr.EndOfStream)
					{

						var line = sr.ReadLine();
						var lines = line.Split(new char[] { '|' });
						var lineList = lines.ToList();

						if (index == 0) { index++; continue; }
						var name = Convert.ToString(lineList[0]);
						var addr = Convert.ToString(lineList[1]);
						var coord = new Coordinates(Convert.ToDouble(lineList[2].Replace('.', ',')), Convert.ToDouble(lineList[3].Replace('.', ',')));
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
			for (int i = 0; i < _pharmacyData.Count; i++) {
				_distance.Add(euclideanNorm(_data.InputCoord, _pharmacyData[i].PharmCoord));
			}
		}
		private void sortByDistance() {
			for (int j = _pharmacyData.Count - 1; j >= 1; j--)
				for (int i = 0; i < j; i++)
				{
					if (_distance[i] < _distance[i + 1])
					{
						var dist = _distance[i];
						_distance[i] = _distance[i + 1];
						_distance[i + 1] = dist;
						var temp = _pharmacyData[i];
						_pharmacyData[i] = _pharmacyData[i + 1];
						_pharmacyData[i + 1] = temp;
					}
				}
		}

		public bool GetThreeCloseFarm() {
			if (_initialized) {
				calculateDist();
				sortByDistance();
				for (int i = _pharmacyData.Count - 1; i >= _pharmacyData.Count - 3; i--)
					CloseFarm.Add(_pharmacyData[i]);
				return true;
			}
			else return false;
		}
	}
}

