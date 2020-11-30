using System;
using System.Collections.Generic;

namespace EuroDiffusion
{
	class Program
	{
		static void Main(string[] args)
		{
            Parser parser = new Parser();

            List<List<Country>> res = null;
            try
            {
                 res = parser.ParseInputFile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            int caseNumber = 1;
            foreach (var caseToSolve in res)
            {
                Console.WriteLine($"Case Number {caseNumber++}");
                try
                {
                    var map = new EuroMap(caseToSolve);
                    map.SimulateEuroDiffusion();

                    foreach (var country in map.Countries)
                    {
                        Console.WriteLine($"{country.Name}: {country.DayOfCompletion}");
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
