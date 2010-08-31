using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DrawingVisualizer
{
    public partial class ViewImageDialog : Form
    {
        Image image = null;

        public ViewImageDialog()
        {
            InitializeComponent();
            this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
            this.SetStyle( ControlStyles.ResizeRedraw, true );
        }

        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                Text = "View Image" + ( ( image != null ) ? ( " (" + image.Width + "x" + image.Height + ")" ) : ( "" ) );
                Invalidate();
            }
        }

        protected override void OnKeyDown( KeyEventArgs e )
        {
            base.OnKeyDown( e );
            if( e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter )
                Close();
        }

        protected override void OnMouseDoubleClick( MouseEventArgs e )
        {
            base.OnMouseDoubleClick( e );
            Close();
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            int cx = ClientSize.Width;
            int cy = ClientSize.Height;
            Graphics g = e.Graphics;

            // Draw background
            using( Brush brush = new HatchBrush( HatchStyle.DiagonalCross, Color.LightGray, Color.White ) )
                g.FillRectangle( brush, 0, 0, cx, cy );

            if( image != null && image.Width != 0 && image.Height != 0 && cx != 0 && cy != 0 )
            {
                // Calculate coordinates
                Rectangle dest;
                if( image.Width > cx || image.Height > cy )
                {
                    if( image.Width > cx )
                    {
                        double factor = cx / (double)image.Width;
                        dest = new Rectangle( 0, (int)( ( cy - image.Height * factor ) / 2 ), (int)( image.Width * factor ), (int)( image.Height * factor ) );
                    }
                    else
                    {
                        double factor = cy / (double)image.Height;
                        dest = new Rectangle( (int)( ( cx - image.Width * factor ) / 2 ), 0, (int)( image.Width * factor ), (int)( image.Height * factor ) );
                    }
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                }
                else
                    dest = new Rectangle( ( cx - image.Width ) / 2, ( cy - image.Height ) / 2, image.Width, image.Height );

                // Draw transparency background
                using( Brush brush = new HatchBrush( HatchStyle.LargeCheckerBoard, Color.LightGray, Color.White ) )
                    g.FillRectangle( brush, dest );

                // Draw image
                g.DrawImage( image, dest, new Rectangle( 0, 0, image.Width, image.Height ), GraphicsUnit.Pixel );
            }
        }
    }
}
