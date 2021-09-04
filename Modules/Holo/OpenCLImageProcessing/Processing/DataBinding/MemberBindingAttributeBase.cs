using System;

namespace Processing.DataBinding
{
    public abstract class MemberBindingAttributeBase : Attribute
    {
        public string TooltipText { get; set; }
        public string Group { get; set; }
        public int? Order { get; set; }

        /// <summary>
        /// Method to call when property changed via binded control
        /// </summary>
        public string OnPropertyChanged { get; set; }

        /// <summary>
        /// Event to listen to update binded control
        /// </summary>
        public string PropertyChangedEventName { get; set; }
    }
}
