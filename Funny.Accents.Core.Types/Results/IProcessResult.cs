using System;

namespace Funny.Accents.Core.Types.Results
{
    public interface IProcessResult
    {
        bool ProcessCompletionState { get; set; }
        Exception ProcessResultException { get; set; }
        string ProcessResultMessage { get; set; }
    }

    public interface IProcessResult<T> : IProcessResult
    {
        T ProcessResultValue { get; set; }
    }

    public interface IProcessResult<TStateType, TKReturnType> : IProcessResult
    {
        TStateType ProcessStateValue { get; set; }
        TKReturnType ProcessResultValue { get; set; }
    }
}/*End of CmkTypes.Results namespace*/