using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ValidationMetadataExtractor
{
    static class MetadataExtractor
    {
        public static PropertyValidationMetadata[] ExtractPropertiesMetadata(this Type objectType)

        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            PropertyValidationMetadata[] propertiesValidationMetadata = objectType.GetProperties()
                .Select(propertyInfo => new PropertyValidationMetadata(propertyInfo))
                .ToArray();

            return propertiesValidationMetadata;
        }

        public static ValidationRuleMedatada[] ExtractValidationAttributesMetadata(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            ValidationAttribute[] validationAttributes = propertyInfo.GetCustomAttributes()
                .OfType<ValidationAttribute>()
                .ToArray();

            ValidationRuleMedatada[] rules = validationAttributes
                .Select(x => x.ExtractValidationMetadata())
                .ToArray();

            return rules;
        }

        public static ValidationRuleMedatada ExtractValidationMetadata(this ValidationAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return new ValidationRuleMedatada(attribute);
        }

        public static Type[] AllowedTypes = new Type[]
        {
            typeof(Object),
            typeof(Boolean),
            typeof(Int32),
            typeof(Decimal),
            typeof(String)
        };

        public static ValidationRuleParameter[] ExtractValidationAttributeParameters(this ValidationAttribute attribute)
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

            ValidationRuleParameter[] validationRuleParameters = resultProps
                .Where(x=>x.GetValue(attribute) != null)
                .Select(x => new ValidationRuleParameter()
                {
                    //Type = x.PropertyType.Name,
                    Name = x.Name,
                    Value = x.GetValue(attribute)
                }).ToArray();

            return validationRuleParameters;
        }
    }
}