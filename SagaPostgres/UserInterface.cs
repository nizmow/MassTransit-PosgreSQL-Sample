using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.NewIdProviders;
using MassTransit.Transports;
using Microsoft.Extensions.Hosting;
using SagaPostgres.Messages;

namespace SagaPostgres
{
    /// <summary>
    /// Seriously messy "UI" and provider of test messages.
    /// </summary>
    public class UserInterface
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public UserInterface(IPublishEndpoint publishEndpoint, IHostApplicationLifetime hostApplicationLifetime)
        {
            _publishEndpoint = publishEndpoint;
            _hostApplicationLifetime = hostApplicationLifetime;
        }
        
        public async Task Run()
        {
            await ShowMenu();
        }

        private async Task ShowMenu()
        {
            var stopping = false;
            
            // display a menu
            while (!stopping)
            {
                Console.WriteLine("Choose an option!");
                Console.WriteLine("1. Order, pay and be served");
                Console.WriteLine("2. Order, be served, and pay");
                Console.WriteLine("0. Quit");
                //Console.WriteLine("3. Dupe orders, be served, and pay (to cause contention)");
                var choice = Console.ReadKey();
                switch (choice.Key)
                {
                    case ConsoleKey.D1:
                        await OrderPayBeServed();
                        break;
                    case ConsoleKey.D2:
                        await OrderBeServedPay();
                        break;
                    case ConsoleKey.D0:
                        _hostApplicationLifetime.StopApplication();
                        stopping = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }

        private async Task OrderPayBeServed()
        {
            var orderId = Guid.NewGuid();

            await _publishEndpoint.Publish<OrderBeer>(new
            {
                OrderId = orderId,
                BeerType = "Victoria Bitter",
            });

            await Task.Delay(500);

            await _publishEndpoint.Publish<PayForBeer>(new
            {
                OrderId = orderId,
                Tip = GenerateTip()
            });

            await Task.Delay(500);

            await _publishEndpoint.Publish<ServeBeer>(new
            {
                OrderId = orderId
            });

            await Task.Delay(1000);
        }

        private async Task OrderBeServedPay()
        {
            var orderId = Guid.NewGuid();

            await _publishEndpoint.Publish<OrderBeer>(new
            {
                OrderId = orderId,
                BeerType = "Victoria Bitter",
            });

            await Task.Delay(500);

            await _publishEndpoint.Publish<ServeBeer>(new
            {
                OrderId = orderId
            });
            
            await Task.Delay(500);

            await _publishEndpoint.Publish<PayForBeer>(new
            {
                OrderId = orderId,
                Tip = GenerateTip()
            });

            await Task.Delay(1000);        }

        private static decimal GenerateTip()
        {
            // I decided to write a tip generator to keep life fun
            var random = new Random();
            var tipAmount = 0.0M;
            
            var shouldTip = random.Next(0, 3);
            if (shouldTip == 0)
            {
                tipAmount = (decimal) random.Next(100, 300) / 100;
            }

            return tipAmount;
        }
    }
}