using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Tintelo.iOS.Utils;

public class ObservableRangeCollection<T> : ObservableCollection<T>
{
	// Event args cache
	// ReSharper disable StaticMemberInGenericType
	static readonly PropertyChangedEventArgs CountPropertyChanged = new(nameof(Count));
	static readonly PropertyChangedEventArgs IndexerPropertyChanged = new("Item[]");


	// Constructors
	public ObservableRangeCollection()
	{
	}

	public ObservableRangeCollection(
		IEnumerable<T> collection)
		: base(collection ?? throw new ArgumentNullException(nameof(collection)))
	{ }

	public ObservableRangeCollection(
		List<T> list)
		: base(list ?? throw new ArgumentNullException(nameof(list)))
	{ }


	// Range methods
	public void AddRange(
		IEnumerable<T> items)
	{
		ArgumentNullException.ThrowIfNull(items);
		CheckReentrancy();

		List<T> addedItems = items.ToList();
		if (addedItems.Count == 0)
			return;

		int startingIndex = Count;
		
		foreach (T item in addedItems)
			Items.Add(item);

		OnPropertyChanged(CountPropertyChanged);
		OnPropertyChanged(IndexerPropertyChanged);
		OnCollectionChanged(new(NotifyCollectionChangedAction.Add, addedItems, startingIndex));
	}

	public void InsertRange(
		int index,
		IEnumerable<T> items)
	{
		ArgumentNullException.ThrowIfNull(items);
		ArgumentOutOfRangeException.ThrowIfNegative(index);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(index, Count);
		CheckReentrancy();

		List<T> insertedItems = items.ToList();
		if (insertedItems.Count == 0)
			return;

		int startingIndex = index;

		foreach (T item in insertedItems)
			Items.Insert(index++, item);

		OnPropertyChanged(CountPropertyChanged);
		OnPropertyChanged(IndexerPropertyChanged);
		OnCollectionChanged(new(NotifyCollectionChangedAction.Add, insertedItems, startingIndex));
	}

	public void RemoveRange(
		int index,
		int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index);
		ArgumentOutOfRangeException.ThrowIfNegative(count);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(index + count, Count);
		CheckReentrancy();

		if (count == 0)
			return;

		List<T> removedItems = new(count);

		for (int offset = 0; offset < count; offset++)
		{
			removedItems.Add(Items[index]);
			Items.RemoveAt(index);
		}

		OnPropertyChanged(CountPropertyChanged);
		OnPropertyChanged(IndexerPropertyChanged);
		OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, removedItems, index));
	}
}