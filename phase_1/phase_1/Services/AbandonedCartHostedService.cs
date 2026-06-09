using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using phase_1.Repositories;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace phase_1.Services
{
    public class AbandonedCartHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AbandonedCartHostedService> _logger;

        public AbandonedCartHostedService(IServiceProvider serviceProvider, ILogger<AbandonedCartHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Abandoned Cart Hosted Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAbandonedCartsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing CheckAbandonedCartsAsync.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task CheckAbandonedCartsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

            var thresholdDate = DateTime.UtcNow.AddHours(-24);
            var abandonedCarts = await cartRepository.GetAbandonedCartsAsync(thresholdDate);

            foreach (var cart in abandonedCarts)
            {
                if (cart.User != null && !string.IsNullOrEmpty(cart.User.Email))
                {
                    var productListHtml = new StringBuilder("<ul>");
                    foreach (var item in cart.CartItems)
                    {
                        productListHtml.Append($"<li>{item.Product?.Name} - Số lượng: {item.Quantity}</li>");
                    }
                    productListHtml.Append("</ul>");

                    emailService.SendAbandonedCartEmail(cart.User.Email, cart.User.Name, productListHtml.ToString());

                    cart.IsReminderSent = true;
                    await cartRepository.UpdateCartAsync(cart);
                    
                    _logger.LogInformation($"Sent abandoned cart email to {cart.User.Email}");
                }
            }
        }
    }
}
