using Asa.Core.Domain;
using Asa.TPC.ExternalService.Common;

namespace Asa.TPC.ExternalService.Block
{
    public class CasBlockResource : TransactionalResource
    {
        private readonly Decision _decision;
        private readonly CasService _casService;
        private int _blockId;

        public CasBlockResource(Decision decision)
        {
            _decision = decision;
            _casService = new CasService();
        }

        protected override void Prepare()
        {
            _blockId = _casService.Block(_decision.TotalAmount).Result;
            _decision.SaveBlockId(_blockId);
        }

        protected override void Commit() {
        }

        protected override void Rollback()
        {
            _casService.Release(_blockId).Wait();
        }
    }
}
