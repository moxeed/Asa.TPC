using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asa.Saga
{
    interface ISagaStep {
        Task Process();
        Task Compensation();
        ISagaStep Next { get; }
    }

    class SagaStep : ISagaStep
    {
        private Func<Task> _process;
        private Func<Task> _compensation;
        public SagaStep Next { get; private set; }
        ISagaStep ISagaStep.Next => Next;

        public SagaStep()
        {
            static Task empty() => Task.CompletedTask;
            _process = empty;
            _compensation = empty;
        }

        public SagaStep WithProcess(Func<Task> process) 
        {
            _process = process;
            return this;
        }

        public SagaStep WithCompensation(Func<Task> compensation)
        {
            _compensation = compensation;
            return this;
        }

        public SagaStep Step() { 
            Next = new SagaStep();
            return Next;
        }
        public Task Process() => _process();
        public Task Compensation() => _compensation();
    }

    abstract class Saga
    {
        private readonly Func<Task> _onCompelete;
        private readonly Func<Task> _onReject;
        protected ISagaStep Start { get; private set; }

        protected Saga(Func<Task> onCompelete, Func<Task> onReject)
        {
            _onCompelete = onCompelete;
            _onReject = onReject;
            Initilizae();
        }

        public SagaStep Step() {
            var step = new SagaStep();
            Start = step;
            return step;
        }

        public Task Run() 
        {
            try
            {
               return Exceute(Start);
            }
            catch 
            {
               return Reject();
            }
        }

        private async Task Exceute(ISagaStep step) 
        {
            if (step is null) 
            {
                await Complete();
                return;
            }
            await step.Process();
            try
            {
            await Exceute(step.Next);
            }
            catch 
            {
                await step.Compensation();
                throw;
            }
        }

        protected abstract void Initilizae();
        private Task Complete() => _onCompelete();
        private Task Reject() => _onReject();
    }
}
