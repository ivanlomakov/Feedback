using System;

namespace Conference.Core
{
    public interface IRespondent
    {
        Guid SessionId { get; }
    }
}