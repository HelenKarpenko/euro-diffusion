using System;
using System.Collections.Generic;
using System.Text;

namespace EuroDiffusion
{
	class EuroMap
	{
		public List<Country> Countries;

		private const int _length = 10;
		private const int _width = 10;
		private City[,] _cities = new City[_length + 2, _width + 2];

		public EuroMap(List<Country> countries)
		{
			Countries = countries;
			InitializeMap(countries);
			ValidateForeignNeighbours();

			for (var x = 0; x < _cities.GetLength(0); x++)
			{
				for (var y = 0; y < _cities.GetLength(1); y++)
				{
					if (_cities[x, y] != null)
						Console.Write($"x ");
					else
						Console.Write($". ");
				}
				Console.WriteLine();
			}
		}

		public void SimulateEuroDiffusion()
		{
			var day = 0;
			var areAllCountriesCompleted = false;

			while (!areAllCountriesCompleted)
			{
				TransferCoinsToNeighbours();

				FinalizeBalancePerDay();


				areAllCountriesCompleted = true;
				foreach (var country in Countries)
				{
					if (country.IsCompleted && country.DayOfCompletion < 0)
						country.DayOfCompletion = day;
					
					if (!country.IsCompleted)
						areAllCountriesCompleted = false;
				}

				day++;
			}
		}

		private void TransferCoinsToNeighbours()
		{
			for (var x = 0; x < _cities.GetLength(0); x++)
			{
				for (var y = 0; y < _cities.GetLength(1); y++)
				{
					if (_cities[x, y] != null)
					{
						var city = _cities[x, y];
						city.TransferCoinsToNeighbours();
					}
				}
			}
		}

		private void FinalizeBalancePerDay()
		{
			for (var x = 0; x < _cities.GetLength(0); x++)
			{
				for (var y = 0; y < _cities.GetLength(1); y++)
				{
					if (_cities[x, y] != null)
					{
						var city = _cities[x, y];
						city.FinalizeBalancePerDay();
					}
				}
			}
		}



		

		

		private void InitializeMap(List<Country> countries)
		{
			foreach (var country in countries)
			{
				AddCountryToMap(country);
				SetNeighbours();
			}
		
		}

		private void AddCountryToMap(Country country)
		{
			for (var x = country.XL; x <= country.XH; x++)
			{
				for (var y = country.YL; y <= country.YH; y++)
				{
					if (_cities[x, y] != null)
						throw new ArgumentException($"{country.Name} intersects with {_cities[x, y]} on [{x}, {y}]");

					var city = new City(country, x, y, Countries);
					_cities[x, y] = city;
					country.Cities.Add(city);
				}
			}
		}

		private void SetNeighbours()
		{
			for (var x = 1; x < _cities.GetLength(0); x++)
			{
				for (var y = 1; y < _cities.GetLength(1); y++)
				{
					if (_cities[x, y] != null)
					{
						var city = _cities[x, y];
						var neighbours = GetNeighbours(x, y);
						city.Neighbours = neighbours; 
					}
				}
			}
		}

		private List<City> GetNeighbours(int x, int y)
		{
			var neighbours = new List<City>();

			if (_cities[x, y + 1] != null)
				neighbours.Add(_cities[x, y + 1]);

			if (_cities[x, y - 1] != null)
				neighbours.Add(_cities[x, y - 1]);

			if (_cities[x + 1, y] != null)
				neighbours.Add(_cities[x + 1, y]);

			if (_cities[x - 1, y] != null)
				neighbours.Add(_cities[x - 1, y]);

			return neighbours;
		}

		private void ValidateForeignNeighbours()
		{
			if (Countries.Count == 1) return;

			foreach (var country in Countries)
			{
				if (!country.HasForeignNeighbours())
					throw new ArgumentException($"{country.Name} hasn't foreign neighbours");
			}
		
		}
	}
}
