using System;
using System.Threading.Tasks;

namespace Gherkin.Testing.Utils
{   

public class RetryLoop
    {             
        private int _retriesPerSecond;
        private int _seconds = 1;

        private RetryLoop(int retriesPerSecond)
        {
            _retriesPerSecond = retriesPerSecond;
        }

        public static RetryLoop TriesPerSecond(int retries)
        {
            return new RetryLoop(retries);
        }

        public RetryLoop ForSeconds(int seconds)
        {
            _seconds = seconds;          
            return this;
        }

        public void Execute(Action action)
        {
            var retries = _retriesPerSecond * _seconds;
            var waitMillis = (_seconds * 1000) / retries;
            new Looper(retries, waitMillis).Execute(action);
        }
    }

    public class Looper
    {
        private int _retries;
        private int _waitMilliseconds;

        public Looper(int retries, int waitMilliseconds)
        {
            _retries = retries;
            _waitMilliseconds = waitMilliseconds;
        }

        public void Execute(Action test)
        {
            bool continueLoop = true;
            var counter = 0;

            while (continueLoop && counter <= _retries)
            {
                try
                {
                    continueLoop = false;
                    test();
                }
                catch (Exception) 
                {
                    // we have an inconsistent result that might produce any exception, so we catch everything
                    continueLoop = true;
                }

                if (continueLoop)
                {
                    counter++;
                    Task.Delay(_waitMilliseconds).GetAwaiter().GetResult();
                }
            }
            // the final run
            test();
        }
    }
}
