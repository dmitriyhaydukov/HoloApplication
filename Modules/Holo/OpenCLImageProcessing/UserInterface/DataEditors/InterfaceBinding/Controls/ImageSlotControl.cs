using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Processing;
using UserInterface.Utility;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public partial class ImageSlotControl : UserControl
    {
        private IImageHandler _imageHandler;

        public List<ImageFormat> AllowedImageFormats = new List<ImageFormat>();
        public List<ImagePixelFormat> AllowedPixelFormats = new List<ImagePixelFormat>();
        public int? RequiredChannelCount;
        public bool OnlyImages;

        public event Action OnValueChanged;

        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        public IImageHandler Value
        {
            get => _imageHandler;
            set => UpdateValue(value);
        }

        public ImageSlotControl()
        {
            InitializeComponent();

            UpdateValue(null, true);
        }

        private void UpdateValue(IImageHandler _newValue, bool forceUpdate = false)
        {
            if (_newValue == _imageHandler && !forceUpdate)
                return;

            _imageHandler = _newValue;

            if (_imageHandler == null)
            {
                IconPictureBox.BackgroundImage = Properties.Resources.icons8_add_new_16__1_;
                IconPictureBox.Image = null;
                InfoLabel.Text = AllowedImageFormats.Any()
                    ? AllowedImageFormats.Select(f => f.ToString()).ToCommaSeparated() + "\n"
                    : "";
                InfoLabel.Text += AllowedPixelFormats.Any()
                    ? AllowedPixelFormats.Select(f => f.ToString()).ToCommaSeparated() + "\n"
                    : "";
                InfoLabel.Text += RequiredChannelCount.HasValue ? "Каналов: " + RequiredChannelCount.Value : "";
            }
            else
            {

                InfoLabel.Text = $"{_imageHandler.GetTitle()}\n{_imageHandler.Width} x {_imageHandler.Height}";
                if (_imageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
                {
                    IconPictureBox.BackgroundImage = null;
                    IconPictureBox.Image = (Image) thumbnail;
                    IconPictureBox.Refresh();
                }
            }

            OnValueChanged?.Invoke();
        }
        
        private bool ImageHandlerPredicate(IImageHandler imageHandler)
        {
            if (!imageHandler.IsReady())
                return false;

            if (OnlyImages && imageHandler.GetType() != typeof(ImageHandler))

            if (AllowedImageFormats.Any() && !AllowedImageFormats.Contains(imageHandler.Format))
                return false;

            if (AllowedPixelFormats.Any() && !AllowedPixelFormats.Contains(imageHandler.PixelFormat))
                return false;

            if (RequiredChannelCount.HasValue && imageHandler.GetChannelsCount() != RequiredChannelCount.Value)
                return false;

            return true;
        }

        private void ShowImageHandlerSelector()
        {
            if (_imageHandler != null)
                return;

            if (Application.OpenForms.OfType<SelectImageHandlerPopupForm>().Any())
            {
                var forms = Application.OpenForms.OfType<SelectImageHandlerPopupForm>().ToArray();
                if (forms.Any(f => f.Visible))
                    return;

                foreach (var form in forms)
                {
                    form.Close();
                    form.Dispose();
                }
            }

            var selectImageHandlerForm = new SelectImageHandlerPopupForm(ImageHandlerPredicate);
            selectImageHandlerForm.Show();
            selectImageHandlerForm.Location = Cursor.Position;

            selectImageHandlerForm.OnImageHandlerSelected += handler => Value = handler;
        }

        private void miRemoveImage_Click(object sender, EventArgs e)
        {
            Value = null;
        }

        private void IconPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ShowImageHandlerSelector();
            else if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(PointToScreen(e.Location));
            else if (e.Button == MouseButtons.Middle)
                Value = null;
        }

        private void ImageSlotControl_Paint(object sender, PaintEventArgs e)
        {
            var pen = new Pen(Color.LightGray, 1.0f);
            pen.DashStyle = DashStyle.Dash;
            e.Graphics.DrawLine(pen, 0, 1, Width, 1);
        }
    }
}