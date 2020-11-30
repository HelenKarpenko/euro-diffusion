﻿using System.Collections.Generic;
using System.Linq;

namespace EuroDiffusion
{
    class City
    {
        public Country Country { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public List<City> Neighbours = new List<City>();
        public Dictionary<string, int> Balance = new Dictionary<string, int>();
        public Dictionary<string, int> BalancePerDay = new Dictionary<string, int>();

        private const int _initialCityBalance = 1_000_000;
        private const int _representativePortion = 1_000;

        public bool IsCompleted
        {
            get
            {
                if (!_isCompleted)
                {
                    foreach (var motif in Balance.Keys.ToList())
                    {
                        if (Balance[motif] == 0)
                        {
                            _isCompleted = false;
                            return _isCompleted;
                        }
                    }

                    _isCompleted = true;
                }

                return _isCompleted;
            }
        }
        private bool _isCompleted;

        

        public City(Country country, int x, int y, List<Country> countryCoins)
        {
            Country = country;
            X = x;
            Y = y;

            Balance = countryCoins.ToDictionary(x => x.Name, x => 0);
            BalancePerDay = countryCoins.ToDictionary(x => x.Name, x => 0);
            Balance[country.Name] = _initialCityBalance;
        }

        public void TransferCoinsToNeighbours() 
        {
            foreach (var neighbour in Neighbours) 
            {
                foreach (var motif in Balance.Keys.ToList()) 
                {
                    if (Balance[motif] > _representativePortion)
                    {
                        var amountToTransfer = Balance[motif] / _representativePortion;
                        neighbour.BalancePerDay[motif] += amountToTransfer;
                        Balance[motif] -= amountToTransfer;
                    }
                }
            }
        }

        public void FinalizeBalancePerDay()
        {
            foreach (var motif in BalancePerDay.Keys.ToList())
            {
                Balance[motif] += BalancePerDay[motif];
                BalancePerDay[motif] = 0;
            }
        }
    }
}