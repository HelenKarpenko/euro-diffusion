﻿namespace EuroDiffusion
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	class EuroMap
	{
		public List<Country> Countries;

		private readonly City[,] _cities = new City[Constants.LENGTH + 2, Constants.WIDTH + 2];

		public EuroMap(List<Country> countries)
		{
			Countries = countries;
			InitializeMap(countries);
			ValidateForeignNeighbours();
		}

		public void SimulateEuroDiffusion()
		{
			if (Countries.Count == 1)
			{
				Countries.First().DayOfCompletion = 0;
				return;
			}

			var day = 1;
			var areAllCountriesCompleted = false;
			var allMotif = Countries.Select(x => x.Name).ToList();

			while (!areAllCountriesCompleted)
			{
				TransferCoinsToNeighbours();

				FinalizeBalancePerDay();

				areAllCountriesCompleted = true;
				foreach (var country in Countries)
				{
					if (country.IsCompleted(allMotif) && country.DayOfCompletion < 0)
						country.DayOfCompletion = day;

					if (!country.IsCompleted(allMotif))
						areAllCountriesCompleted = false;
				}

				day++;
			}
		}

		private void TransferCoinsToNeighbours()
		{
			foreach (var city in _cities)
			{
				city?.TransferCoinsToNeighbours();
			}
		}

		private void FinalizeBalancePerDay()
		{
			foreach (var city in _cities)
			{
				city?.FinalizeBalancePerDay();
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
						throw new ArgumentException($"{country.Name} intersects with {_cities[x, y].Country.Name} on [{x}, {y}].");

					var city = new City(country, x, y);
					_cities[x, y] = city;
					country.Cities.Add(city);
				}
			}
		}

		private void SetNeighbours()
		{
			foreach (var city in _cities)
			{
				if (city != null)
				{
					city.Neighbours = GetNeighbours(city.X, city.Y);
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
					throw new ArgumentException($"{country.Name} hasn't foreign neighbours.");
			}
		}
	}
}
