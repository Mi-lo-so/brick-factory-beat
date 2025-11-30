namespace BrickFactoryBeat.Domain.Common.Exceptions;

/// <summary>
/// StateSettingException is thrown when there is an error setting the state of an entity.
/// This is unlikely to happen here, but if e.g. a state cannot go from red to green directly,
/// an attempt to request it would throw this exception.
/// </summary>
public class StateSettingException : Exception
{
    
}