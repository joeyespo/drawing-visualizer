using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace DrawingVisualizer
{
    public class ImageVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show( IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider )
        {
            Image image = (Image)objectProvider.GetObject();
            using( ViewImageDialog dialog = new ViewImageDialog() )
            {
                dialog.Image = image;
                windowService.ShowDialog( dialog );
            }
        }
    }
}
