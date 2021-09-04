using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure;
using Processing;

namespace UserInterface.Utility
{
    public partial class SelectImageHandlerPopupForm : Form
    {
        public event Action<IImageHandler> OnImageHandlerSelected;
        public event Action OnCancel;

        public SelectImageHandlerPopupForm(Func<IImageHandler, bool> predicate)
        {
            InitializeComponent();

            var imageHandlers = Singleton.Get<ImageHandlerRepository>().Get(predicate);

            foreach (var imageHandler in imageHandlers)
            {
                workspacePanel1.AddImageHandler(imageHandler);
            }

            workspacePanel1.ShowToolbar = false;
            workspacePanel1.SelectItemOnClick = true;

            workspacePanel1.OnItemDoubleClick += (item, me) =>
            {
                OnImageHandlerSelected?.Invoke((IImageHandler) item.Data);
                Hide();
                Dispose(true);
            };

            btnCancel.Click += (sender, args) =>
            {
                OnCancel?.Invoke();
                Hide();
                Dispose(true);
            };

            btnSelect.Click += (sender, args) =>
            {
                if (workspacePanel1.SelectedItem != null && workspacePanel1.SelectedItem.Data is IImageHandler selectedImageHandler)
                {
                    OnImageHandlerSelected?.Invoke(selectedImageHandler);
                    Hide();
                    Dispose(true);
                }
            };
        }
    }
}
