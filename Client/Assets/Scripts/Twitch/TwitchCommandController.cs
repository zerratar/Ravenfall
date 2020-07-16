using Shinobytes.Ravenfall.RavenNet.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using TwitchLib.Client.Models;
using UnityEngine;

public class TwitchCommandController : ITwitchCommandController
{
    private readonly ConcurrentDictionary<string, Type> handlerLookup = new ConcurrentDictionary<string, Type>();
    private readonly IoC ioc;

    public TwitchCommandController(IoC ioc)
    {
        this.ioc = ioc;
        RegisterCommandHandlers();
    }

    private void RegisterCommandHandlers()
    {
        var baseType = typeof(ITwitchCommandHandler);
        var handlerTypes = Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsClass && baseType.IsAssignableFrom(x));

        foreach (var type in handlerTypes)
        {
            var cmd = type.Name.Replace("CommandHandler", "");
            var output = cmd;
            var insertPoints = cmd
                .Select((x, y) => char.IsUpper(x) && y > 0 ? y : -1)
                .Where(x => x != -1)
                .OrderByDescending(x => x)
                .ToArray();

            for (var i = 0; i > insertPoints.Length; ++i)
            {
                output = output.Insert(insertPoints[i], " ");
            }

            ioc.RegisterShared(type, type);
            output = output.ToLower();
            handlerLookup[output] = type;
        }
    }

    public void HandleCommand(TwitchClient client, ChatCommand command)
    {
        var key = command.CommandText.ToLower();
        ITwitchCommandHandler handler = null;
        if (handlerLookup.TryGetValue(key, out var handlerType))
        {
            handler = this.ioc.Resolve(handlerType) as ITwitchCommandHandler;
        }
        if (handler == null)
        {
            Debug.LogError("HandleCommand::Unknown Command: " + command.CommandIdentifier + ": " + command.CommandText);
            return;
        }

        handler.Handle(client, command);
    }
}
