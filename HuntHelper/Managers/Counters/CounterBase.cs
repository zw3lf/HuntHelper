﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dalamud.Logging;

namespace HuntHelper.Managers.Counters;

public abstract class CounterBase
{
    public ushort MapID { get; init; }
    public List<(string Name, int Count)> Tally { get; init; }

    protected string[] NamesToMatch;

    protected string RegexPattern;

    protected CounterBase(string[] namesToMatch)
    {
        Tally = new List<(string Name, int Count)>(); //count and list.count is a bit potentially maybe confusing
        NamesToMatch = namesToMatch;
        AddCountRequirements();
    }

    private void AddOne(string name)
    {
        var index = Tally.FindIndex(i => i.Name == name);
        if (index == -1) return;
        Tally[index] = new(Tally[index].Name, Tally[index].Count + 1);
    }

    public void Reset()
    {
        Tally.Clear();
        AddCountRequirements();
    }

    public void TryAddFromLogLine(string msg)
    {
#if DEBUG
        // PluginLog.Error($"|{msg}|");
        // PluginLog.Error($"|{RegexPattern}|");
        // PluginLog.Error($"{Regex.IsMatch(msg, RegexPattern)}");
#endif
        if (Regex.IsMatch(msg, RegexPattern)) FindNameAndAdd(msg);
    }

    private void FindNameAndAdd(string msg)
    {
        foreach (var name in NamesToMatch)
        {
            //PluginLog.Error(name);
            if (!Regex.IsMatch(msg.ToLowerInvariant(), name.ToLowerInvariant())) continue;
            AddOne(name);
            return; //if a matching name is found, stop looking.
        }
    }
    private void AddCountRequirements()
    {
        foreach (var s in NamesToMatch)
        {
            Tally.Add((s, 0));
        }
    }

}