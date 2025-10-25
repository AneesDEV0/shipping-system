using BusinessLayer.Contracts;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Shippment
{
    public class TrackingNumberCreatorService : ITrackingNumberCreator
    {
        public double Create(ShipmentDto dto)
        {
            return 65432; 
        }
    }
}
