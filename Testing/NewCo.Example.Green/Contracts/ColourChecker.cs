using System;

namespace Contracts
{
    public record ColourChecker
    {
        public Guid? CorrelationId { get; set; }
        public string Colour { get; init; }
        public bool? IsValid { get; set; }
    }
}