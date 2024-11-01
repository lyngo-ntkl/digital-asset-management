using System.Linq.Expressions;

namespace DigitalAssetManagement.UseCases.Common
{
    public interface Scheduler
    {
        void ScheduleAfterTimeInterval(Expression<Action> methodCall, TimeSpan delay);
    }
}
