using System;

namespace DocumentBuilder
{
    /// <summary>
    ///     Defines an attribute which helps determine where to place the data with it's host PDF.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class StampAttribute : Attribute
    {
        /// <summary>
        ///     Creates a default <see cref="Stamp" />.
        /// </summary>
        /// <param name="fieldName">The name of the PDF field where the data will be placed.</param>
        public StampAttribute(string fieldName, string format = null)
        {
            FieldName = fieldName;
            Format = format;
        }

        /// <summary>
        ///     Gets or sets the field name.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///     Gets or sets the data format.
        /// </summary>
        public string Format { get; set; }
    }
}