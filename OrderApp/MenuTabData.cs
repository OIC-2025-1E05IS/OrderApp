using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp
{
    public class MenuTabData
    {
        public string BrandLabel { get; set; }
        public string MiniHeader { get; set; }
        public string SmallHeader { get; set; }
        public string LargeHeader { get; set; }

        public string[] Row0Names { get; set; }
        public int[] Row0Prices { get; set; }

        public int[] Row1Prices { get; set; }
        public int[] Row2Prices { get; set; }

        public string SidebarSizeName { get; set; }
        public string SidebarMiniName { get; set; }

        public string[] Row1Names { get; set; }
        public string[] Row2Names { get; set; }


        public static MenuTabData Get(MenuCategory category)
        {
            switch (category)
            {
                case MenuCategory.Ramen:
                    return new MenuTabData
                    {
                        BrandLabel = "ラーメン",
                        MiniHeader = "ミニラーメン (麺125g)",
                        SmallHeader = "小ラーメン (麺250g)",
                        LargeHeader = "大ラーメン (麺375g)",

                        Row0Names = new[] { "ミニラーメン", "小ラーメン", "大ラーメン" },
                        Row0Prices = new[] { 950, 1000, 1100 },

                        Row1Names = new[] { "ミニぶた（ラーメン）", "小ぶた（ラーメン）", "大ぶた（ラーメン）" },
                        Row1Prices = new[] { 1200, 1250, 1350 },

                        Row2Names = new[] { "ミニぶたダブル（ラーメン）", "小ぶたダブル（ラーメン）", "大ぶたダブル（ラーメン）" },
                        Row2Prices = new[] { 1400, 1450, 1550 },

                        SidebarSizeName = "小ラーメン",
                        SidebarMiniName = "ミニラーメン"
                    };


                case MenuCategory.Soupless:
                    return new MenuTabData
                    {
                        BrandLabel = "ラーメン",
                        MiniHeader = "ミニ汁なし (麺125g)",
                        SmallHeader = "小汁なし (麺250g)",
                        LargeHeader = "大汁なし (麺375g)",

                        Row0Names = new[] { "ミニ汁なし", "小汁なし", "大汁なし" },
                        Row0Prices = new[] { 1050, 1100, 1200 },

                        Row1Names = new[] { "ミニぶた（汁なし）", "小ぶた（汁なし）", "大ぶた（汁なし）" },
                        Row1Prices = new[] { 1300, 1350, 1450 },

                        Row2Names = new[] { "ミニぶたダブル（汁なし）", "小ぶたダブル（汁なし）", "大ぶたダブル（汁なし）" },
                        Row2Prices = new[] { 1500, 1550, 1650 },

                        SidebarSizeName = "小汁なし",
                        SidebarMiniName = "ミニ汁なし"
                    };


                case MenuCategory.Tsukemen:
                    return new MenuTabData
                    {
                        BrandLabel = "ラーメン",
                        MiniHeader = "ミニつけ麺 (麺125g)",
                        SmallHeader = "小つけ麺 (麺250g)",
                        LargeHeader = "大つけ麺 (麺375g)",

                        Row0Names = new[] { "ミニつけ麺", "小つけ麺", "大つけ麺" },
                        Row0Prices = new[] { 1050, 1100, 1200 },

                        Row1Names = new[] { "ミニぶた（つけ麺）", "小ぶた（つけ麺）", "大ぶた（つけ麺）" },
                        Row1Prices = new[] { 1300, 1350, 1450 },

                        Row2Names = new[] { "ミニぶたダブル（つけ麺）", "小ぶたダブル（つけ麺）", "大ぶたダブル（つけ麺）" },
                        Row2Prices = new[] { 1500, 1550, 1650 },

                        SidebarSizeName = "小つけ麺",
                        SidebarMiniName = "ミニつけ麺"
                    };


                default:
                    return null;
            }
        }
    }
}

