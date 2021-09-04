using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK;

namespace UserInterface.DataEditors.Renderers
{
    public interface IViewParametres
    {
        Matrix4 ViewMatrix { get; }
        Matrix4 ProjectionMatrix { get; }
    }
    public class ViewParametres: IViewParametres
    {
        private bool _matricesShouldBeUpdated;
        private PointF _displacement;
        private SizeF _viewportSize;
        private float _zoom = 1f;
        private Matrix4 _viewMatrix;
        private Matrix4 _projectionMatrix;

        public float Zoom { get; set; } = 1f;
        public float ViewportRatio => _viewportSize.Width / _viewportSize.Height;

        public PointF Displacement
        {
            get => _displacement;
            set
            {
                _displacement = value;
                _matricesShouldBeUpdated = true;
            }
        }

        public SizeF ViewportSize
        {
            get => _viewportSize;
            set
            {
                _viewportSize = value;
                _matricesShouldBeUpdated = true;
            }
        }

        public Matrix4 ViewMatrix
        {
            get
            {
                if (_matricesShouldBeUpdated)
                    UpdateMatrices();

                return _viewMatrix;
            }
        }

        public Matrix4 ProjectionMatrix
        {
            get
            {
                if (_matricesShouldBeUpdated)
                    UpdateMatrices();

                return _projectionMatrix;
            }
        }

        private void UpdateMatrices()
        {
            //Matrix4.CreateTranslation(Displacement.X / ViewportSize.Height, Displacement.Y / ViewportSize.Height, -1f);
            _viewMatrix = Matrix4.CreateTranslation(_displacement.X / _viewportSize.Width, _displacement.Y / _viewportSize.Height, -1f);
            _projectionMatrix = Matrix4.CreateOrthographic(ViewportRatio, 1f, 1f, 10f);
            _matricesShouldBeUpdated = false;
        }

        public void Move(PointF displacement)
        {
            Displacement = new PointF(Displacement.X + displacement.X, Displacement.Y + displacement.Y);
        }

        public void ResetDisplacement()
        {
            Displacement = PointF.Empty;
        }

        public PointF PointToView(PointF point)
        {
            return point.ResolveToView(Zoom, Displacement.X, Displacement.Y);
        }

        public PointF PointToImage(PointF point)
        {
            return point.ResolveToImage(Zoom, Displacement.X, Displacement.Y);
        }
    }
}
