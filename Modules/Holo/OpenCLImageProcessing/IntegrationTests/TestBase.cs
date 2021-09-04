using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTests
{
    public abstract class TestBase
    {
        public abstract void SetupTestData();
        public abstract void Execute();

        public void Run()
        {
            SetupTestData();

            try
            {
                Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
