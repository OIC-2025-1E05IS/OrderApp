using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrderApp
{
    public partial class Kaikei : Window
    {
        private int _goukei = 0;
        private int _azukari = 0;
        private List<(string Name, int Price)> _orderItems = new();

        public Kaikei(int goukei, List<(string Name, int Price)> orderItems)
        {
            InitializeComponent();
            _goukei = goukei;
            _orderItems = orderItems;
            txtGoukei.Text = $"¥{_goukei:N0}";
            txtAzukariInput.IsEnabled = false;
            UpdateDisplay();
        }

        private void txtAzukariInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(txtAzukariInput.Text, out int val))
                _azukari = val;
            else
                _azukari = 0;

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            int diff = _azukari - _goukei;

            if (_azukari <= 0)
            {
                txtSashihiki.Text = "¥0";
                lblSashihiki.Text = "不足";
                txtSashihiki.Foreground = new SolidColorBrush(Color.FromRgb(0x8F, 0xF0, 0xD8));
            }
            else if (diff < 0)
            {
                txtSashihiki.Text = $"¥{-diff:N0}";
                lblSashihiki.Text = "不足";
                txtSashihiki.Foreground = new SolidColorBrush(Color.FromRgb(0x8F, 0xF0, 0xD8));
            }
            else
            {
                // 足りた！→ 「食券発券ボタンを押してください」表示
                txtSashihiki.Text = $"¥{diff:N0}";
                lblSashihiki.Text = "おつり";
                txtSashihiki.Foreground = new SolidColorBrush(Colors.Yellow);
                CashInputPanel.Visibility = Visibility.Collapsed;
                txtAfterMessage.Text = "食券発券ボタンを押してください";
                AfterIssuePanel.Visibility = Visibility.Visible;
            }
        }

        private void CashButton_Click(object sender, RoutedEventArgs e)
        {
            NormalPanel.Visibility = Visibility.Collapsed;
            CashInputPanel.Visibility = Visibility.Visible;
            txtAzukariInput.IsEnabled = true;
            txtAzukariInput.Focus();
        }

        private void HakkouButton_Click(object sender, RoutedEventArgs e)
        {
            if (_azukari < _goukei)
            {
                MessageBox.Show("お預かり金額が不足しています。", "エラー");
                return;
            }

            // 食券発行
            DateTime dt = DateTime.Now;
            string receiptId = "RCPT" + dt.Year + dt.Month + dt.Day + dt.Hour + dt.Minute + dt.Second;
            string filePath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"食券_{receiptId}.txt");

            var lines = new List<string>();
            lines.Add("＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝");
            lines.Add("　　　ラーメン山豚　食券");
            lines.Add("＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝");
            lines.Add("発行日時：" + dt.ToString("yyyy/MM/dd HH:mm:ss"));
            lines.Add("食券番号：" + receiptId);
            lines.Add("-------------------------");
            foreach (var item in _orderItems)
                lines.Add($"{item.Name}　¥{item.Price:#,0}");
            lines.Add("-------------------------");
            lines.Add($"合計金額：¥{_goukei:#,0}");
            lines.Add($"お預かり：¥{_azukari:#,0}");
            lines.Add($"おつり　：¥{_azukari - _goukei:#,0}");
            lines.Add("＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝");
            lines.Add("ありがとうございました！");

            System.IO.File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8);

            // メッセージをお取り忘れに変更
            txtAfterMessage.Text = "お取り忘れにご注意ください";

            // もどるボタンを無効にする
            BtnModoru.IsEnabled = false;

            // 3秒後にMainWindowに戻る
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                var main = new MainWindow();
                main.Show();
                this.Close();
            };
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {      
            this.Close();
        }
    }
}