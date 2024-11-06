using System.Linq.Expressions;

namespace DigitalAssetManagement.UseCases.Common
{
    public interface IScheduler
    {
        void ScheduleAfterTimeInterval(Expression<Action> action, TimeSpan delay);
    }
}
