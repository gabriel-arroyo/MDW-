using MDW_wf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MDW_wf.Connectivity;

namespace MDW_wf.Controller
{
    public static class TagManager
    {
        public static Start StartWindow;
        public static bool PublishSQL = false;
        public static bool PublishCSV = false;
        public static bool PublishWebService = false;
        public static bool PublishWebSocket = false;
        public static bool PublishXML = false;
        private static object myLock = new object();
        public static void Initialize(Start window)
        {
            StartWindow = window;
        }

        public static void Add(ListViewTagsData tag)
        {
            lock (myLock)
            {
                //if (tag.Timestamp.Contains("UTC"))
                //{
                //    string[] parts = tag.Timestamp.Split(' ');
                //    tag.Timestamp = string.Format("{0:o}", DateTime.Parse(parts[0] + " " + parts[1]).ToUniversalTime());
                //}
                //else
                //{
                //    tag.Timestamp = String.Format("{0:o}", DateTime.Parse(tag.Timestamp).ToUniversalTime());
                //}
                Thread.Sleep(100);
                TagsCollections.AddTag(tag);
                //StartWindow.UpdateUI_Tags();
                StartWindow.AddTagToListview(tag.Timestamp, tag.IP, tag.EPC, tag.RSSI, tag.Direction.ToString());

                #region Publish
                if (PublishSQL)
                {
                    SQL.Write(tag);
                }
                if (PublishCSV)
                {
                    //HTKLibrary.Comunications.Net35.DB.CSV csv = new HTKLibrary.Comunications.Net35.DB.CSV();
                    //csv.Write(tag, ConfigManager.CSVPath);
                }
                if (PublishXML)
                {
                    XML.Write(tag);
                }
                if(PublishWebService)
                {
                    WebService.Write(tag);
                }
                if (PublishWebSocket)
                {
                    WebSocket.Write(tag);
                }
                #endregion
            }
        }
        public static void Remove(ListViewTagsData tag)
        {
            Thread.Sleep(10);
            TagsCollections.RemoveTag(tag);
            StartWindow.RemoveTagFromList(tag.IP, tag.EPC);
        }
        public static void Remove(string ip, string epc)
        {
            Thread.Sleep(10);
            TagsCollections.RemoveTag(ip, epc);
            StartWindow.RemoveTagFromList(ip, epc);
        }

        internal static void RemoveAll(string ip)
        {
            Thread.Sleep(10);
            TagsCollections.RemoveAllTags(ip);
            StartWindow.RemoveTagFromList(ip);
        }
    }
}
