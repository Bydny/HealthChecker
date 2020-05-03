using System.Threading.Tasks;

namespace HealthChecker.Api.Services.Interfaces.BackgroundExecution
{
    public interface IJobAsync
    {
        Task Execute();
    }
}
