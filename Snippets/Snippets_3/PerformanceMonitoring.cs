﻿namespace Snippets3
{
    using NServiceBus;

    public class PerformanceMonitoring
    {
        public void EnablingCriticalTime()
        {
            #region enable-criticaltime
            Configure configure = Configure.With();
            // in this version there was no granular control over individual counters
            configure.EnablePerformanceCounters();
            #endregion
        }

        #region enable-sla-host-attribute

        [EndpointSLA("00:03:00")]
        public class EndpointConfig : IConfigureThisEndpoint
        {
        #endregion

            public void Customize(Configure configure)
            {
            }
        }
    }
}