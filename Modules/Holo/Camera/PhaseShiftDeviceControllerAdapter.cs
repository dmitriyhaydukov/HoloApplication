using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using Processing.DataBinding;
using rab1;

namespace Camera
{
    public class PhaseShiftDeviceControllerAdapter
    {
        private const short ZeroPhaseShiftValue = 0x2000;
        private bool _phaseShiftDeviceConnected = false;
        private PhaseShiftDeviceController _inner;

        public ListWithEvents<string> PortNames { get; }

        public PhaseShiftDeviceControllerAdapter()
        {
            PortNames = new ListWithEvents<string>();
            UpdatePortNames();
        }

        public void UpdatePortNames()
        {
            PortNames.Clear();
            PortNames.AddRange(SerialPort.GetPortNames());
        }

        public void Connect(string port)
        {
            if (!_phaseShiftDeviceConnected)
            {
                _inner = new PhaseShiftDeviceController(port);
                _inner.Initialize();
                _phaseShiftDeviceConnected = true;
            }
        }

        public void SetShift(float shift)
        {
            if (shift < 0 || shift > short.MaxValue - ZeroPhaseShiftValue)
                throw new InvalidOperationException();

            if (_phaseShiftDeviceConnected)
                _inner.SetShift((short)((short) shift + ZeroPhaseShiftValue));
        }

        public void SetShift(float step, int stepNumber, float delay)
        {
            if (!_phaseShiftDeviceConnected)
                return;

            SetShift(step * stepNumber);
            Thread.Sleep((int) delay); // TODO: сделать ожидание не блокирующим!
        }

        public void Disconnect()
        {
            _inner?.Dispose();
            _inner = null;
            _phaseShiftDeviceConnected = false;
        }

        ~PhaseShiftDeviceControllerAdapter()
        {
            _inner?.Dispose();
            _inner = null;
        }
    }
}
