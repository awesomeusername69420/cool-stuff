/*
 * Mass image downscaler
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using Ookii.Dialogs.WinForms;

namespace DownAll
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private int Round(double x) // CS0121
		{
			return (int)Math.Round(x);
		}

		private int Clamp(int x, int min = 0, int max = 2147483647)
		{
			if (x < min) return min;
			if (x > max) return max;

			return x;
		}

		private Bitmap ResizeImage(Image image, int width, int height) // https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp (I'm lazy, okay?)
		{
			Rectangle destRect = new Rectangle(0, 0, width, height);
			Bitmap destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (Graphics graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (ImageAttributes wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
		
		private void SourceBrowse_Click(object sender, EventArgs e)
		{
			using (VistaFolderBrowserDialog VFBD = new VistaFolderBrowserDialog())
			{
				if (VFBD.ShowDialog() == DialogResult.OK)
				{
					if (string.IsNullOrWhiteSpace(VFBD.SelectedPath)) return;

					SourceText.Text = VFBD.SelectedPath;
				}
			}
		}

		private void OutputBrowse_Click(object sender, EventArgs e)
		{
			using (VistaFolderBrowserDialog VFBD = new VistaFolderBrowserDialog())
			{
				if (VFBD.ShowDialog() == DialogResult.OK)
				{
					if (string.IsNullOrWhiteSpace(VFBD.SelectedPath)) return;

					OutputText.Text = VFBD.SelectedPath;
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string SourceDir = SourceText.Text;
			
			if (!Directory.Exists(SourceDir))
			{
				MessageBox.Show("Invalid source path.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string OutputDir = OutputText.Text;

			if (!Directory.Exists(OutputDir))
			{
				try
				{
					Directory.CreateDirectory(OutputDir);
				} catch (Exception)
				{
					MessageBox.Show("Invalid output path.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			string[] SourceFiles = Directory.GetFiles(SourceDir);

			int StartWidth = (Int32)StartingWidth.Value;
			int StartHeight = (Int32)StartingHeight.Value;

			int SubWidth = 0;
			int SubHeight = 0;

			if (DecreaseWidthAuto.Checked)
				SubWidth = Round((double)StartWidth / SourceFiles.Length);
			else
				SubWidth = (Int32)DecreaseWidth.Value;

			if (DecreaseHeightAuto.Checked)
				SubHeight = Round((double)StartHeight / SourceFiles.Length);
			else
				SubHeight = (Int32)DecreaseWidth.Value;

			int Index = 0;

			foreach (string FilePath in SourceFiles)
			{
				string FileName = Path.GetFileName(FilePath);

				int NewWidth = Clamp(StartWidth - (SubWidth * Index));
				int NewHeight = Clamp(StartHeight - (SubHeight * Index));

				Bitmap SourceImg = new Bitmap(FilePath);
				Bitmap OutputImg = ResizeImage(SourceImg, NewWidth, NewHeight);

				OutputImg.Save(OutputDir + "\\" + FileName);

				Index = Index + 1;
			}

			MessageBox.Show("Finished doing stuff.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
