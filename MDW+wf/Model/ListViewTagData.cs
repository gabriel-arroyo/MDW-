using System;
using System.Collections.Generic;
using System.Linq;

namespace MDW_wf.Model
{
    public static class TagsCollections
    {
        public static Dictionary<string, ListViewTagsData> ListViewTagsCollections = new Dictionary<string, ListViewTagsData>();
        public static List<ListViewTagsData> TagsCollectionsList
        {
            get
            {
                List<ListViewTagsData> tags = (from keyValuePair in ListViewTagsCollections
                                               select keyValuePair.Value).ToList();
                return tags;
            }
        }
        public static void AddTag(ListViewTagsData tag)
        {
            if (tag == null) return;
            try
            {
                string key = string.Format("{0}{1}", tag.IP, tag.EPC);
                if (!ListViewTagsCollections.ContainsKey(key) && !string.IsNullOrEmpty(key))
                    ListViewTagsCollections.Add(key, tag);
            }
            catch { }
        }
        public static void RemoveTag(ListViewTagsData tag)
        {
            ListViewTagsData data;
            ListViewTagsCollections.TryGetValue(String.Format("{0}{1}", tag.IP, tag.EPC), out data);
            ListViewTagsCollections.Remove(String.Format("{0}{1}", tag.IP, tag.EPC));
            data = null;
            ListViewTagsCollections.TryGetValue(String.Format("{0}{1}", tag.IP, tag.EPC), out data);
        }
        public static void RemoveTag(string ip, string epc)
        {
            ListViewTagsData tag = new ListViewTagsData();
            ListViewTagsCollections.Remove(String.Format("{0}{1}", tag.IP, tag.EPC));
        }

        internal static void RemoveAllTags(string ip)
        {
            var copy = new Dictionary<string, ListViewTagsData>(ListViewTagsCollections);
            foreach(var tag in copy)
            {
                if(tag.Key.Contains(ip))
                {
                    ListViewTagsCollections.Remove(tag.Key);
                }
            }
        }
    }
   
    public class ListViewTagKey : IEqualityComparer<ListViewTagKey>
    {
        public string EPC { get; set; }
        public string IP { get; set; }

        public string ID { get { return String.Format("{0}{1}",IP,EPC); } }

        public bool Equals(ListViewTagKey x, ListViewTagKey y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(ListViewTagKey obj)
        {
            return obj.ID.GetHashCode();
        }
    }
    public class ListViewTagsData
    {
        public string Timestamp { get; set; }
        public string IP { get; set; }
        public string EPC { get; set; }
        public string RSSI { get; set; }
        public string Direction { get; set; }

        //public ListViewTagKey GetListViewkey()
        //{
        //    return new ListViewTagKey { EPC = EPC, IP = IP };
        //}
    }
    
}
