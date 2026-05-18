using AutoMapper;
using DataAccessLayer;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;
using DataAccessLayer.RefreshToken;

namespace BusinessLayer.Mapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
         
            CreateMap<TbShippingPackaging, DTOShippingPackaging>().ReverseMap();
            CreateMap<TbRefreshToken, DTORefreashToken>().ReverseMap();
            CreateMap<TbCarrier, DtoCarrier>().ReverseMap();
            CreateMap<VwCity, VwCityDTO>().ReverseMap();
            CreateMap<TbCity, DtoCity>().ReverseMap();
            CreateMap<VwCity, DtoCity>().ReverseMap();
            CreateMap<TbCountry, DtoCounty>().ReverseMap();
            CreateMap<TbPaymentMethod, DtoPaymentMethod>().ReverseMap();
            CreateMap<TbSetting, DtoSetting>().ReverseMap();
            CreateMap<TbShippingType, DtoShippingType>().ReverseMap();
            CreateMap<TbShipment, DtoShipment>().ReverseMap()
                    .ForMember(dest => dest.Sender, opt => opt.Ignore())
                    .ForMember(dest => dest.Receiver, opt => opt.Ignore())
                    .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore())
                    .ForMember(dest => dest.ShippingType, opt => opt.Ignore())
                    .ForMember(dest => dest.ShippingPackaging, opt => opt.Ignore())
                    .ForMember(dest => dest.TbShipmentStatuses, opt => opt.Ignore());

            CreateMap<TbShipmentStatus, DtoShipmentStatus>().ReverseMap();
            CreateMap<TbSubscriptionPackage, DtoSubscriptionPackage>().ReverseMap();
            CreateMap<TbUserReceiver, DtoUserReceiver>().ReverseMap();
            CreateMap<TbUserSender, DtoUserSender>().ReverseMap();
            CreateMap<TbUserSubscription, DtoUserSubscription>().ReverseMap();
        }
    }
}
