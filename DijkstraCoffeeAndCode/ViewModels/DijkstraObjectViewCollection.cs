﻿using DijkstraAlgorithm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class DijkstraObjectViewCollection<K, V> where K : notnull where V : DijkstraObjectViewModel
    {
        private Dictionary<K, V> _objectToViewModel = new();
        private Func<K, V> viewFactory;

        public event DijkstraObjectViewCollectionEvent? AddOrRemove;
        public DijkstraObjectViewCollection(ObservableCollection<K> collection, Func<K, V> objectToViewModel)
        {
            collection.CollectionChanged += CollectionChangedHandler;
            viewFactory = objectToViewModel;
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

        public V GetViewModel(K node)
        {
            if (_objectToViewModel.ContainsKey(node))
            {
                return _objectToViewModel[node];
            }

            throw new Exception("Object not found in collection.");
        }

        public void AddNewObjectViewModel(K node)
        {
            V objectToAdd = viewFactory.Invoke(node);
            _objectToViewModel.Add(node, objectToAdd);
            AddOrRemove?.Invoke(objectToAdd, true);
        }

        public void RemoveObjectViewModel(K node)
        {
            DijkstraObjectViewModel objectToRemove = GetViewModel(node);
            _objectToViewModel.Remove(node);
            AddOrRemove?.Invoke(objectToRemove, false);
        }
    }
}