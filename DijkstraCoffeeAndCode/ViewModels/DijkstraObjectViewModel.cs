// WunderVision 2023
// https://www.wundervisionengineering.com/
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public delegate void DijkstraObjectViewCollectionEvent(DijkstraObjectViewModel dijkstraObject, bool isAdd);

    public class DijkstraObjectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                _isHighlighted = value;
                Notify();
            }
        }

        private bool _isHighlightedAlternate;
        public bool IsHighlightedAlternate
        {
            get { return _isHighlightedAlternate; }
            set
            {
                _isHighlightedAlternate = value;
                Notify();
            }
        }

        public virtual void Reset()
        {
            IsHighlighted = false;
            IsHighlightedAlternate = false;
        }
    }
}
