using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using Cloo;
using Infrastructure;
using OpenTK;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors.Test
{
    [DataProcessor(Group = "Test", Name = "Test Fourier", Tooltip = "Тест Преобразования Фурье")]
    public class TestFourierProcessor : SingleImageOutputDataProcessorBase
    {
        private const int PlotHeight = 128;
       // private const int InputDataLength = 16;
        private Vector2[] _inputData;
        private Bitmap _testOutput;

        [Number("Размер массива", 2, 1024, 1)]
        public float InputDataLength { get; set; } = 16;

        [Action("Обновить")]
        public void Refresh()
        {
            Test();
        }

        public TestFourierProcessor()
        {
        }

        public override void Awake()
        {
            Test();
        }

        public override void FreeResources()
        {
            
        }

        public void Test()
        {
            _inputData = new Vector2[(int)InputDataLength];
            for (int i = 0; i < _inputData.Length; i++)
            {
                _inputData[i] = new Vector2(i);
            }

            var arrayDrawer = new ArrayDrawer();
            var plotSize = arrayDrawer.MeasurePlotSize(PlotHeight, _inputData.Length);
            _testOutput = new Bitmap(plotSize.Width * 2, plotSize.Height * 4);
            var g = Graphics.FromImage(_testOutput);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.FillRectangle(Brushes.White, 0, 0, _testOutput.Width, _testOutput.Height);
            
            var app = Singleton.Get<OpenClApplication>();
            var computeBuffer1 = new ComputeBuffer<Vector2>(app.ComputeContext, ComputeMemoryFlags.None, _inputData);
            var computeBuffer2 = new ComputeBuffer<Vector2>(app.ComputeContext, ComputeMemoryFlags.None, _inputData.Length);

            int n = _inputData.Length;
            int m = Fourier.OddDenominator(n);
            int l = n / m;
            int t = Fourier.PowerOfTwo(l);
            app.Queue.WriteToBuffer(_inputData, computeBuffer1, true, null);
            app.ExecuteKernel("splitFftA", _inputData.Length, 1, computeBuffer1, computeBuffer2, n, m, l, t);
            var splittedArray = new Vector2[_inputData.Length];
            app.Queue.ReadFromBuffer(computeBuffer2, ref splittedArray, true, null);

            var expectedSplitStepResult = TestCpuFourier.Split(_inputData.ToComplex()).ToVector2();
            var expectedSplitAndFftResult = TestCpuFourier.SplitAndFft(_inputData.ToComplex()).ToVector2();

            //app.Queue.WriteToBuffer(expectedSplitStepResult, computeBuffer2, true, null);
            Fft(computeBuffer2, l, m);
            var fftResultArray = new Vector2[_inputData.Length];
            app.Queue.ReadFromBuffer(computeBuffer2, ref fftResultArray, true, null);

            app.ExecuteKernel("mergeFftStep1A", _inputData.Length, 1, computeBuffer2, computeBuffer1, n, m, l);
            var mergedArray = new Vector2[_inputData.Length];
            app.Queue.ReadFromBuffer(computeBuffer1, ref mergedArray, true, null);

            
            var expectedBpfStep1Result = TestCpuFourier.BPF_Step1(_inputData.ToComplex());
            var expectedBpfStep1ResultVec2 = expectedBpfStep1Result.ToVector2();
            var expectedBpfStep2ResultVec2 = TestCpuFourier.BPF_Step2(expectedBpfStep1Result).ToVector2();

            int y = arrayDrawer.Draw(_testOutput, splittedArray, "Split Result", PlotHeight, g: g).Y;
            arrayDrawer.Draw(_testOutput, expectedSplitStepResult, "Expected Split Result", PlotHeight, 0, _testOutput.Width / 2, g);

            arrayDrawer.Draw(_testOutput, expectedSplitAndFftResult, "Expected FFT Result", PlotHeight, y, _testOutput.Width / 2, g);
            y = arrayDrawer.Draw(_testOutput, fftResultArray, "FFT Result", PlotHeight, y, g: g).Y;

            arrayDrawer.Draw(_testOutput, expectedBpfStep1ResultVec2, "Expected Merge FFT Step 1", PlotHeight, y, _testOutput.Width / 2, g);
            y = arrayDrawer.Draw(_testOutput, mergedArray, "Merge FFT Step 1 (Merge Arrays)", PlotHeight, top: y, g: g).Y;

            // step2 test
            //app.Queue.WriteToBuffer(expectedBpfStep1ResultVec2, computeBuffer1, true, null);
            app.ExecuteKernel("mergeFftStep2A", n, 1, computeBuffer1, computeBuffer2, n, m, l);
            var step2Result = new Vector2[_inputData.Length];
            app.Queue.ReadFromBuffer(computeBuffer2, ref step2Result, true, null);

            arrayDrawer.Draw(_testOutput, expectedBpfStep2ResultVec2, "Expected Merge FFT Step2 Result", PlotHeight, y, _testOutput.Width / 2, g);
            y = arrayDrawer.Draw(_testOutput, step2Result, "Actual Merge FFT Step2 Result", PlotHeight, top: y, g: g).Y;
            
            Output = ImageHandler.FromBitmap(_testOutput);
            Output.UploadToComputingDevice();
            Update();
            OnUpdated();
        }

        private void Fft(ComputeBuffer<Vector2> input, int n, int height)
        {
            var app = Singleton.Get<OpenClApplication>();
            var spinFactBuffer = new ComputeBuffer<Vector2>(app.ComputeContext, ComputeMemoryFlags.None, n / 2);
            app.ExecuteKernel("spinFact", n / 2, 1, spinFactBuffer, n);
            int t = (int) Math.Log(n, 2);
            for (var i = 1; i <= t; i++)
            {
                app.ExecuteKernel("butterfly", n/2, height, input, spinFactBuffer, t, n, i, 0);
            }
        }
    }
}
