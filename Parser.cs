using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EuroDiffusion
{
	class Parser
	{
		private const string _defaultInputFilePath = @"C:\Users\lena\source\repos\EuroDiffusion\EuroDiffusion\TestData\input.txt";
		private const int _maxCountriesAmount = 20;
		private const int _minCountriesAmount = 1;

		public List<List<Country>> ParseInputFile(string filePath = _defaultInputFilePath)
		{
			var result = new List<List<Country>>();

			var input = ReadFile(filePath);


			var lineIndex = 0;
			while (lineIndex < input.Count) 
			{
				var trimmedLine = input[lineIndex].Trim();
				if (!trimmedLine.All(char.IsDigit))
					throw new ArgumentException("Invalid line with number of countries.");

				var numberOfCountries = int.Parse(input[lineIndex]);
				if (numberOfCountries == 0)
					return result;

				if(numberOfCountries > _maxCountriesAmount || numberOfCountries < _minCountriesAmount)
					throw new ArgumentException($"Invalid amount of countries: {numberOfCountries}.");

				lineIndex++;

				var countries = new List<Country>();
				for (int i = 0; i < numberOfCountries; i++) 
				{
					var country = ParseCountry(input[lineIndex]);
					countries.Add(country);
					lineIndex++;
				}

				result.Add(countries);
			}

			return result;
		}

		private List<string> ReadFile(string filePath)
		{
			var result = new List<string>();
			using var sr = new StreamReader(filePath);
			string line;
			while ((line = sr.ReadLine()) != null)
			{
				result.Add(line);
			}

			return result;
		}

		private Country ParseCountry(string line) 
		{
			var args = line.Split(" ");
			if (args.Length != 5)
				throw new ArgumentException($"Error at line {line}. Invalid number of params.");

			var name = args[0];
			var xl = int.Parse(args[1]);
			var yl = int.Parse(args[2]);
			var xh = int.Parse(args[3]);
			var yh = int.Parse(args[4]);

			var country = new Country(name, xl, yl, xh, yh);
			return country;
		}
	}
}
