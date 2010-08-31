using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace DrawingVisualizer
{
    [Serializable]
    public sealed class SerializableGraphics : IDisposable
    {
        [DllImport( "gdi32.dll" )]
        static extern bool BitBlt( IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop );

        readonly Bitmap bitmap;

        public SerializableGraphics( Graphics graphics )
        {
            if( graphics == null )
                throw new ArgumentNullException( "graphics" );

            FieldInfo fi = graphics.GetType().GetField( "backingImage", BindingFlags.NonPublic | BindingFlags.Instance );
            if( fi != null )
            {
                Bitmap bm = (Bitmap)fi.GetValue( graphics );

                if( bm != null )
                {
                    // graphics was derived from image : clone internal bitmap
                    bitmap = (Bitmap)bm.Clone();
                }
                else
                {
                    // graphics without backing image : bitblt to new bitmap
                    Size sz = graphics.VisibleClipBounds.Size.ToSize();
                    bitmap = new Bitmap( sz.Width, sz.Height, graphics );
                    drawToBitmap( bitmap, graphics );
                }
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                return bitmap;
            }
        }

        private static void drawToBitmap( Image bitmap, Graphics graphics )
        {
            using( Graphics g = Graphics.FromImage( bitmap ) )
            {
                IntPtr hdcDst = g.GetHdc();
                IntPtr hdcSrc = graphics.GetHdc();

                try
                {
                    if( !BitBlt( hdcDst, 0, 0, bitmap.Width, bitmap.Height, hdcSrc, 0, 0, CopyPixelOperation.SourceCopy ) )
                        throw new Exception( "BitBlt failed." );
                }
                finally
                {
                    g.ReleaseHdc( hdcDst );
                    graphics.ReleaseHdc( hdcSrc );
                }
            }
        }

        public void Dispose()
        {
            if( bitmap != null )
            {
                bitmap.Dispose();
            }
        }
    }
}
