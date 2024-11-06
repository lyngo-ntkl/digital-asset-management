using DigitalAssetManagement.UseCases.Common;
using Hangfire;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Infrastructure.Hangfire
{
    public class SchedulerImplementation(IBackgroundJobClient backgroundJobClient) : IScheduler
    {
        private readonly IBackgroundJobClient _client = backgroundJobClient;
        public void ScheduleAfterTimeInterval(Expression<Action> action, TimeSpan delay)
        {
            _client.Schedule(action, delay);
        }
    }
}
