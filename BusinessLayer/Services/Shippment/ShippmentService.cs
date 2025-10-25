using AutoMapper;
using BusinessLayer.Contracts;
using DataAccess.Contracts;
using DataAccess.Models;
using Domains;
using Microsoft.AspNetCore.Identity;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Shippment
{
    public class ShippmentService : BaseService<TbShippment, ShipmentDto>, IShipment
    {
        IUserRecever _userRecever;
        IUserSender _userSender;
        ITrackingNumberCreator _trackingNumberCreator;
        IRateCalculator _rateCalculator;
        IUnitOfWork _uow;
        ITableRepository<TbShippment> _repository;
        IUserService _userService;
        IMapper _mapper;
        IShipmentStatus _shipmentStatus;

        public ShippmentService(ITableRepository<TbShippment> repository, IMapper mapper, IUserService userService, IUserSender userSender, IUserRecever userRecever, ITrackingNumberCreator trackingNumberCreator, IRateCalculator rateCalculator, IUnitOfWork unitOfWork , IShipmentStatus shipmentStatus )
            : base(unitOfWork, mapper, userService)
        {
            _uow = unitOfWork;
            _userRecever = userRecever;
            _userSender = userSender;
            _trackingNumberCreator = trackingNumberCreator;
            _rateCalculator = rateCalculator;
            _repository = repository;
            _userService = userService;
            _mapper = mapper;
            _shipmentStatus = shipmentStatus;

        }

        public async Task Create(ShipmentDto dto)
        {
            try
            {
                await _uow.BeginTransactionAsync();
                dto.TrackingNumber = _trackingNumberCreator.Create(dto);
                dto.ShippingRate = _rateCalculator.Calculate(dto);
                if (dto.SenderId == Guid.Empty)
                {
                    Guid UserSenderId = Guid.Empty;
                    _userSender.Add(dto.UserSender, out UserSenderId);
                    dto.SenderId = UserSenderId;
                }
                if (dto.ReceiverId == Guid.Empty)
                {
                    Guid UserReceverId = Guid.Empty;
                    _userRecever.Add(dto.UserReceiver, out UserReceverId);
                    dto.ReceiverId = UserReceverId;
                }
                Guid gShipmentId = Guid.Empty;

                this.Add(dto , out gShipmentId);
                // add shipment status
                ShippmentStatusDto status = new ShippmentStatusDto();
                status.ShippmentId = gShipmentId;
                status.CurrentState = (int)ShipmentStatusEnum.Created;
                status.CarrierId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                _shipmentStatus.Add(status);
                await _uow.CommitAsync();

            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();

            }
        }

        public Task Edit(ShipmentDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShipmentDto>> GetShipments()
        {
            try
            {
                var userId = _userService.GetLoggedInUser();

                var shipments = await _repository.GetList(
                    filter: a => a.CreatedBy == userId,
                    selector: a => new ShipmentDto
                    {
                        Id = a.Id,
                        ShippingDate = a.ShippingDate,
                        DelivryDate = a.DelivryDate,
                        SenderId = a.SenderId,
                        ReceiverId = a.ReceiverId,
                        ShippingTypeId = a.ShippingTypeId,
                        ShipingPackgingId = a.ShipingPackgingId,
                        Width = a.Width,
                        Height = a.Height,
                        Weight = a.Weight,
                        Length = a.Length,
                        PackageValue = a.PackageValue,
                        ShippingRate = a.ShippingRate,
                        PaymentMethodId = a.PaymentMethodId,
                        UserSubscriptionId = a.UserSubscriptionId,
                        TrackingNumber = a.TrackingNumber,
                        ReferenceId = a.ReferenceId,

                        UserSender = new UserSenderDto
                        {
                            Id = a.Sender.Id,
                            SenderName = a.Sender.SenderName,
                            Email = a.Sender.Email,
                            Phone = a.Sender.Phone
                        },
                        UserReceiver = new UserReceiverDto
                        {
                            Id = a.Receiver.Id,
                            ReceiverName = a.Receiver.ReceiverName,
                            Email = a.Receiver.Email,
                            Phone = a.Receiver.Phone
                        }
                    },
                    orderBy: a => a.CreatedDate,
                    isDescending: true,
                    a => a.Sender, a => a.Receiver
                );

                return shipments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting shipments", ex);
            }
        }

        public async Task<PagedResult<ShipmentDto>> GetShipments(int pageNumber, int pageSize)
        {
            try
            {
                var userId = _userService.GetLoggedInUser();

                var result = await _repository.GetPagedList(
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    filter: a => a.CreatedBy == userId && a.CurrentState > 0,
                    selector: a => new ShipmentDto
                    {
                        Id = a.Id,
                        ShippingDate = a.ShippingDate,
                        DelivryDate = a.DelivryDate,
                        SenderId = a.SenderId,
                        ReceiverId = a.ReceiverId,
                        ShippingTypeId = a.ShippingTypeId,
                        ShipingPackgingId = a.ShipingPackgingId,
                        Width = a.Width,
                        Height = a.Height,
                        Weight = a.Weight,
                        Length = a.Length,
                        PackageValue = a.PackageValue,
                        ShippingRate = a.ShippingRate,
                        PaymentMethodId = a.PaymentMethodId,
                        UserSubscriptionId = a.UserSubscriptionId,
                        TrackingNumber = a.TrackingNumber,
                        ReferenceId = a.ReferenceId,
                        UserSender = new UserSenderDto
                        {
                            Id = a.Sender.Id,
                            SenderName = a.Sender.SenderName,
                            Email = a.Sender.Email,
                            Phone = a.Sender.Phone
                        },
                        UserReceiver = new UserReceiverDto
                        {
                            Id = a.Receiver.Id,
                            ReceiverName = a.Receiver.ReceiverName,
                            Email = a.Receiver.Email,
                            Phone = a.Receiver.Phone
                        }
                    },
                    orderBy: a => a.CreatedDate,
                    isDescending: true,
                    a => a.Sender, a => a.Receiver
                );

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting shipments", ex);
            }
        }

        public async Task<ShipmentDto> GetShipment(Guid id)
        {
            try
            {
                var userId = _userService.GetLoggedInUser();

                var shipments = await _repository.GetList(
                    filter: a => a.Id == id && a.CreatedBy == userId,
                    selector: a => new ShipmentDto
                    {
                        Id = a.Id,
                        ShippingDate = a.ShippingDate,
                        DelivryDate = a.DelivryDate,
                        SenderId = a.SenderId,
                        ReceiverId = a.ReceiverId,
                        ShippingTypeId = a.ShippingTypeId,
                        ShipingPackgingId = a.ShipingPackgingId,
                        Width = a.Width,
                        Height = a.Height,
                        Weight = a.Weight,
                        Length = a.Length,
                        PackageValue = a.PackageValue,
                        ShippingRate = a.ShippingRate,
                        PaymentMethodId = a.PaymentMethodId,
                        UserSubscriptionId = a.UserSubscriptionId,
                        TrackingNumber = a.TrackingNumber,
                        ReferenceId = a.ReferenceId,

                        UserSender = new UserSenderDto
                        {
                            Id = a.Sender.Id,
                            SenderName = a.Sender.SenderName,
                            Email = a.Sender.Email,
                            Phone = a.Sender.Phone,
                            Address = a.Sender.Address,
                            //Contact = a.Sender.Contact,
                            //PostalCode = a.Sender.PostalCode,
                            //OtherAddress = a.Sender.OtherAddress,
                            CityId = a.Sender.CityId,
                            //CountryId = a.Sender.City.CountryId
                        },
                        UserReceiver = new UserReceiverDto
                        {
                            Id = a.Receiver.Id,
                            ReceiverName = a.Receiver.ReceiverName,
                            Email = a.Receiver.Email,
                            Phone = a.Receiver.Phone,
                            Address = a.Receiver.Address,
                            //Contact = a.Receiver.Contact,
                            //PostalCode = a.Receiver.PostalCode,
                            //OtherAddress = a.Receiver.OtherAddress,
                            CityId = a.Receiver.CityId,
                            //CountryId = a.Receiver.City.CountryId
                        }
                    },
                    orderBy: a => a.CreatedDate,
                    isDescending: true,
                    a => a.Sender,
                    a => a.Sender.City,
                    a => a.Sender.City.Country,
                    a => a.Receiver,
                    a => a.Receiver.City,
                    a => a.Receiver.City.Country
                );

                return shipments.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting shipment", ex);
            }
        }


    }
}