using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW2_MemoryCardGame
{
    public partial class frmMCG : Form
    {
        // 儲存 16 張牌對應的資源名稱
        private List<string> cardTags = new List<string>();
        private PictureBox firstClicked = null;
        private PictureBox secondClicked = null;
        private int moves = 0;
        private int matchesPairs = 0;

        // 宣告音效播放器 (對應你的新音檔名稱：flip, correct, win, wrong)
        private SoundPlayer flipSound;
        private SoundPlayer correctSound;
        private SoundPlayer winSound;
        private SoundPlayer wrongSound;

        public frmMCG()
        {
            InitializeComponent();
            InitializeAudio();
            SetupGame();
        }

        private void InitializeAudio()
        {
            // 載入 Resources 裡對應名稱的 wav 檔
            if (Properties.Resources.correct != null)
                correctSound = new SoundPlayer(Properties.Resources.correct);
            if (Properties.Resources.win != null)
                winSound = new SoundPlayer(Properties.Resources.win);
            if (Properties.Resources.wrong != null)
                wrongSound = new SoundPlayer(Properties.Resources.wrong);
        }

        private void SetupGame()
        {
            // 初始化遊戲數據
            moves = 0;
            matchesPairs = 0;
            lblStatus.Text = "嘗試步數: 0";
            firstClicked = null;
            secondClicked = null;

            // 準備牌組標籤 (card1 ~ card8 各兩張)
            cardTags.Clear();
            for (int i = 1; i <= 8; i++)
            {
                cardTags.Add("card" + i);
                cardTags.Add("card" + i);
            }

            // 洗牌演算法 (Fisher-Yates Shuffle)
            Random random = new Random();
            int n = cardTags.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string temp = cardTags[k];
                cardTags[k] = cardTags[n];
                cardTags[n] = temp;
            }

            // 清空並重新生成網格內的 PictureBox
            tableLayoutPanel1.Controls.Clear();
            for (int i = 0; i < 16; i++)
            {
                PictureBox pic = new PictureBox
                {
                    BackColor = Color.White,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(5),
                    // 使用卡片背面圖片 back.png
                    Image = Properties.Resources.back,
                    // 將洗好的圖案檔名記錄在 Tag 屬性中
                    Tag = cardTags[i]
                };

                pic.Click += Card_Click;
                tableLayoutPanel1.Controls.Add(pic);
            }
        }

        private void Card_Click(object sender, EventArgs e)
        {
            // 1. 如果計時器執行中 (正在等候翻回背面)，不接受點擊
            if (timer1.Enabled) return;

            PictureBox clickedCard = sender as PictureBox;
            if (clickedCard == null) return;

            // 2. 判斷這張牌是否已經被翻開了 (重複點擊第一張牌則忽略)
            if (clickedCard == firstClicked) return;

            // 3. 判斷這張牌是否已經沒有 Click 事件 (代表已經成功配對過，忽略點擊)
            // 透過檢查 Tag 內容是否已被清空來作為判定依據是最簡單安全的做法
            if (clickedCard.Tag == null) return;

            // 播放翻牌音效
            PlaySound(flipSound);

            // 顯示對應的正面上圖片
            clickedCard.Image = GetCardImage(clickedCard.Tag.ToString());

            // 強制系統立刻更新畫面顯示出正面圖案，解決翻牌看不清楚的問題
            clickedCard.Refresh();

            // 紀錄第一張翻開的牌
            if (firstClicked == null)
            {
                firstClicked = clickedCard;
                return;
            }

            // 紀錄第二張翻開的牌
            secondClicked = clickedCard;
            moves++;
            lblStatus.Text = $"嘗試步數: {moves}";

            // 判斷兩張牌的 Tag 是否相同
            if (firstClicked.Tag.ToString() == secondClicked.Tag.ToString())
            {
                // 配對成功！播放 correct.wav
                matchesPairs++;
                PlaySound(correctSound);

                // 解除安裝 Click 事件並清空 Tag，防止玩家後續誤點到已完成配對的牌
                firstClicked.Click -= Card_Click;
                secondClicked.Click -= Card_Click;
                firstClicked.Tag = null;
                secondClicked.Tag = null;

                firstClicked = null;
                secondClicked = null;

                // 檢查是否達成 8 組完成遊戲
                if (matchesPairs == 8)
                {
                    PlaySound(winSound);
                    MessageBox.Show($"恭喜過關！共花了 {moves} 步完成遊戲。", "遊戲結束", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // 配對失敗：播放 wrong.wav 並啟動計時器準備蓋牌
                PlaySound(wrongSound);
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 停止計時器
            timer1.Stop();

            // 蓋回背面圖片 back.png
            if (firstClicked != null) firstClicked.Image = Properties.Resources.back;
            if (secondClicked != null) secondClicked.Image = Properties.Resources.back;

            // 重置選取狀態
            firstClicked = null;
            secondClicked = null;
        }

        private Image GetCardImage(string imageName)
        {
            // 透過 ResourceManager 動態提取對應名稱的圖片資源
            object resource = Properties.Resources.ResourceManager.GetObject(imageName);
            return resource as Image;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            SetupGame();
        }

        private void PlaySound(SoundPlayer player)
        {
            try
            {
                player?.Play();
            }
            catch { /* 避免缺少音效檔案時導致程式崩潰 */ }
        }
    }
}