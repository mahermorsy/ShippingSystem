using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.Payment
{
    public class PaymobGetAway : IPayPalGateway
    {
        public Task<(string, bool)> CaptureOrde(string Orderid)
        {
            throw new NotImplementedException();
        }

        public Task<(string, bool)> CreateOrder(CreatePaymentRequist Paymentrequist)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPaymentAccessTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
