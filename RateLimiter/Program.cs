
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace RateLimiter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("IpLimiter", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(20)
                        }
                    )
                );


                options.AddPolicy("UserLimiter", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity!.Name,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(20)
                        }
                    )
                );



                options.AddConcurrencyLimiter("Concurrency", option =>
                {
                    option.PermitLimit = 2;
                    option.QueueLimit = 1;
                    option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                options.AddTokenBucketLimiter("Token" , option =>
                {
                    option.TokenLimit = 2;
                    option.QueueLimit = 1;
                    option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    option.TokensPerPeriod = 2;
                    option.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                    option.AutoReplenishment = true;
                });

                options.AddFixedWindowLimiter("FixedLimiter", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 2;
                    limiterOptions.Window = TimeSpan.FromSeconds(20);
                    limiterOptions.QueueLimit = 1;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                options.AddSlidingWindowLimiter("SlidingLimiter", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 2;
                    limiterOptions.Window = TimeSpan.FromSeconds(20);
                    limiterOptions.SegmentsPerWindow = 2;
                    limiterOptions.QueueLimit = 1;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

            });

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRateLimiter();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
