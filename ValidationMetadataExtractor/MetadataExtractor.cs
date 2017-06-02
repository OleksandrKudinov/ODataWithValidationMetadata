using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ValidationMetadataExtractor
{
    static class MetadataExtractor
    {
        public static PropertyMetadata[] ExtractPropertiesMetadata(this Type objectType)

        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            PropertyMetadata[] propertiesMetadata = objectType.GetProperties()
                .Select(propertyInfo => new PropertyMetadata(propertyInfo))
                .ToArray();

            return propertiesMetadata;
        }

        public static ValidationMedatada[] ExtractValidationAttributesMetadata(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            ValidationAttribute[] validationAttributes = propertyInfo.GetCustomAttributes()
                .OfType<ValidationAttribute>()
                .ToArray();

            ValidationMedatada[] rules = validationAttributes
                .Select(x => x.ExtractValidationMetadata())
                .ToArray();

            return rules;
        }

        public static ValidationMedatada ExtractValidationMetadata(this ValidationAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return new ValidationMedatada(attribute);
        }

        public static Type[] AllowedTypes = new Type[]
        {
            typeof(Object),
            typeof(Boolean),
            typeof(Int32),
            typeof(Decimal),
            typeof(String)
        };

        public static ValidationAttributeParameter[] ExtractValidationAttributeParameters(this ValidationAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            PropertyInfo[] baseValidationAttrProps = typeof(ValidationAttribute)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.SetProperty | BindingFlags.GetProperty)
                .Where(propertyInfo => AllowedTypes.Any(type => type == propertyInfo.PropertyType))
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();

            //JUST FOR TEST
            //PropertyInfo[] allProperties = attribute.GetType().GetProperties();

            PropertyInfo[] currAttrProps = attribute.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => AllowedTypes.Any(z => z == x.PropertyType))
                .ToArray();

            PropertyInfo[] resultProps = currAttrProps
                .Concat(baseValidationAttrProps).ToArray();

            ValidationAttributeParameter[] validationAttributeParameters = resultProps
                .Where(x=>x.GetValue(attribute) != null)
                .Select(x => new ValidationAttributeParameter()
                {
                    //Type = x.PropertyType.Name,
                    Name = x.Name,
                    Value = x.GetValue(attribute)
                }).ToArray();

            return validationAttributeParameters;
        }
    }
}