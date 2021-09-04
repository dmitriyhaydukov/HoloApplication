using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing
{
    public delegate void DataReadyEvent(object data, IDataProvider provider);
    public interface IDataProvider
    {
        Type DataType { get; }

        event DataReadyEvent OnDataReady;

    }

    public interface IDataProvider<out TData>: IDataProvider
    {
        TData Data { get; }
    }

    public abstract class DataProviderBase<TData>: IDataProvider<TData>
    {
        public virtual TData Data { get; protected set; }
        public virtual Type DataType => typeof(TData);

        public event DataReadyEvent OnDataReady;

        protected virtual void TriggerDataReadyEvent()
        {
            OnDataReady?.Invoke(Data, this);
        }
    }
}
