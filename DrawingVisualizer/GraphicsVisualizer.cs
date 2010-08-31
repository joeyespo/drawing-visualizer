using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace DrawingVisualizer
{
    public class GraphicsVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show( IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider )
        {
            using( SerializableGraphics wrapper = (SerializableGraphics)objectProvider.GetObject() )
            {
                using( ViewImageDialog dialog = new ViewImageDialog() )
                {
                    dialog.Image = wrapper.Bitmap;
                    windowService.ShowDialog( dialog );
                }
            }
        }
    }
}
