using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Processing.ImageReaders;
using Common;
using Processing;
using Processing.DataProcessors;

namespace UserInterface.WorkspacePanel
{
    public partial class WorkspacePanel : UserControl
    {
        public class Item
        {
            public object Data { get; set; }
            public WorkspacePanelItem View { get; set; }
            public WorkspacePanelItemControllerBase Controller { get; set; }
        }

        private List<Item> items = new List<Item>();
        private bool _showToolbar = true;

        public event Action<Item, MouseEventArgs> OnItemDoubleClick;
        public event Action<Item, MouseEventArgs> OnItemClick;
        public event Action<Item> OnItemAdded;

        public List<ContextMenuAction> ContextMenuActions { get; }= new List<ContextMenuAction>();

        public bool SelectItemOnClick { get; set; }
        public Item SelectedItem => items.SingleOrDefault(i => i.View.Selected);

        public bool ShowToolbar
        {
            get => _showToolbar;
            set
            {
                _showToolbar = value;
                if (_showToolbar)
                    toolStrip1.Show();
                else
                    toolStrip1.Hide();
            }
        }
        
        public WorkspacePanel()
        {
            InitializeComponent();
        }
        
        public void MarkItemSelected(WorkspacePanelItem item)
        {
            items.ForEach(i => i.View.SetSelectionState(false));
            item.SetSelectionState(true);
        }

        public void AddDataProcessor(IDataProcessor dataProcessor)
        {
            var item = new WorkspacePanelItem();
            MainLayoutPanel.Controls.Add(item);

            var itemController = new DataProcessorWorkspacePanelItemController(dataProcessor, item);
            itemController.UpdateView();

            var itemData = new Item
            {
                Data = dataProcessor,
                Controller = itemController,
                View = item
            };
            items.Add(itemData);

            item.Click += (sender, args) =>
            {
                if (args is MouseEventArgs me)
                {
                    if (SelectItemOnClick && me.Button == MouseButtons.Left)
                        MarkItemSelected(item);

                    OnItemClick?.Invoke(itemData, me);

                    if (me.Button == MouseButtons.Right)
                        ShowMenu(itemData, me.Location);
                }
            };

            item.DoubleClick += (o, args) =>
            {
                OnItemDoubleClick?.Invoke(itemData, args as MouseEventArgs);
            };

            OnItemAdded?.Invoke(itemData);
        }

        public void AddImageHandler(IImageHandler imageHandler)
        {
            var item = new WorkspacePanelItem();
            MainLayoutPanel.Controls.Add(item);

            var itemController = new ImageWorkspacePanelItemController(imageHandler, item);
            itemController.UpdateView();

            var itemData = new Item
            {
                Data = imageHandler,
                Controller = itemController,
                View = item
            };
            items.Add(itemData);
            
            item.Click += (sender, args) =>
            {
                if (args is MouseEventArgs me)
                {
                    if (SelectItemOnClick && me.Button == MouseButtons.Left)
                        MarkItemSelected(item);

                    OnItemClick?.Invoke(itemData, me);

                    if (me.Button == MouseButtons.Right)
                        ShowMenu(itemData, me.Location);
                }
            };

            item.DoubleClick += (o, args) =>
            {
                OnItemDoubleClick?.Invoke(itemData, args as MouseEventArgs);
            };

            OnItemAdded?.Invoke(itemData);
        }

        /// <summary>
        /// Удаляет изображение или обработчик с панели
        /// </summary>
        /// <param name="item">Изображение или обработчик</param>
        public void Remove(object item)
        {
            var itemData = items.SingleOrDefault(i => i.Data == item);
            if (itemData == null)
                return;

            MainLayoutPanel.Controls.Remove(itemData.View);
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            var imageReaders = ImageReaderProvider.GetAll();

            openFileDialog1.Filter = imageReaders.Select(r => r.GetFileFilter()).Join("|");
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            foreach (var fileName in openFileDialog1.FileNames)
            {
                OpenImage(fileName);
            }
        }

        public void OpenImage(string fileName)
        {
            var imageReaders = ImageReaderProvider.GetAll();
            IImageHandler imageHandler;
            using (var stream = File.OpenRead(fileName))
            {
                var reader = imageReaders.First(r => r.CanRead(stream));
                imageHandler = reader.Read(stream);
                stream.Close();
            }

            imageHandler.Tags[ImageHandlerTagKeys.Title] = new FileInfo(fileName).Name;
            imageHandler.Tags[ImageHandlerTagKeys.Source] = "filesystem";
            imageHandler.Tags[ImageHandlerTagKeys.FileSystemPath] = fileName;

            AddImageHandler(imageHandler);
        }

        public void ShowMenu(Item item, Point point)
        {
            var menuItems = ContextMenuActions
                .Where(a => a.DisplayCondition?.Invoke(item) ?? true)
                .Select(a =>
                {
                    var menuItem = new MenuItem(a.Text);
                    menuItem.Click += (sender, args) => a.Action(item);
                    return menuItem;
                })
                .ToArray();

            var contextMenu = new ContextMenu(menuItems);
            contextMenu.Show(item.View, point);
        }

        public class ContextMenuAction
        {
            public Bitmap Image { get; set; }
            public string Text { get; set; }
            public Func<Item, bool> DisplayCondition { get; set; }
            public Action<Item> Action { get; set; }
            public Shortcut Shortcut { get; set; }
        }
    }
}
