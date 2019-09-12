using System.Collections.Generic;

namespace Library.API.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; }
        public bool Revert { get; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            this.DestinationProperties = destinationProperties;
            this.Revert = revert;
        }
    }
}