using System;

namespace EventContracts
{
    public interface SubmitOrder
    {
        Guid OrderId { get; }
        int ErrorId { get; }
    }
}