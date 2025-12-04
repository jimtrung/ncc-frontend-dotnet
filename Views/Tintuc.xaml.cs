using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Controllers;

namespace Theater_Management_FE.Views
{
    public partial class Tintuc : Window
    {
        private TinTucController _controller;
        private WrapPanel newsWrapPanel;
        private Button btnPrev;
        private Button btnNext;
        private Button btnBackHome;

        public Tintuc()
        {
            InitializeComponent();
        }

        public void Initialize(TinTucController controller)
        {
            _controller = controller;

            this.Loaded += (s, e) =>
            {
                _controller.BindUIControls(
                    newsWrapPanel,
                    btnPrev,
                    btnNext,
                    btnBackHome
                );
            };
        }
    }
}
