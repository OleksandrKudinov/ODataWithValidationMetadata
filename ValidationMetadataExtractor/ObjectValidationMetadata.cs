using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ValidationMetadataExtractor
{
    public sealed class ObjectValidationMetadata
    {
        public ObjectValidationMetadata(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            _type = objectType;
        }

        public String TypeName => _type.Name;

        public ICollection<PropertyValidationMetadata> Properties => _type.ExtractPropertiesMetadata();

        private readonly Type _type;
    }
}