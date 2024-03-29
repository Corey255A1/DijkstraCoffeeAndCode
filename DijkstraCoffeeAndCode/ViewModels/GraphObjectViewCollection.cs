﻿// WunderVision 2023
// https://www.wundervisionengineering.com/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class GraphObjectViewCollection<K, V> where K : notnull where V : GraphObjectViewModel
    {
        public delegate V MakeViewModelFactory(K key);
        private Dictionary<K, V> _objectToViewModel = new();
        private MakeViewModelFactory viewFactory;

        public IEnumerable<K> Keys { get => _objectToViewModel.Keys; }
        public IEnumerable<V> Values { get => _objectToViewModel.Values; }

        public event GraphObjectViewCollectionEvent? AddOrRemove;
        public GraphObjectViewCollection(ObservableCollection<K> collection, MakeViewModelFactory objectToViewModel)
        {
            collection.CollectionChanged += CollectionChangedHandler;
            viewFactory = objectToViewModel;

            foreach (var item in collection)
            {
                AddNewObjectViewModel(item);
            }
        }

        public void CollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems == null || e.NewItems.Count == 0) { return; }
                        if (!(e.NewItems[0] is K dijkstraObject)) { return; }
                        AddNewObjectViewModel(dijkstraObject);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems == null || e.OldItems.Count == 0) { return; }
                        if (!(e.OldItems[0] is K dijkstraObject)) { return; }
                        RemoveObjectViewModel(dijkstraObject);
                    }
                    break;
            }
        }

        public bool Contains(K item)
        {
            return _objectToViewModel.ContainsKey(item);
        }

        public V GetViewModel(K item)
        {
            if (Contains(item))
            {
                return _objectToViewModel[item];
            }

            throw new Exception("Object not found in collection.");
        }

        public void AddNewObjectViewModel(K item)
        {
            V objectToAdd = viewFactory.Invoke(item);
            _objectToViewModel.Add(item, objectToAdd);
            AddOrRemove?.Invoke(objectToAdd, true);
        }

        public void RemoveObjectViewModel(K node)
        {
            GraphObjectViewModel objectToRemove = GetViewModel(node);
            _objectToViewModel.Remove(node);
            AddOrRemove?.Invoke(objectToRemove, false);
        }
    }
}
