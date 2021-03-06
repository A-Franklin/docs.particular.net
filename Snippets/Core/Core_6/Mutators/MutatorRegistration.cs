﻿namespace Core6.Mutators
{
    using NServiceBus;

    class MutatorRegistration
    {
        MutatorRegistration(EndpointConfiguration endpointConfiguration)
        {
            #region MutatorRegistration
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall);
                });
            #endregion
        }

        public class MyMutator
        {
        }
    }
}

