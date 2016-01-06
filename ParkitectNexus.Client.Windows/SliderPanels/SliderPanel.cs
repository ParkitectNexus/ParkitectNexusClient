using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Animation;
using MetroFramework.Controls;

namespace ParkitectNexus.Client.Windows.SliderPanels
{
    public partial class SliderPanel : MetroUserControl
    {
        private string _backText;
        private MoveAnimation _currentAnimation;
        private bool _isSlidedIn;
        private Control _parent;

        public SliderPanel()
        {
            InitializeComponent();
            BackColor = MetroColors.White;

            UpdateText();
        }

        #region Overrides of Panel

        /// <summary>
        ///     This member is not meaningful for this control.
        /// </summary>
        /// <returns>
        ///     The text associated with this control.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public virtual string BackText
        {
            get { return _backText; }
            set
            {
                _backText = value;
                UpdateText();
            }
        }

        #endregion

        public bool IsSlidedIn
        {
            get { return _isSlidedIn; }
            set
            {
                if (_isSlidedIn == value || Parent == null)
                    return;

                _isSlidedIn = value;

                if (_currentAnimation != null && _currentAnimation.IsRunning)
                    _currentAnimation.Cancel();

                _currentAnimation = new MoveAnimation();
                _currentAnimation.Start(this, new Point(value ? Parent.Width - Width : Parent.Width, Top),
                    TransitionType.EaseOutExpo, 22);
                _currentAnimation.AnimationCompleted += CurrentAnimationOnAnimationCompleted;
            }
        }

        private void metroLink_Click(object sender, EventArgs e)
        {
            IsSlidedIn = false;
        }

        private void UpdateText()
        {
            int width;
            using (var b = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(b))
            using (var f = MetroFonts.Link(MetroLinkSize.Tall, MetroLinkWeight.Light))
                width = (int) g.MeasureString(BackText, f).Width;

            metroLink.Width = 32 + 8 + width;
            metroLink.Text = BackText;
        }

        #region Overrides of Control

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnParentChanged(EventArgs e)
        {
            if (DesignMode)
                return;

            if (Parent != _parent)
            {
                if (_parent != null)
                    _parent.Resize -= _parent_Resize;
                if (Parent != null)
                {
                    Parent.Resize += _parent_Resize;
                    Height = Parent.Height;
                }
            }
            _parent = Parent;
            _isSlidedIn = false;

            FixSize();
            UpdateText();

            base.OnParentChanged(e);
        }

        #endregion

        private void _parent_Resize(object sender, EventArgs e)
        {
            FixSize();
        }

        private void FixSize()
        {
            if (Parent == null) return;
            Top = 30;
            Left = IsSlidedIn ? Parent.Width - Width : Parent.Width;
            Height = Parent.Height - Top - 20;
        }

        public event EventHandler SlidedIn;
        public event EventHandler SlidedOut;

        private void CurrentAnimationOnAnimationCompleted(object sender, EventArgs eventArgs)
        {
            if (_isSlidedIn)
                OnSlidedIn();
            else
                OnSlidedOut();
        }

        protected virtual void OnSlidedIn()
        {
            SlidedIn?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSlidedOut()
        {
            SlidedOut?.Invoke(this, EventArgs.Empty);
        }
    }
}
