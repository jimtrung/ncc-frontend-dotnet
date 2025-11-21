using System.ComponentModel;

namespace Theater_Management_FE.Helpers
{
    public class SelectableItem<T> : INotifyPropertyChanged
    {
        private bool _isSelected;
        private T _item;

        public SelectableItem(T item, bool isSelected = false)
        {
            _item = item;
            _isSelected = isSelected;
        }

        public T Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public override string ToString()
        {
            return _item?.ToString() ?? string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
