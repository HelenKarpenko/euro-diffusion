namespace EuroDiffusion
{
    using System.Collections.Generic;
    using System.Linq;

    class City
    {
        public Country Country { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public List<City> Neighbours = new List<City>();
        public Dictionary<string, int> Balance = new Dictionary<string, int>();
        public Dictionary<string, int> BalancePerDay = new Dictionary<string, int>();

        public City(Country country, int x, int y)
        {
            Country = country;
            X = x;
            Y = y;

            Balance[country.Name] = Constants.INITIAL_CITY_BALANCE;
        }

        public void TransferCoinsToNeighbours()
        {
            foreach (var motif in Balance.Keys.ToList())
            {
                var amountToTransfer = Balance[motif] / Constants.REPRESENTATIVE_PORTION;
                if (amountToTransfer > 0)
                {
                    foreach (var neighbour in Neighbours)
                    {
                        neighbour.TransferMoneyPerDay(motif, amountToTransfer);
                        Balance[motif] -= amountToTransfer;
                    }
                }
            }
        }

        public void FinalizeBalancePerDay()
        {
            foreach (var motif in BalancePerDay.Keys.ToList())
            {
                if (!Balance.Keys.Contains(motif))
                    Balance.Add(motif, BalancePerDay[motif]);
                else
                    Balance[motif] += BalancePerDay[motif];

                BalancePerDay[motif] = 0;
            }
        }

        public void TransferMoneyPerDay(string motif, int amountToTransfer)
        {
            if (!BalancePerDay.Keys.Contains(motif))
                BalancePerDay.Add(motif, amountToTransfer);
            else
                BalancePerDay[motif] += amountToTransfer;
        }

        public bool IsCompleted(List<string> allMotif)
        {
            return allMotif.Any() && allMotif.All(key => Balance.ContainsKey(key));
        }
    }
}
