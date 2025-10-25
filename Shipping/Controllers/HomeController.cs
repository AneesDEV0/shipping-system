using BusinessLayer.Contracts;
using Shared.Dtos;
using DataAccess.Contracts;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ShippingApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       public readonly IShippingTypeService shippingType;
        private readonly IShipment _IGenericRepository;

        public HomeController(IShippingTypeService service, IShipment iGenericRepository)
        {
            shippingType = service;
            _IGenericRepository = iGenericRepository;
        }
        void TestShipment()
        {
            var testShipment = new ShipmentDto
            {
                Id = Guid.NewGuid(),
                ShippingDate = DateTime.UtcNow,
                DelivryDate = DateTime.UtcNow.AddDays(3),

                SenderId = Guid.Empty,
                UserSender = new UserSenderDto
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    SenderName = "Ali Sender",
                    Email = "sender@example.com",
                    Phone = "01012345678",
                    PostalCode = "12345",
                    Contact = "Ali Sender Contact",
                    OtherAddress = "Apartment 5B, Sender Tower",
                    IsDefault = true,
                    CityId = Guid.Parse("BB070EAB-71DA-49D5-9C20-CBA65583C48D"),
                    Address = "123 Sender Street"
                },

                ReceiverId = Guid.Empty,
                UserReceiver = new UserReceiverDto
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    ReceiverName = "Omar Receiver",
                    Email = "receiver@example.com",
                    Phone = "01087654321",
                    PostalCode = "54321",
                    Contact = "Omar Receiver Contact",
                    OtherAddress = "Floor 2, Receiver Building",
                    CityId = Guid.Parse("BB070EAB-71DA-49D5-9C20-CBA65583C48D"),
                    Address = "456 Receiver Road"
                },

                ShippingTypeId = Guid.Parse("16F28412-2511-4615-8F07-67B028068777"),
                ShipingPackgingId = null, // optional
                Width = 25.0,
                Height = 15.0,
                Weight = 5.5,
                Length = 30.0,
                PackageValue = 1000m,
                ShippingRate = 75.0m,
                PaymentMethodId = null,
                UserSubscriptionId = null,
                TrackingNumber = 10000001,
                ReferenceId = Guid.NewGuid()
            };
            _IGenericRepository.Create(testShipment);
        }

        public IActionResult Index()
        {
            //TestShipment();

            //var data = shippingType.GetAll();
            return View();
        }
    }
}
