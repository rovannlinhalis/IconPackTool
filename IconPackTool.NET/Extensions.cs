using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconPackTool
{
    public static class Extensions
    {
        public static Bitmap AdjustBrightness(this Image image, float brightness)
        {
            // Make the ColorMatrix.
            float b = brightness;
            ColorMatrix cm = new ColorMatrix(new float[][]
                {
            new float[] {b, 0, 0, 0, 0},
            new float[] {0, b, 0, 0, 0},
            new float[] {0, 0, b, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1},
                });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm);

            // Draw the image onto the new bitmap while applying
            // the new ColorMatrix.
            Point[] points =
            {
        new Point(0, 0),
        new Point(image.Width, 0),
        new Point(0, image.Height),
    };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }
        public static Bitmap AdjustImage(this Image image, float brightness, float contrast = 1.0f, float gamma = 1.0f)
        {
            Bitmap adjustedImage = new Bitmap(image);

            float adjustedBrightness = brightness - 1.0f;
            // create matrix that will brighten and contrast the image
            float[][] ptsArray ={
        new float[] {contrast, 0, 0, 0, 0}, // scale red
        new float[] {0, contrast, 0, 0, 0}, // scale green
        new float[] {0, 0, contrast, 0, 0}, // scale blue
        new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
        new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
            using (Graphics g = Graphics.FromImage(adjustedImage))
            {
                g.DrawImage(image, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
                      , 0, 0, image.Width, image.Height,
                      GraphicsUnit.Pixel, imageAttributes);
            }

            return adjustedImage;


    //        // Make the ColorMatrix.
    //        float b = brightness;
    //        ColorMatrix cm = new ColorMatrix(new float[][]
    //            {
    //        new float[] {b, 0, 0, 0, 0},
    //        new float[] {0, b, 0, 0, 0},
    //        new float[] {0, 0, b, 0, 0},
    //        new float[] {0, 0, 0, 1, 0},
    //        new float[] {0, 0, 0, 0, 1},
    //            });
    //        ImageAttributes attributes = new ImageAttributes();
    //        attributes.SetColorMatrix(cm);

    //        // Draw the image onto the new bitmap while applying
    //        // the new ColorMatrix.
    //        Point[] points =
    //        {
    //    new Point(0, 0),
    //    new Point(image.Width, 0),
    //    new Point(0, image.Height),
    //};
    //        Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

    //        // Make the result bitmap.
    //        Bitmap bm = new Bitmap(image.Width, image.Height);
    //        using (Graphics gr = Graphics.FromImage(bm))
    //        {
    //            gr.DrawImage(image, points, rect,
    //                GraphicsUnit.Pixel, attributes);
    //        }

    //        // Return the result.
    //        return bm;
        }
    }
}
