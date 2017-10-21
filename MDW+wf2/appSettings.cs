using CSLibrary;
using CSLibrary.Constants;
using CSLibrary.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MDW_wf
{
    [Serializable]
    public class appSettings
    {
        #region private Fields
        private bool m_enableRssiFilter = false;

        private uint m_RssiFilterThreshold = 60;

        private uint m_reconnectTimeout = 30000;

        private uint m_link_profile = 2;

        private uint m_power = 300;

        public bool m_fixedChannel = false;

        private uint m_channel_number = 0;

        private bool m_lbt = false;

        public bool FreqAgile = false;

        private bool m_cfg_blocking_mode = false;

        private bool m_cfg_continuous_mode = true;

        private bool m_debug_log = false;

        private RegionCode region = RegionCode.UNKNOWN;

        private TagGroup m_tagGroup = new TagGroup();

        private String m_mac_address = String.Empty;
        private String m_SerialNum = String.Empty;


        private FixedQParms m_fixedQ = new FixedQParms();
        private DynamicQParms m_dynQ = new DynamicQParms();
        /*private DynamicQAdjustParms m_dynQA = new DynamicQAdjustParms();
        private DynamicQThresholdParms m_dynQT = new DynamicQThresholdParms();*/
        private SingulationAlgorithm m_singulation = SingulationAlgorithm.FIXEDQ;
        private SingulationAlgorithmParms m_singulationAlgorithm = new SingulationAlgorithmParms();

        private AntennaList m_antennaList = AntennaList.DEFAULT_ANTENNA_LIST;
        private AntennaSequenceMode m_sequence_mode = AntennaSequenceMode.NORMAL;
        private uint m_sequence_size = 0;
        private byte[] m_sequence_list = new byte[48];

        private uint m_MaskBank = 0;
        private uint m_MaskOffset = 0;
        private uint m_MaskBitLength = 0;
        private string m_Mask = "";

        #endregion

        #region Constructors

        public appSettings()
        {
            //inital here
            m_tagGroup.selected = Selected.ALL;
            m_tagGroup.session = Session.S0;
            m_tagGroup.target = SessionTarget.A;

            m_dynQ.thresholdMultiplier = 0;
            m_dynQ.maxQValue = 15;
            m_dynQ.minQValue = 0;
            m_dynQ.retryCount = 0;
            m_dynQ.startQValue = 7;
            m_dynQ.toggleTarget = 1;

            /*m_dynQA.maxQueryRepCount = 0;
            m_dynQA.maxQValue = 15;
            m_dynQA.minQValue = 0;
            m_dynQA.retryCount = 0;
            m_dynQA.startQValue = 7;
            m_dynQA.toggleTarget = 1;

            m_dynQT.thresholdMultiplier = 0;
            m_dynQT.maxQValue = 15;
            m_dynQT.minQValue = 0;
            m_dynQT.retryCount = 0;
            m_dynQT.startQValue = 7;
            m_dynQT.toggleTarget = 1;*/

            m_fixedQ.qValue = 7;
            m_fixedQ.repeatUntilNoTags = 0;
            m_fixedQ.retryCount = 0;
            m_fixedQ.toggleTarget = 1;

            m_singulation = SingulationAlgorithm.DYNAMICQ;

        }

        #endregion

        #region Public properties
        public bool EnableRssiFilter
        {
            get { return m_enableRssiFilter; }
            set { m_enableRssiFilter = value; }
        }
        public uint RssiFilterThreshold
        {
            get { return m_RssiFilterThreshold; }
            set { m_RssiFilterThreshold = value; }
        }
        public uint ReconnectTimeout
        {
            get { return m_reconnectTimeout; }
            set { m_reconnectTimeout = value; }
        }
        public String MAC_ADDRESS
        {
            get { return m_mac_address; }
            set { m_mac_address = value; }
        }
        public String SerialNum
        {
            get { return m_SerialNum; }
            set { m_SerialNum = value; }
        }
        public bool Debug_log
        {
            get { return m_debug_log; }
            set { m_debug_log = value; }
        }

        public bool Cfg_blocking_mode
        {
            get { return m_cfg_blocking_mode; }
            set { m_cfg_blocking_mode = value; }
        }
        public bool Cfg_continuous_mode
        {
            get { return m_cfg_continuous_mode; }
            set { m_cfg_continuous_mode = value; }
        }
        public RegionCode Region
        {
            get { return region; }
            set { region = value; }
        }
        public uint Link_profile
        {
            get { return m_link_profile; }
            set { m_link_profile = value; }
        }
        public uint Power
        {
            get { return m_power; }
            set { m_power = value; }
        }
        public bool FixedChannel
        {
            get { return m_fixedChannel; }
            set { m_fixedChannel = value; }
        }
        public uint Channel_number
        {
            get { return m_channel_number; }
            set { m_channel_number = value; }
        }
        public bool Lbt
        {
            get { return m_lbt; }
            set { m_lbt = value; }
        }
        public SingulationAlgorithm Singulation
        {
            get { return m_singulation; }
            set { m_singulation = value; }
        }
        public SingulationAlgorithmParms SingulationAlg
        {
            get
            {
                switch (m_singulation)
                {
                    case SingulationAlgorithm.DYNAMICQ:
                        return m_dynQ;
                    /*case SingulationAlgorithm.DYNAMICQ_ADJUST:
                        return m_dynQA;
                    case SingulationAlgorithm.DYNAMICQ_THRESH:
                        return m_dynQT;*/
                    case SingulationAlgorithm.FIXEDQ:
                        return m_fixedQ;
                    default:
                        return new SingulationAlgorithmParms();
                }
            }
            set
            {
                /*if (value.GetType() == typeof(DynamicQAdjustParms))
                {
                    m_dynQA = (DynamicQAdjustParms)value;
                }
                else */
                if (value.GetType() == typeof(DynamicQParms))
                {
                    m_dynQ = (DynamicQParms)value;
                }
                /*else if (value.GetType() == typeof(DynamicQThresholdParms))
                {
                    m_dynQT = (DynamicQThresholdParms)value;
                }*/
                else if (value.GetType() == typeof(FixedQParms))
                {
                    m_fixedQ = (FixedQParms)value;
                }
                m_singulationAlgorithm = value;
            }
        }
        public FixedQParms FixedQ
        {
            get { return m_fixedQ; }
            set { m_fixedQ = value; }
        }
        public DynamicQParms DynQ
        {
            get { return m_dynQ; }
            set { m_dynQ = value; }
        }
        /*public DynamicQAdjustParms DynQA
        {
            get { return m_dynQA; }
            set { m_dynQA = value; }
        }
        public DynamicQThresholdParms DynQT
        {
            get { return m_dynQT; }
            set { m_dynQT = value; }
        } */
        public TagGroup tagGroup
        {
            get { return m_tagGroup; }
            set { m_tagGroup = value; }
        }

        [XmlArray("AntennaList")]
        public AntennaList AntennaList
        {
            get { return m_antennaList; }
            set { m_antennaList = value; }
        }

        /// <summary>
        /// Sequence Mode: 
        /// NORMAL | SEQUENCE | SMART_CHECK | SEQUENCE_SMART_CHECK
        /// </summary>
        //[XmlIgnore]
        public AntennaSequenceMode AntennaSequenceMode
        {
            get { return m_sequence_mode; }
            set { m_sequence_mode = value; }
        }

        /// <summary>
        /// Sequence Size for SEQUENCE mode or SEQUENCE_SMART_CHECK mode
        /// Size should be within 1-48
        /// </summary>
        //[XmlIgnore]
        public uint AntennaSequenceSize
        {
            get { return m_sequence_size; }
            set { m_sequence_size = value; }
        }

        //[XmlIgnore]
        [XmlArray("AntennaPortSequence")]
        //public AntennaPortCollections antennaPortSequence
        public byte[] AntennaPortSequence
        {
            get { return m_sequence_list; }
            set { m_sequence_list = value; }
        }

        public uint MaskBank
        {
            get { return m_MaskBank; }
            set { m_MaskBank = value; }
        }

        public uint MaskOffset
        {
            get { return m_MaskOffset; }
            set { m_MaskOffset = value; }
        }

        public uint MaskBitLength
        {
            get { return m_MaskBitLength; }
            set { m_MaskBitLength = value; }
        }

        public string Mask
        {
            get { return m_Mask; }
            set { m_Mask = value; }
        }

        #endregion

        #region Methods: Save, Load

        /// <summary>
        /// Saves this settings object to desired location
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            // Insert code to set properties and fields of the object.
            XmlSerializer mySerializer = new XmlSerializer(typeof(appSettings));
            // To write to a file, create a StreamWriter object.
            StreamWriter myWriter = new StreamWriter(fileName);
            mySerializer.Serialize(myWriter, this);
            myWriter.Close();
        }
        /// <summary>
        /// Saves this settings object to desired location
        /// </summary>
        public bool Save()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CSLReader";

                try
                {
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                }
                catch (Exception )
                {
                }

                string mac = path + "\\" + Program.configManager.MacAddress.Replace(':', '.') + ".cfg";
                // Insert code to set properties and fields of the object.
                XmlSerializer mySerializer = new XmlSerializer(typeof(appSettings));
                // To write to a file, create a StreamWriter object.
                StreamWriter myWriter = new StreamWriter(mac);
                mySerializer.Serialize(myWriter, this);
                myWriter.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Returns a clsSettings object, loaded from a specific location
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public appSettings Load(string fileName)
        {
            // Constructs an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer = new XmlSerializer(typeof(appSettings));
            // To read the file, creates a FileStream.
            FileStream myFileStream = new FileStream(fileName, FileMode.Open);
            // Calls the Deserialize method and casts to the object type.
            appSettings pos = (appSettings)mySerializer.Deserialize(myFileStream);
            myFileStream.Close();
            return pos;
        }

        /// <summary>
        /// Returns a clsSettings object, loaded from a specific location
        /// </summary>
        public appSettings Load()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CSLReader";

            try
            {
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
            }

            string mac = path + "\\" + Program.configManager.MacAddress.Replace(':', '.') + ".cfg";
            // Constructs an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer = new XmlSerializer(typeof(appSettings));
            // To read the file, creates a FileStream.
            FileStream myFileStream = new FileStream(mac, FileMode.Open);
            // Calls the Deserialize method and casts to the object type.
            appSettings pos = (appSettings)mySerializer.Deserialize(myFileStream);
            myFileStream.Close();
            return pos;
        }
        #endregion
    }
}
