using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HTKLibrary.Classes.MDW;

namespace MDW_wf.Connectivity
{
        public delegate void PublishEventHandhler(object sender, Tag tag);

    public class Publish
    {
        public event PublishEventHandhler ToPublish;
        protected virtual void OnPublish(Tag tag)
        {
            if (ToPublish != null)
                ToPublish(this, tag);
        }

        public void Tag(Tag tag)
        {
            OnPublish(tag);
        }
    }
}
