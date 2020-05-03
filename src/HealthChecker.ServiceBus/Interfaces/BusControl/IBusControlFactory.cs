using System;

namespace HealthChecker.ServiceBus.Interfaces.BusControl
{
    public interface IBusControlFactory
    {
        IBusControl Create(Action<BusControlConfigurator> configure);
    }
}
