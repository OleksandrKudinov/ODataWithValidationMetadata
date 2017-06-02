using System;
using System.Collections.Generic;
using System.Reflection;

namespace ValidationMetadataExtractor
{
    public sealed class PropertyMetadata
    {
        public PropertyMetadata(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            _propertyInfo = propertyInfo;
        }

        public String PropertyName => _propertyInfo.Name;
        public String PropertyTypeName => _propertyInfo.PropertyType.Name;
        public ICollection<ValidationMedatada> ValidationRules => _propertyInfo.ExtractValidationAttributesMetadata();

        private readonly PropertyInfo _propertyInfo;
    }
}