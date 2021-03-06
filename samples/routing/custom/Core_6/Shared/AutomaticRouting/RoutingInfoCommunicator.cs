﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class RoutingInfoCommunicator :
    FeatureStartupTask
{
    Dictionary<string, Entry> cache = new Dictionary<string, Entry>();
    SqlDataAccess dataAccess;
    Timer timer;

    public RoutingInfoCommunicator(SqlDataAccess dataAccess)
    {
        this.dataAccess = dataAccess;
    }

    public Func<Entry, Task> Changed = entry => Task.CompletedTask;
    public Func<Entry, Task> Removed = entry => Task.CompletedTask;

    protected override Task OnStart(IMessageSession messageSession)
    {
        timer = new Timer(state =>
        {
            Refresh()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    async Task Refresh()
    {
        var addedOrUpdated = new List<Entry>();
        var results = await dataAccess.Query()
            .ConfigureAwait(false);
        var removed = cache.Values
            .Where(x => results.All(r => r.Publisher != x.Publisher))
            .ToArray();
        foreach (var entry in results)
        {
            Entry oldEntry;
            var key = entry.Publisher;
            if (!cache.TryGetValue(key, out oldEntry) || oldEntry.Data != entry.Data)
            {
                cache[key] = entry;
                addedOrUpdated.Add(entry);
            }
        }
        foreach (var change in addedOrUpdated)
        {
            await Changed(change)
                .ConfigureAwait(false);
        }
        foreach (var entry in removed)
        {
            cache.Remove(entry.Publisher);
            await Removed(entry)
                .ConfigureAwait(false);
        }
    }

    protected override Task OnStop(IMessageSession messageSession)
    {
        using (var waitHandle = new ManualResetEvent(false))
        {
            timer.Dispose(waitHandle);
            waitHandle.WaitOne();
        }
        return Task.CompletedTask;
    }

    public Task Publish(string data)
    {
        return dataAccess.Publish(data);
    }
}