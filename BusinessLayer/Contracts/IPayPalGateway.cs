using DataAccessLayer;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;

namespace BusinessLayer.Contracts
{
    public interface IPayPalGateway
    {
        Task<string> GetPaymentAccessTokenAsync();
        Task<(string, bool)> CreateOrder(CreatePaymentRequist Paymentrequist);
        Task<(string, bool)> CaptureOrde(string Orderid);
    }
}
