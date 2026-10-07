using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderApp
{
    public enum MenuCategory
    {
        Ramen, Soupless, Tsukemen
    }

    public partial class Menu : Window
    {
        private static readonly SolidColorBrush ActiveTabBrush = new(Color.FromRgb(230, 0, 18));
        private static readonly SolidColorBrush InactiveTabBrush = new(Color.FromRgb(51, 51, 51));

        private readonly (TextBlock Name, TextBlock Price)[][] _menuButtons;
        private MenuCategory _currentCategory = MenuCategory.Ramen;

        private int _totalPrice = 0;
        private int _deposit = 0;
        private readonly List<(string Name, int Price)> _orderItems = new();

        public Menu(MenuCategory category = MenuCategory.Ramen)
        {
            InitializeComponent();

            _menuButtons = new (TextBlock Name, TextBlock Price)[][]
            {
                new[] { (MenuR0C0Name, MenuR0C0Price), (MenuR0C1Name, MenuR0C1Price), (MenuR0C2Name, MenuR0C2Price) },
                new[] { (MenuR1C0Name, MenuR1C0Price), (MenuR1C1Name, MenuR1C1Price), (MenuR1C2Name, MenuR1C2Price) },
                new[] { (MenuR2C0Name, MenuR2C0Price), (MenuR2C1Name, MenuR2C1Price), (MenuR2C2Name, MenuR2C2Price) }
            };

            SelectCategory(category);
        }

        // ===============================
        // ▼ タブ切り替え
        // ===============================
        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not string tag)
                return;

            var category = tag switch
            {
                "Ramen" => MenuCategory.Ramen,
                "Soupless" => MenuCategory.Soupless,
                "Tsukemen" => MenuCategory.Tsukemen,
                _ => _currentCategory
            };

            SelectCategory(category);
        }

        private void SelectCategory(MenuCategory category)
        {
            _currentCategory = category;
            var data = MenuTabData.Get(category);

            ImgLeft.Height = 88;
            ImgCenter.Height = 88;
            ImgRight.Height = 88;

            UpdateTabStyles(category);
            UpdateHeaders(data);
            UpdateMenuGrid(data);
            UpdateSidebar(data);
        }

        private void UpdateTabStyles(MenuCategory category)
        {
            SetTabStyle(BtnTabRamen, category == MenuCategory.Ramen);
            SetTabStyle(BtnTabSoupless, category == MenuCategory.Soupless);
            SetTabStyle(BtnTabTsukemen, category == MenuCategory.Tsukemen);
        }

        private static void SetTabStyle(Button button, bool isActive)
        {
            button.Background = isActive ? ActiveTabBrush : InactiveTabBrush;
            button.Foreground = Brushes.White;
        }

        private void UpdateHeaders(MenuTabData data)
        {
            BrandCategoryText.Text = data.BrandLabel;
            HeaderMiniText.Text = data.MiniHeader;
            HeaderSmallText.Text = data.SmallHeader;
            HeaderLargeText.Text = data.LargeHeader;

            var imgPath = _currentCategory switch
            {
                MenuCategory.Ramen => "pack://application:,,,/Images/Main.png",
                MenuCategory.Soupless => "pack://application:,,,/Images/SoupNG.png",
                MenuCategory.Tsukemen => "pack://application:,,,/Images/tsukemen_new.png",
                _ => "pack://application:,,,/Images/Main.png"
            };

            var bmp = new BitmapImage(new Uri(imgPath, UriKind.Absolute));
            ImgLeft.Source = bmp;
            ImgCenter.Source = bmp;
            ImgRight.Source = bmp;
        }

        private void UpdateMenuGrid(MenuTabData data)
        {
            var allNames = new[] { data.Row0Names, data.Row1Names, data.Row2Names };
            var allPrices = new[] { data.Row0Prices, data.Row1Prices, data.Row2Prices };

            for (var row = 0; row < 3; row++)
                for (var col = 0; col < 3; col++)
                {
                    _menuButtons[row][col].Name.Text = allNames[row][col];
                    _menuButtons[row][col].Price.Text = $"{allPrices[row][col]:N0}円";
                }

            MenuR0C1Name.Foreground = new SolidColorBrush(Color.FromRgb(255, 69, 0));
            MenuR0C1Name.FontWeight = FontWeights.Bold;
        }

        private void UpdateSidebar(MenuTabData data)
        {
            SidebarLine1.Text = $"当店の{data.SidebarSizeName}は";
            SidebarRecommend1.Text = $"{data.SidebarSizeName} か";
            SidebarRecommend2.Text = $"{data.SidebarMiniName} を";
        }

        // ===============================
        // ▼ メニューボタンが押された時
        // ===============================
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.Content is not StackPanel sp) return;

            var nameText = (sp.Children[0] as TextBlock)?.Text ?? "";
            var priceText = (sp.Children[1] as TextBlock)?.Text ?? "";
            var price = ParsePrice(priceText);
            if (price <= 0) return;

            _orderItems.Add((nameText, price));
            _totalPrice += price;
            AddSelectedMenu($"{nameText}　{priceText}");
            UpdateDisplay();
        }

        // ===============================
        // ▼ トッピングが押された時
        // ===============================
        private void Topping_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            var content = btn.Content?.ToString() ?? "";
            var price = ParsePrice(content);
            if (price <= 0) return;

            var name = content.Split(' ')[0].Split('\n')[0];
            _orderItems.Add((name, price));
            _totalPrice += price;
            AddSelectedMenu(content.Replace("\r\n", " ").Replace("\n", " "));
            UpdateDisplay();
        }

        private void AddSelectedMenu(string text)
        {
            SelectedMenuList.Items.Add(text);
        }

        // ===============================
        // ▼ 金額ボタン（1000円など）
        // ===============================
        private void Money_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            var tagStr = btn.Tag?.ToString() ?? "0";
            if (!int.TryParse(tagStr, out var amount)) return;

            _deposit += amount > 0 ? amount : Math.Max(0, _totalPrice - _deposit);
            UpdateDisplay();
        }

        // ===============================
        // ▼ はじめからやりなおす
        // ===============================
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _totalPrice = 0;
            _deposit = 0;
            _orderItems.Clear();
            SelectedMenuList.Items.Clear();
            UpdateDisplay();
        }

        // ===============================
        // ▼ 発券ボタン
        // ===============================
        private void Issue_Click(object sender, RoutedEventArgs e)
        {
            if (_totalPrice <= 0)
            {
                MessageBox.Show("商品を選択してください。", "確認",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Kaikei k = new Kaikei(_totalPrice, _orderItems);
            k.Closed += (s, e) => this.Show(); // Kaikeiが閉じたらMenuを再表示
            this.Hide(); // Menuを隠す
            k.Show();
        }

        // ===============================
        // ▼ 表示更新
        // ===============================
        private void UpdateDisplay()
        {
            TextTotal.Text = $"￥ {_totalPrice:#,0}";
            TextDeposit.Text = $"￥ {_deposit:#,0}";
        }

        // ===============================
        // ▼ 価格文字列をパース
        // ===============================
        private static int ParsePrice(string text)
        {
            var match = Regex.Match(text, @"[\d,]+(?=円)");
            if (!match.Success) return 0;
            return int.TryParse(match.Value.Replace(",", ""), out var value) ? value : 0;
        }

        private void Top_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Owner?.Show();
            this.Close();
        }
    }
}