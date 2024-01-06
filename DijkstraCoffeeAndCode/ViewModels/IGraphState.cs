// WunderVision 2024
// https://www.wundervisionengineering.com/
namespace DijkstraCoffeeAndCode.ViewModels
{
    public interface IGraphState
    {
        public void StoreState(BaseGraphViewModel viewModel);
        public void RestoreState(BaseGraphViewModel viewModel);
    }
}
