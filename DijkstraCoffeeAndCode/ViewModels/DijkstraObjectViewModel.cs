using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public virtual void Reset() { 
            IsHighlighted = false;
            IsHighlightedAlternate = false;
        }
    }
}
