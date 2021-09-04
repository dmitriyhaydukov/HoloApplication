using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Infrastructure;
using OpenTK;
using Processing;
using Processing.Computing;
using UserInterface.DataEditors.Renderers.Graphics.Selection;
using UserInterface.DataEditors.Renderers.ImageRenderer;
using UserInterface.Properties;

namespace UserInterface.DataEditors.Tools
{
    public class SelectionTool : ToolBase
    {
        public enum SelectionEditAction
        {
            None,
            Move,
            ResizeNW,
            ResizeNE,
            ResizeSE,
            ResizeSW,
            ResizeN,
            ResizeE,
            ResizeS,
            ResizeW
        }

        public enum SelectionMode
        {
            Square512X512,
            Square1024X1024,
            Square2048X2048,
            Arbitrary2Nx2M,
        }

        private const string NewSelectionName = "<Новая область>";

        private ToolboxButtonInfo _buttonInfo;

        private SelectionEditAction _currentSelectionEditAction = SelectionEditAction.None;
        private ImageSelection _newSelection; // TODO: можно избавиться от этого
        private ImageSelection _currentSelection;
        private SelectionMode _currentSelectionMode = SelectionMode.Arbitrary2Nx2M;

        private ToolStripComboBox _selectionDropdown;
        private ToolStripComboBox _selectionModeDropdown;
        private ToolStripButton _saveSelectionButton;
        private ToolStripLabel _selectionInfoLabel;
        private ToolStripTextBox _xTextBox;
        private ToolStripTextBox _yTextBox;
        private ToolStripTextBox _widthTextBox;
        private ToolStripTextBox _heightTextBox;
        private Vector2 _prevCoord;

        private SelectionDrawable _drawable;
        private IReadOnlyCollection<ImageSelection> AvailableSelections => Singleton.Get<ImageSelectionManager>().GetAllSelections();
        public override ToolboxButtonInfo ButtonInfo => _buttonInfo;

        private ImageRenderer _renderer;

        public SelectionTool(ImageRenderer renderer)
        {
            _renderer = renderer;
            _buttonInfo = new ToolboxButtonInfo
            {
                Icon = Resources.select,
                Text = "Выделение"
            };

            if (!_renderer.ImageHandler.IsReady() || !Singleton.Get<ImageSelectionManager>().TryGetSelection(_renderer.ImageHandler, out _newSelection))
            {
                _newSelection = new ImageSelection
                {
                    Name = NewSelectionName
                };
                _newSelection.Width = 2816;
                _newSelection.Height = 2816;
            }

            _currentSelection = _newSelection;
            _drawable = new SelectionDrawable();
            _drawable.Selection = _currentSelection;
        }


        public void Save()
        {
        }

        public override void MouseEvent(MouseEventData eventData)
        {
            if (_currentSelectionMode != SelectionMode.Arbitrary2Nx2M)
            {
                if ((eventData.Event == MouseEventData.EventType.Down ||
                     eventData.Event == MouseEventData.EventType.Move)
                    && eventData.Args.Button == MouseButtons.Left)
                {
                    if (_currentSelection == _newSelection)
                    {
                        var imagePos = _renderer.GetImageCoordinate(new Vector2(eventData.Args.X, eventData.Args.Y));
                        _currentSelection.MoveTo(imagePos);
                        _drawable.SelectionUpdate();
                    }
                }
            }
            else
            {
                var nw = new Vector2(_currentSelection.X0, _currentSelection.Y0);
                var sw = new Vector2(_currentSelection.X0, _currentSelection.Y1);
                var se = new Vector2(_currentSelection.X1, _currentSelection.Y1);
                var ne = new Vector2(_currentSelection.X1, _currentSelection.Y0);

                var cur = new Vector2(eventData.Args.X, eventData.Args.Y);
                cur = _renderer.GetImageCoordinate(cur);

                var editRadius = 5f;
                var action = SelectionEditAction.None;

                if (cur.InRadius(se, editRadius))
                    action = SelectionEditAction.ResizeSE;
                else if (cur.InRadius(ne, editRadius))
                    action = SelectionEditAction.ResizeNE;
                else if (cur.InRadius(nw, editRadius))
                    action = SelectionEditAction.ResizeNW;
                else if (cur.InRadius(sw, editRadius))
                    action = SelectionEditAction.ResizeSW;
                else if (cur.DistanceToSegment(nw, ne).Abs() < editRadius)
                    action = SelectionEditAction.ResizeN;
                else if (cur.DistanceToSegment(ne, se).Abs() < editRadius)
                    action = SelectionEditAction.ResizeE;
                else if (cur.DistanceToSegment(se, sw).Abs() < editRadius)
                    action = SelectionEditAction.ResizeS;
                else if (cur.DistanceToSegment(nw, sw).Abs() < editRadius)
                    action = SelectionEditAction.ResizeW;
                else if (_currentSelection.GetRectangle().ContainsPoint((int) cur.X, (int) cur.Y))
                    action = SelectionEditAction.Move;

                if (action == SelectionEditAction.ResizeNW || action == SelectionEditAction.ResizeSE)
                    Cursor.Current = Cursors.SizeNWSE;
                else if (action == SelectionEditAction.ResizeNE || action == SelectionEditAction.ResizeSW)
                    Cursor.Current = Cursors.SizeNESW;
                else if (action == SelectionEditAction.ResizeN || action == SelectionEditAction.ResizeS)
                    Cursor.Current = Cursors.SizeNS;
                else if (action == SelectionEditAction.ResizeW || action == SelectionEditAction.ResizeE)
                    Cursor.Current = Cursors.SizeWE;
                else if (action == SelectionEditAction.Move)
                    Cursor.Current = Cursors.SizeAll;
                else
                    Cursor.Current = Cursors.Default;

                if (eventData.Event == MouseEventData.EventType.Down && eventData.Args.Button == MouseButtons.Left)
                {
                    _prevCoord = cur;
                    _currentSelectionEditAction = action;
                }

                if (eventData.Event == MouseEventData.EventType.Up && eventData.Args.Button == MouseButtons.Left)
                {
                    _currentSelectionEditAction = SelectionEditAction.None;
                }

                if (eventData.Event == MouseEventData.EventType.Move && eventData.Args.Button == MouseButtons.Left)
                {
                    switch (_currentSelectionEditAction)
                    {
                        case SelectionEditAction.ResizeNW:
                            _currentSelection.X0 = (int) cur.X;
                            _currentSelection.Y0 = (int) cur.Y;
                            break;
                        case SelectionEditAction.ResizeSW:
                            _currentSelection.X0 = (int) cur.X;
                            _currentSelection.Y1 = (int) cur.Y;
                            break;
                        case SelectionEditAction.ResizeSE:
                            _currentSelection.X1 = (int) cur.X;
                            _currentSelection.Y1 = (int) cur.Y;
                            break;
                        case SelectionEditAction.ResizeNE:
                            _currentSelection.X1 = (int) cur.X;
                            _currentSelection.Y0 = (int) cur.Y;
                            break;
                        case SelectionEditAction.ResizeN:
                            _currentSelection.Y0 = (int) cur.Y;
                            break;
                        case SelectionEditAction.ResizeS:
                            _currentSelection.Y1 = (int) cur.Y;
                            break;
                        case SelectionEditAction.ResizeW:
                            _currentSelection.X0 = (int) cur.X;
                            break;
                        case SelectionEditAction.ResizeE:
                            _currentSelection.X1 = (int) cur.X;
                            break;
                        case SelectionEditAction.Move:
                            _currentSelection.MoveBy(cur - _prevCoord);
                            _prevCoord = cur;
                            break;
                    }

                    _currentSelection.Width = _currentSelection.Width - _currentSelection.Width % 2;
                    _currentSelection.Height = _currentSelection.Height - _currentSelection.Height % 2;
                    _drawable.SelectionUpdate();
                }
            }

            UpdateInfoLabel();
        }

