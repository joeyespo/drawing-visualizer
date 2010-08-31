using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace DrawingVisualizer
{
    internal class GraphicsVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData( object target, Stream outgoingData )
        {
            Graphics data = (Graphics)target;
            base.GetData( new SerializableGraphics( data ), outgoingData );
        }
    }
}
