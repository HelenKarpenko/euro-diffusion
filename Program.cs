namespace EuroDiffusion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
	{
		static void Main(string[] args)
		{
            Parser parser = new Parser();

            List<List<Country>> testCases;
            try
            {
                testCases = parser.ParseInputFile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            int caseNumber = 1;
            foreach (var testCase in testCases)
            {
                Console.WriteLine($"Case Number {caseNumber++}");
                try
                {
                    var euroMap = new EuroMap(testCase);
                    euroMap.SimulateEuroDiffusion();

                    var countries = euroMap.Countries.OrderBy(x => x.DayOfCompletion).ThenBy(x => x.Name);
                    foreach (var country in countries)
                    {
                        Console.WriteLine($"{country.Name} {country.DayOfCompletion}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.ReadLine();
        }
	}
}
