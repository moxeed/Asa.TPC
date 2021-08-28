using Asa.Core.Domain;
using Asa.Persistence;
using Asa.TPC;
using Asa.TPC.ExternalService.Block;
using System.Threading.Tasks;

namespace Asa.Saga
{
    class CreateOrderSaga : Saga
    {
        private readonly int _amount;
        private readonly IUnitOfWork _unitOfWork;
        private Decision _decision;

        public CreateOrderSaga(int amount) : base(onCompelete, onReject)
        {
            _amount = amount;
            _unitOfWork = UnitOfWorkFactory.CreateUnitOfWork();
        }

        private static Task onReject() => Task.CompletedTask;
        private static Task onCompelete() => Task.CompletedTask;

        protected override void Initilizae()
        {
            Step()
            .WithProcess(CreateOrder)
            .WithCompensation(CancelOrder)
            .Step()
            .WithProcess(Block)
            .WithCompensation(ReleaseBlock)
            .Step()
            .WithProcess(SendToBourse);
        }

        private Task CreateOrder() {
            _decision = new Decision(_amount);
            _unitOfWork.OrderRepository.Save(_decision);
            _unitOfWork.Commit();
            return Task.CompletedTask;
        }

        private Task CancelOrder() 
        {
            _decision.Reject();
            _unitOfWork.OrderRepository.Save(_decision);
            _unitOfWork.Commit();
            return Task.CompletedTask;
        }

        private async Task Block()
        {
            var casService = new CasService();
            _decision.SaveBlockId(await casService.Block(_decision.TotalAmount));
        }

        private Task ReleaseBlock()
        {
            var casService = new CasService();
            return casService.Release(_decision.BlockId);
        }

        private Task SendToBourse()
        {
            _decision.Close();
            _unitOfWork.OrderRepository.Save(_decision);
            _unitOfWork.Commit();
            return Task.CompletedTask;
        }
    }
}
