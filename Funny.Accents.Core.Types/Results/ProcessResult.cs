using System;
using Newtonsoft.Json;

namespace Funny.Accents.Core.Types.Results
{
    public class ProcessResult : IProcessResult
    {
        public bool ProcessCompletionState { get; set; }
        public Exception ProcessResultException { get; set; }
        public string ProcessResultMessage { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ProcessResult<T> : IProcessResult<T>
    {
        [JsonProperty("process-completion-state", Required = Required.Always)]
        public bool ProcessCompletionState { get; set; }
        [JsonProperty("process-result-exception", Required = Required.Default)]
        public Exception ProcessResultException { get; set; }
        [JsonProperty("process-result-message", Required = Required.Always)]
        public string ProcessResultMessage { get; set; }
        [JsonProperty("process-result-value", Required = Required.Always)]
        public T ProcessResultValue { get; set; }
    }/*End of ProcessResult class*/

    [JsonObject(MemberSerialization.OptIn)]
    public class ProcessResult<TProcessState, TKProcessReturnType>
        : IProcessResult<TProcessState, TKProcessReturnType>
    {
        [JsonProperty("process-completion-state", Required = Required.Always)]
        public bool ProcessCompletionState { get; set; }

        [JsonProperty("process-result-exception", Required = Required.Default)]
        public Exception ProcessResultException { get; set; }

        [JsonProperty("process-result-message", Required = Required.Always)]
        public string ProcessResultMessage { get; set; }

        [JsonProperty("process-state-value", Required = Required.Always)]
        public TProcessState ProcessStateValue { get; set; }

        [JsonProperty("process-result-value", Required = Required.Always)]
        public TKProcessReturnType ProcessResultValue { get; set; }
    }/*End of ProcessResult class*/
}/*End of CmkTypes.Results namespace*/
