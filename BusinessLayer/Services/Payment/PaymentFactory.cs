using BusinessLayer.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.Payment
{
    public class PaymentFactory
    {
        IServiceProvider _serviceProvider;
        public PaymentFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPayPalGateway CreatePayPalGateway(string CountyCode)
        {
            if (CountyCode == "EG")
            {
                return (IPayPalGateway)_serviceProvider.GetRequiredService(typeof(PaymobGetAway));
            }
            else {
                return (IPayPalGateway)_serviceProvider.GetRequiredService(typeof(PayPalGateway));
            }
        }
          
        }
    
}
