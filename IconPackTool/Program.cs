using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace IconPackTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly);

            List<int> sizes = new List<int>() { 16, 24, 32, 48, 64, 96, 128, 160 };

            Parallel.ForEach(files, file =>
            {

                if (file.Contains("(") && file.Contains(")."))
                {
                    File.Delete(file);
                }
                else
                {
                    Console.WriteLine(file);

                    FileInfo fi = new FileInfo(file);
                    Image img = Image.FromFile(file);

                    Image state2 = img.AdjustImage(0.7f, 1.2f, 0.8f);
                    Image state3 = img.AdjustImage(1.0f, 1.0f, 0.4f);
                    Image state4 = img.AdjustImage(1.2f, 0.9f, 1.3f);
                    Image state5 = img.AdjustImage(1.2f, 1.0f, 1.1f);

                    //Parallel.ForEach(sizes, s =>
                    //{

                        foreach (int s in sizes)
                        {
                            string folder = Path.Combine(dir, "iconpack", "simples", s.ToString());
                            string folderStates = Path.Combine(dir, "iconpack", "states", s.ToString());
                            if (!Directory.Exists(folder))
                                Directory.CreateDirectory(folder);

                            if (!Directory.Exists(folderStates))
                                Directory.CreateDirectory(folderStates);

                            Image img1 = RedimensionarImagem(img, s, s, PixelFormat.Format32bppArgb);
                            Image img2 = RedimensionarImagem(state2, s, s, PixelFormat.Format32bppArgb);
                            Image img3 = RedimensionarImagem(state3, s, s, PixelFormat.Format32bppArgb);
                            Image img4 = RedimensionarImagem(state4, s, s, PixelFormat.Format32bppArgb);
                            Image img5 = RedimensionarImagem(state5, s, s, PixelFormat.Format32bppArgb);

                            Bitmap bmpStates = new Bitmap(img1.Width * 5, img1.Height, PixelFormat.Format32bppArgb);

                            using (Graphics g = Graphics.FromImage(bmpStates))
                            {
                                int x = 0;

                                g.DrawImage(img1, new Point(x, 0));
                                x += img1.Width;

                                g.DrawImage(img2, new Point(x, 0));
                                x += img1.Width;

                                g.DrawImage(img3, new Point(x, 0));
                                x += img1.Width;

                                g.DrawImage(img4, new Point(x, 0));
                                x += img1.Width;

                                g.DrawImage(img5, new Point(x, 0));
                                x += img1.Width;

                            }

                            string newPath = Path.Combine(folder, fi.Name.Replace(fi.Extension, "") + "_" + s.ToString() + fi.Extension);
                            string newPathState = Path.Combine(folderStates, fi.Name.Replace(fi.Extension, "") + "_" + s.ToString() + fi.Extension);
                            img1.Save(newPath);
                            bmpStates.Save(newPathState);
                        }
                    //});
                }
            });
        }


        public static System.Drawing.Image RedimensionarImagem(int newSize, char orientacao, System.Drawing.Image srcImage)
        {
            if (srcImage != null)
            {
                if (orientacao == 'w' || orientacao == 'W')
                {
                    int newH = newSize * srcImage.Height / srcImage.Width;
                    return RedimensionarImagem(srcImage, newSize, newH);
                }
                else
                {
                    int newW = newSize * srcImage.Width / srcImage.Height;
                    return RedimensionarImagem(srcImage, newW, newSize);
                }
            }
            else return null;

        }

        public static System.Drawing.Image RedimensionarImagem(System.Drawing.Image srcImage, int newWidth, int newHeight)
        {
            return RedimensionarImagem(srcImage, newWidth, newHeight, PixelFormat.Format24bppRgb);
        }
        /// <summary>
        /// Redimensiona um Objeto System.Drawing.Image
        /// </summary>
        /// <param name="srcImage">System.Drawing.Image</param>
        /// <param name="newWidth">int Largura (pixels)</param>
        /// <param name="newHeight">int Altura  (pixels)</param>
        /// <returns>System.Drawing.Image</returns>
        public static System.Drawing.Image RedimensionarImagem(System.Drawing.Image srcImage, int newWidth, int newHeight, PixelFormat pf)
        {
            if (srcImage != null)
            {
                Bitmap newImage = new Bitmap(newWidth, newHeight, pf);
                newImage.MakeTransparent();
                using (Graphics gr = Graphics.FromImage((Image)newImage))
                {
                    //gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(srcImage, 0, 0, newWidth, newHeight);// new Rectangle(0, 0, newWidth, newHeight));
                    gr.Dispose();
                    return (Image)newImage;
                }
            }
            else
                return null;

        }

        /// <summary>
        /// Redimensiona uma imagem
        /// </summary>
        public static System.Drawing.Image RedimensionarImagem(System.Drawing.Image img, decimal percentual)
        {
            if (img != null)
            {
                int nW = (int)(img.Width * percentual / 100);
                int nH = (int)(img.Height * percentual / 100);
                return RedimensionarImagem(img, nW, nH);
            }
            else return null;

        }

        /// <summary>
        /// Redimensiona uma imagem
        /// </summary>
        public static System.Drawing.Image RedimensionarImagem(System.Drawing.Image img, int limite, char c)
        {
            if (c == 'h' || c == 'H')
            {
                if (img.Height > limite)
                {
                    int delta = img.Height - limite;
                    decimal percent = 100 - (delta * 100 / img.Height);
                    percent = percent < 0 ? percent + 100 : percent;

                    return RedimensionarImagem(img, percent);
                }
                else
                    return img;

            }
            else
            {
                if (img.Width > limite)
                {
                    int delta = img.Width - limite;
                    decimal percent = delta * 100 / img.Width;
                    percent = percent < 0 ? percent + 100 : percent;
                    return RedimensionarImagem(img, percent);
                }
                return img;
            }


        }

       
    }
}
