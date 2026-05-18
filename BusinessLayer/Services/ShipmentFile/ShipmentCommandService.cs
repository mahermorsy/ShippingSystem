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
    public class ShipmentCommandService : BaseService<TbShipment, DtoShipment>, IShipmentCommand
    {
        private readonly IShipmentStatus _shipmentStatus;
        private readonly IUserReceiver _userreciver;
        private readonly IUserSender _usersender;
        private readonly ITrackingNumber _trackingNumber;
        private readonly IRateCalculator _rateCalculator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _UserService;

        public ShipmentCommandService   (
            IUnitOfWork unitOfWork,
            IGenericRepository<TbShipment> GenericRepo,
            IMapper mapper,
            IUserService UserService,
            IUserReceiver userreciver,
            IUserSender usersender,
            ITrackingNumber trackingNumber,
            IShipmentStatus shipmentStatus,


            IRateCalculator rateCalculator) : base(unitOfWork, mapper, UserService)
        {
            _unitOfWork = unitOfWork;
            _userreciver = userreciver;
            _usersender = usersender;
            _trackingNumber = trackingNumber;
            _UserService = UserService;
            _rateCalculator = rateCalculator;
            _shipmentStatus = shipmentStatus;
        }
        public async Task<bool> CreateShipment(DtoShipment dtoShipment)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var auditUserId = ResolveAuditUserId();

                dtoShipment.ShippingRate = await _rateCalculator.CalculateRate(dtoShipment);
                dtoShipment.TrackingNumber = _trackingNumber.CreateTrackingNumber(dtoShipment);

                if (dtoShipment.SenderId == Guid.Empty)
                {
                    if (dtoShipment.Sender != null && dtoShipment.Sender.UserId == Guid.Empty)
                    {
                        dtoShipment.Sender.UserId = auditUserId;
                    }

                    var (success, entityId) = await _usersender.AddAsyncWithID(dtoShipment.Sender, auditUserId);

                    if (!success)
                    {
                        throw new Exception("Failed to create sender");
                    }

                    dtoShipment.SenderId = entityId;
                }

                if (dtoShipment.ReceiverId == Guid.Empty)
                {
                    if (dtoShipment.Receiver != null && dtoShipment.Receiver.UserId == Guid.Empty)
                    {
                        dtoShipment.Receiver.UserId = auditUserId;
                    }

                    var (success, entityId) = await _userreciver.AddAsyncWithID(dtoShipment.Receiver, auditUserId);

                    if (!success)
                    {
                        throw new Exception("Failed to create receiver");
                    }

                    dtoShipment.ReceiverId = entityId;
                }

                Guid NShipmentId = Guid.NewGuid();
                var result = await AddAsyncWithID(dtoShipment, auditUserId);
                await _shipmentStatus.AddShipmentStatus(result.EntityId, ShipmentStatusEnum.Created, "Shipment created");

                await _unitOfWork.CommitAsync();
                return result.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("An error occurred while creating the shipment.", ex);
            }
        }
        public async Task<bool> EditAsync(DtoShipment dtoShipment)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                dtoShipment.ShippingRate = await _rateCalculator.CalculateRate(dtoShipment);

                await _usersender.UpdateAsync(dtoShipment.Sender);
                await _userreciver.UpdateAsync(dtoShipment.Receiver);

                await UpdateAsync(dtoShipment);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("An error occurred while updating the shipment.", ex);
            }
        }
        public async Task EditFieldsAsync(Guid ShipId, Action<TbShipment> updateAction)
        {
           await _unitOfWork.Repository<TbShipment>().UpdateFieldsync(ShipId,updateAction);
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



