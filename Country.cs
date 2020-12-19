namespace EuroDiffusion
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	class Country
	{
		public string Name { get; set; }
		public int XL { get; set; }
		public int YL { get; set; }
		public int XH { get; set; }
		public int YH { get; set; }
		public List<City> Cities = new List<City>();
		public int DayOfCompletion { get; set; }

		public bool IsCompleted
		{
			get
			{
				if (!_isCompleted && 
					(Cities.All(x => x.IsCompleted) || DayOfCompletion != -1))
					_isCompleted = true;

				return _isCompleted;
			}
		}
		private bool _isCompleted;

		public Country(string name, int xl, int yl, int xh, int yh)
		{
			var namePattern = $"[A-Z][a-z]{{1,{Constants.MAX_NAME_LENGTH}}}$";
			if (!Regex.IsMatch(name, namePattern))
				throw new ArgumentException($"Invalid country name: {name}. The country name must be no more than {Constants.MAX_NAME_LENGTH} characters and contain only letters.");

			if (xl > xh || yl > yh)
				throw new ArgumentException($"Invalid country coordinates: xl={xl}, xh={xh}, yl={yl}, yh={yh}.");

			if (new List<int> { xl, yl, xh, yh}.Any(x => x < Constants.MIN_SIZE || x > Constants.MAX_SIZE))
				throw new ArgumentException($"Invalid country coordinates: xl={xl}, xh={xh}, yl={yl}, yh={yh}.");

			Name = name;
			XL = xl;
			YL = yl;
			XH = xh;
			YH = yh;
			DayOfCompletion = -1;
		}

		public bool HasForeignNeighbours() 
		{
			foreach (var city in Cities)
			{
				foreach (var neighbours in city.Neighbours) 
				{
					if (neighbours.Country != this) return true;
				}
			}

			return false;
		}
	}
}
