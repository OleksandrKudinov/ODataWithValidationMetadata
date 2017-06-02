using System;
using System.Collections.Generic;

namespace ValidationMetadataExtractor
{
    public sealed class ObjectMetadata
    {
        public ObjectMetadata(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            _type = objectType;
        }

        public String TypeName => _type.Name;

        public ICollection<PropertyMetadata> Properties => _type.ExtractPropertiesMetadata();

        private readonly Type _type;
    }
}