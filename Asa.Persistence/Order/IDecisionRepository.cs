using Asa.Core.Domain;
using System.Threading.Tasks;

namespace Asa.TPC.Persistence
{
    public interface IDecisionRepository
    {
        Task<Decision> GetDecision(int decisionId);
        void Save(Decision decision);
    }
}