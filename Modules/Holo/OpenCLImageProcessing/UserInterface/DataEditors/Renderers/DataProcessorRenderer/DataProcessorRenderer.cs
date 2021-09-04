using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Processing.DataBinding;
using Processing.DataProcessors;
using UserInterface.DataEditors.Tools;

namespace UserInterface.DataEditors.Renderers.DataProcessorRenderer
{
    [DataRendererFor(typeof(IDataProcessor))]
    public class DataProcessorRenderer : IDataRenderer
    {
        private IDataRenderer _outputDataRenderer;
        private IDataProcessor _dataProcessor;
        private DataProcessorInfo _dataProcessorInfo;

        public event Action OnUpdateRequest;
        public event Action UpdateControlsRequest;
        public Type DataType => typeof(IDataProcessor);

        [SubfieldGroup]
        public IDataRenderer OutputDataRenderer => _outputDataRenderer;
        [SubfieldGroup]
        public IDataProcessor DataProcessor => _dataProcessor;

        public void Resize(Size size)
        {
            _outputDataRenderer?.Resize(size);
        }

        public object GetData()
        {
            return _dataProcessor;
        }

        public IReadOnlyCollection<ITool> GetTools()
        {
            return _outputDataRenderer.GetTools();
        }

        public void SetData(object data)
        {
            var dataProcessor = data as IDataProcessor;

            _dataProcessor = dataProcessor ?? throw new InvalidOperationException($"Аргумент {nameof(data)} должен наследовать интерфейс {nameof(IDataProcessor)}");
            _dataProcessorInfo = new DataProcessorInfo(dataProcessor);
            _dataProcessor.Awake();

            if (_dataProcessorInfo.Outputs.Count > 1)
                throw new NotImplementedException();

            var output = _dataProcessorInfo.Outputs.Single();
            
            _outputDataRenderer = DataRendererUtil.GetRendererFor(output.Type);
            _outputDataRenderer.OnUpdateRequest += RequestUpdate;
            _outputDataRenderer.UpdateControlsRequest += () => UpdateControlsRequest?.Invoke();

            var outputObject = output.Get();
            if (outputObject != null)
                _outputDataRenderer.SetData(outputObject);

            _dataProcessor.Updated += DataProcessorUpdated;
        }

        private void DataProcessorUpdated()
        {
            var newOutput = _dataProcessorInfo.Outputs.Single().Get();
            if (newOutput != _outputDataRenderer.GetData())
            {
                _outputDataRenderer.SetData(newOutput);
                _outputDataRenderer.Update();
                RequestUpdate();
            }
        }

        public void Update()
        {
            _outputDataRenderer?.Update();
        }

        public string GetTitle() => _dataProcessorInfo.DisplayName;
        
        public void RequestUpdate()
        {
            OnUpdateRequest?.Invoke();
        }
        public void Dispose()
        {
            _outputDataRenderer?.Dispose();
            _outputDataRenderer = null;

            if (_dataProcessor != null)
                _dataProcessor.Updated -= DataProcessorUpdated;

            _dataProcessor = null;
            _dataProcessorInfo = null;
        }
    }
}
