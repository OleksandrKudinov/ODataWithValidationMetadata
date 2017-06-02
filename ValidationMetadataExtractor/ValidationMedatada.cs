﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ValidationMetadataExtractor
{
    public sealed class ValidationMedatada
    {
        public ValidationMedatada(ValidationAttribute baseAttribute)
        {
            if (baseAttribute == null)
            {
                throw new ArgumentNullException(nameof(baseAttribute));
            }

            _baseAttribute = baseAttribute;
        }

        public String Name => _baseAttribute.GetType().Name.Replace("Attribute", String.Empty);
        public ICollection<ValidationAttributeParameter> Parameters => _baseAttribute.ExtractValidationAttributeParameters();

        private readonly ValidationAttribute _baseAttribute;
    }
}