using AutoMapper;
using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Domains.Models;
using BusinessLayer.Services;
using System.Net.NetworkInformation;
using BusinessLayer.Services.ShipmentFile.ManageStatus;

namespace BusinessLayer.Services.ShipmentFile
{
    public class ShipmentQueryService : BaseService<TbShipment, DtoShipment>, IShipmentQuery      
    {
   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _UserService;

        public ShipmentQueryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserService UserService) : base(unitOfWork, mapper, UserService)
        {
            _unitOfWork = unitOfWork;
            _UserService = UserService;
       
        }
        public async Task<DtoShipment> GetShimpentAsync(Guid ShipmentId)
        {
            try
            {
                var data = await _unitOfWork.Repository<TbShipment>().GetByIdAsync(
                    e => e.CreatedBy == ResolveAuditUserId() && e.Id == ShipmentId,
                    e => new DtoShipment
                    {
                        Id = e.Id,
                        ShippingDate = e.ShippingDate,
                        DeliveryDate = e.DeliveryDate,
                        ShippingTypeId = e.ShippingTypeId,
                        ShippingPackagingId = e.ShippingPackagingId,
                        PaymentMethodId = e.PaymentMethodId,
                        UserSubscriptionId = e.UserSubscriptionId,
                        ReferenceId = e.ReferenceId,
                        Width = e.Width,
                        Height = e.Height,
                        Length = e.Length,
                        Weight = e.Weight,
                        TrackingNumber = e.TrackingNumber,
                        PackageValue = e.PackageValue,
                        ShippingRate = e.ShippingRate,
                        SenderId = e.SenderId,
                        ReceiverId = e.ReceiverId,
                        CurrentState = e.CurrentState,
                        Sender = e.Sender == null ? null : new DtoUserSender
                        {
                            Id = e.Sender.Id,
                            UserId = e.Sender.UserId,
                            SenderName = e.Sender.SenderName,
                            Email = e.Sender.Email,
                            Phone = e.Sender.Phone,
                            Contact = e.Sender.Contact,
                            CityId = e.Sender.CityId,
                            CountryId = e.Sender.CountryId,
                            Address = e.Sender.Address,
                            AddressDetails = e.Sender.AddressDetails,
                            OtherAddress = e.Sender.OtherAddress,
                            IsDefault = e.Sender.IsDefault,
                            PostalCode = e.Sender.PostalCode,
                        },
                        Receiver = e.Receiver == null ? null : new DtoUserReceiver
                        {
                            Id = e.Receiver.Id,
                            UserId = e.Receiver.UserId,
                            ReceiverName = e.Receiver.ReceiverName,
                            Email = e.Receiver.Email,
                            Phone = e.Receiver.Phone,
                            CityId = e.Receiver.CityId,
                            CountryId = e.Receiver.CountryId,
                            Address = e.Receiver.Address,
                            AddressDetails = e.Receiver.AddressDetails,
                            OtherAddress = e.Receiver.OtherAddress,
                            IsDefault = false,
                            PostalCode = e.Receiver.PostalCode,
                        },
                    },
                    e => e.CreatedDate,
                    true);

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving shipment", ex);
            }
        }
        public async Task<DtoShipment> GetShipmentByAdminAsync(Guid ShipmentId)
        {
            try
            {
                var data = await _unitOfWork.Repository<TbShipment>().GetByIdAsync(
                    e => e.Id == ShipmentId,          // بدون فلتر UserId
                    e => new DtoShipment
                    {
                        Id = e.Id,
                        ShippingDate = e.ShippingDate,
                        DeliveryDate = e.DeliveryDate,
                        ShippingTypeId = e.ShippingTypeId,
                        ShippingPackagingId = e.ShippingPackagingId,
                        PaymentMethodId = e.PaymentMethodId,
                        CarrierId = e.CarrierId,
                        UserSubscriptionId = e.UserSubscriptionId,
                        ReferenceId = e.ReferenceId,
                        Width = e.Width,
                        Height = e.Height,
                        Length = e.Length,
                        Weight = e.Weight,
                        TrackingNumber = e.TrackingNumber,
                        PackageValue = e.PackageValue,
                        ShippingRate = e.ShippingRate,
                        SenderId = e.SenderId,
                        ReceiverId = e.ReceiverId,
                        CurrentState = e.CurrentState,
                        Sender = e.Sender == null ? null : new DtoUserSender
                        {
                            Id = e.Sender.Id,
                            UserId = e.Sender.UserId,
                            SenderName = e.Sender.SenderName,
                            Email = e.Sender.Email,
                            Phone = e.Sender.Phone,
                            Contact = e.Sender.Contact,
                            CityId = e.Sender.CityId,
                            CountryId = e.Sender.CountryId,
                            Address = e.Sender.Address,
                            AddressDetails = e.Sender.AddressDetails,
                            OtherAddress = e.Sender.OtherAddress,
                            IsDefault = e.Sender.IsDefault,
                            PostalCode = e.Sender.PostalCode,
                        },
                        Receiver = e.Receiver == null ? null : new DtoUserReceiver
                        {
                            Id = e.Receiver.Id,
                            UserId = e.Receiver.UserId,
                            ReceiverName = e.Receiver.ReceiverName,
                            Email = e.Receiver.Email,
                            Phone = e.Receiver.Phone,
                            CityId = e.Receiver.CityId,
                            CountryId = e.Receiver.CountryId,
                            Address = e.Receiver.Address,
                            AddressDetails = e.Receiver.AddressDetails,
                            OtherAddress = e.Receiver.OtherAddress,
                            IsDefault = false,
                            PostalCode = e.Receiver.PostalCode,
                        },
                    },
                    e => e.CreatedDate,
                    true);

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving shipment", ex);
            }
        }
        public async Task<PagedResult<DtoShipment>> GetShipmentsList(int pageNumber, int pageSize, bool IsUserData, ShipmentStatusEnum? Status)
        {
            var userId = ResolveAuditUserId();
            try
            {
                int? InStatus = Status.HasValue ? (int?)Status.Value : null;

                var data = await _unitOfWork.Repository<TbShipment>()
                    .GetPagedList(
                        e => (!IsUserData || e.CreatedBy == userId)
                        && 
                        (!Status.HasValue || e.CurrentState == (int)Status.Value)
                        &&
                        (Status.HasValue || e.CurrentState != 1),
                        e => new DtoShipment
                        {
                            Id = e.Id,
                            ShippingDate = e.ShippingDate,
                            DeliveryDate = e.DeliveryDate,
                            SenderId = e.SenderId,
                            ReceiverId = e.ReceiverId,
                            ShippingTypeId = e.ShippingTypeId,
                            ShippingPackagingId = e.ShippingPackagingId,
                            PaymentMethodId = e.PaymentMethodId,
                            Weight = e.Weight,
                            Width = e.Width,
                            Height = e.Height,
                            Length = e.Length,
                            PackageValue = e.PackageValue,
                            ShippingRate = e.ShippingRate,
                            TrackingNumber = e.TrackingNumber,
                            CurrentState = e.CurrentState,
                            Sender = e.Sender == null ? null : new DtoUserSender
                            {
                                Id = e.Sender.Id,
                                SenderName = e.Sender.SenderName,
                                Email = e.Sender.Email,
                                Phone = e.Sender.Phone,
                                CityId = e.Sender.CityId,
                                CountryId = e.Sender.CountryId,
                            },
                            Receiver = e.Receiver == null ? null : new DtoUserReceiver
                            {
                                Id = e.Receiver.Id,
                                ReceiverName = e.Receiver.ReceiverName,
                                Email = e.Receiver.Email,
                                Phone = e.Receiver.Phone,
                                CityId = e.Receiver.CityId,
                                CountryId = e.Receiver.CountryId,
                            },
                        },
                        pageNumber,
                        pageSize,
                        e => e.CreatedDate,
                        true);

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving shipments", ex);
            }
        }
        public Guid ResolveAuditUserId()
        {
            try
            {
                return _UserService.GetLoggedInUser();
            }
            catch (UnauthorizedAccessException)
            {
                return Guid.Empty;
            }
        }
    }
}



