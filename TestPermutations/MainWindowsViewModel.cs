using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;

namespace TestPermutations
{
    public class Item
    {
        public int Id { get; set; }
        public Item(int id)
        {
            Id = id;
        }
    }

    public class MainWindowsViewModel : ViewModelBase
    {
        private RelayCommand _findAllCmd;
        public RelayCommand FindAllCmd => _findAllCmd ?? (_findAllCmd = new RelayCommand(
            () => findAll(),
            () => { return 1 == 1; },
            keepTargetAlive: true
            ));

        private ObservableCollection<string> _retList = new ObservableCollection<string>();
        public ObservableCollection<string> RetList { get => _retList; set { Set(() => RetList, ref _retList, value); }}

        private ConcurrentBag<List<Item>> ItemBag = new ConcurrentBag<List<Item>>();

        private int _numItems; public int NumItems { get => _numItems; set { Set(() => NumItems, ref _numItems, value); }}
        private int _ms; public int Ms { get => _ms; set { Set(() => Ms, ref _ms, value); }}

        private void findAll()
        {
            List<Item> StartingItems = new List<Item>();
            for (int i = 1; i <= 10; i++)
            {
                StartingItems.Add(new Item(i));
            }
            ItemBag = new ConcurrentBag<List<Item>>();

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            permutate(new List<Item>(), StartingItems);
            sw.Stop();
            Ms = (int)sw.ElapsedMilliseconds;
            NumItems =        StartingItems.Count;

            RetList.Clear();
            foreach (var permutation in ItemBag)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(RetList.Count + 1).Append(':');

                for (int i = 0; i < StartingItems.Count - 1; i++)
                {
                    sb.Append(permutation[i].Id).Append(',');

                }
                sb.Append(permutation[permutation.Count - 1].Id);

                RetList.Add(sb.ToString());

            }

        }

        private void permutate(List<Item> listDone, List<Item> listToBeDone)
        {
            if (listToBeDone.Count == 0)
            {
                ItemBag.Add(listDone);
                return;
            }

            foreach (Item i in listToBeDone)
            {
                List<Item> newDone = new List<Item>();
                newDone.AddRange(listDone);
                newDone.Add(i);

                List<Item> newToBeDone = listToBeDone.Where(oi => oi != i).ToList();
                permutate(newDone, newToBeDone);
            }

        }

       
    }
}

