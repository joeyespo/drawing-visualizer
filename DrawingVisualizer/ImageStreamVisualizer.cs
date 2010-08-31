using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace DrawingVisualizer
{
    public class ImageStreamVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show( IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider )
        {
            Stream stream = null;
            Image image = null;

            try
            {
                stream = (Stream)objectProvider.GetObject();

                if( stream != null )
                    stream.Seek( 0, SeekOrigin.Begin );
                try
                {
                    image = Image.FromStream( stream );
                }
                catch
                {
                }
            }
            finally
            {
                // Do not release stream
                stream = null;
            }

            using( ViewImageDialog dialog = new ViewImageDialog() )
            {
                dialog.Image = image;
                windowService.ShowDialog( dialog );
            }
        }
    }
}
