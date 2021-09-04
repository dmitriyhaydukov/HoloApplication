using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using OpenTK;
using Processing;

namespace IntegrationTests.KernelTests
{
    public class SplitFftTest : KernelTestBase
    {
        private int[] _testInput;

        private IImageHandler _imageHandler;

        public override void SetupTestData()
        {
            _imageHandler = CreateImageFromIntArray(_testInput);

        }

        public override void Execute()
        {
        }
    }
}
