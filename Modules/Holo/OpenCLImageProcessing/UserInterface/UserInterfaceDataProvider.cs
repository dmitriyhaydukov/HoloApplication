using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing;

namespace UserInterface
{
    public delegate void PreviewDataUpdated<TData>(TData previewData, UserInterfaceDataProvider<TData> provider);
    public abstract class UserInterfaceDataProvider<TData>: DataProviderBase<TData>
    {
        public event PreviewDataUpdated<TData> OnPreviewDataUpdated; 

        public UserInterfaceDataProviderSettingsBase Settings { get; set; }
        public abstract void ShowInterface();

        public virtual void TriggerPreviewDataUpdatedEvent(TData previewData)
        {
            OnPreviewDataUpdated?.Invoke(previewData, this);
        }
    }

    public abstract class UserInterfaceDataProviderSettingsBase
    {
        
    }
}