        public override void Activate()
        {
            Cursor.Current = Cursors.Cross;
            _renderer.AddDrawable(_drawable);
            _drawable.SelectionUpdate();
        }

        public override void Deactivate()
        {
            Cursor.Current = Cursors.Default;
            _renderer.RemoveDrawable(_drawable);
        }

        public override void PopulateToolstrip(ToolStrip toolStrip)
        {
            _selectionDropdown = new ToolStripComboBox("Selection");
            _selectionDropdown.Items.AddRange(new[] {_newSelection}.Concat(AvailableSelections).Select(s => (object) s).ToArray());
            _selectionDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            _selectionDropdown.SelectedItem = _currentSelection;
            _selectionDropdown.SelectedIndexChanged += (sender, args) =>
            {
                _currentSelection = (ImageSelection) _selectionDropdown.SelectedItem;
                _saveSelectionButton.Enabled = _currentSelection.Name == NewSelectionName;
                _drawable.Selection = _currentSelection;
                _drawable.SelectionUpdate();
            };

            _selectionModeDropdown = new ToolStripComboBox("Selection Mode");
            _selectionModeDropdown.Items.AddRange(EnumExtensions.GetValues<SelectionMode>().Select(v => (object) v).ToArray());
            _selectionModeDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            _selectionModeDropdown.SelectedItem = _currentSelectionMode;
            _selectionModeDropdown.SelectedIndexChanged += (sender, args) =>
            {
                _currentSelectionMode = (SelectionMode) _selectionModeDropdown.SelectedItem;

                int size = 2048;
                if (_currentSelectionMode == SelectionMode.Square512X512)
                    size = 512;
                else if (_currentSelectionMode == SelectionMode.Square1024X1024)
                    size = 1024;

                _currentSelection.Width = size;
                _currentSelection.Height = size;
                _drawable.SelectionUpdate();
            };

            _saveSelectionButton = new ToolStripButton("Сохранить")
            {
                Enabled = _currentSelection.Name == NewSelectionName
            };
            _saveSelectionButton.Click += (sender, args) =>
            {
                var imageName = _renderer.ImageHandler?.GetTitle() ?? "Область";
                _currentSelection.Name = $"{imageName} {AvailableSelections.Count(s => s.Name.StartsWith(imageName)) + 1}";
                Singleton.Get<ImageSelectionManager>().SetSelection(_renderer.ImageHandler, _currentSelection);
                _saveSelectionButton.Enabled = false;
                _selectionDropdown.SelectedItem = _currentSelection;
            };

            _selectionInfoLabel = new ToolStripLabel();

            toolStrip.Items.Add(_selectionDropdown);
            toolStrip.Items.Add(_selectionModeDropdown);
            toolStrip.Items.Add(_saveSelectionButton);
            toolStrip.Items.Add(_selectionInfoLabel);
        }

        private void UpdateInfoLabel()
        {
            var m1 = Fourier.OddDenominator(_currentSelection.Width);
            var m2 = Fourier.OddDenominator(_currentSelection.Height);
            var l1 = _currentSelection.Width / m1;
            var l2 = _currentSelection.Height / m2;

            _selectionInfoLabel.Text = $"{_currentSelection.CoordsToString()} [M {m1},L {l1}] x [M {m2}, L{l2}]";
        }
    }
}