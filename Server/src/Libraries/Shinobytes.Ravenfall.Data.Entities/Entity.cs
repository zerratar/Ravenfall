using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.Data.Entities
{
    public interface IEntity { }
    public class Entity<TModel> : IEntity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<TProp>(ref TProp item, TProp value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(item, value) || ReferenceEquals(item, value)) return false;
            item = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> forEach)
        {
            foreach (var item in items)
            {
                forEach(item);
            }
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePair<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, Func<TValue, TValue> valueSelector)
        {
            return values.Select(x => new KeyValuePair<TKey, TValue>(keySelector(x), valueSelector(x)));
        }
    }

    //public enum EntityState
    //{
    //    Unchanged,
    //    Added,
    //    Modified,
    //    Removed
    //}
}
