using BusinessLayer.Contracts;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Shippment
{
    public class RateCalculatorService : IRateCalculator
    {
        public decimal Calculate(ShipmentDto dto)
        {
            return 100.50m;
        }
    }
}
