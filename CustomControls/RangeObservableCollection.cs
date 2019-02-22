using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CustomControls
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        public RangeObservableCollection()
        {

        }

        private void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                base.Add(item);
            }
            OnPropertyChanged(new PropertyChangedEventArgs("Items"));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

    }
}
