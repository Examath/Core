using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Model
{
   /// <summary>
   /// Represents an exception thrown when an object reference for a
   /// specific identifier could not be found during initialization.   /// 
   /// </summary>
    public sealed class ObjectLinkingException : Exception
    {
        /// <summary>
        /// The object attempting to make a link using it's <see cref="TargetIdentifier"/>
        /// </summary>
        public object Subject { get; private set; }

        /// <summary>
        /// The ID of the target object that cannot be found
        /// </summary>
        public object TargetIdentifier { get; private set; }

        /// <summary>
        /// The type of the target object
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// Creates a <see cref="ObjectLinkingException"/> specifying the subject, target identifier and type.
        /// </summary>
        /// <param name="subject">The object attempting to make a link using it's <see cref="TargetIdentifier"/></param>
        /// <param name="targetIdentifier">The ID of the target object that cannot be found</param>
        /// <param name="targetType">The type of the target object</param>
        public ObjectLinkingException(object subject, object targetIdentifier, Type targetType)
            : base($"Linking failure initializing {subject}: Could not find {targetType.Name} with ID '{targetIdentifier}'")
        {
            Subject = subject;
            TargetIdentifier = targetIdentifier;
            TargetType = targetType;
        }

        /// <summary>
        /// Creates a <see cref="ObjectLinkingException"/> specifying the subject, target identifier and type, and inner exception.
        /// </summary>
        /// <param name="subject">The object attempting to make a link using it's <see cref="TargetIdentifier"/></param>
        /// <param name="targetIdentifier">The ID of the target object that cannot be found</param>
        /// <param name="targetType">The type of the target object</param>
        /// <param name="innerException"><inheritdoc/></param>
        public ObjectLinkingException(object subject, object targetIdentifier, Type targetType, Exception innerException)
            : base($"Linking failure initializing {subject}: Could not find {targetType.Name} with ID '{targetIdentifier}'", innerException)
        {
            Subject = subject;
            TargetIdentifier = targetIdentifier;
            TargetType = targetType;
        }
    }
}
