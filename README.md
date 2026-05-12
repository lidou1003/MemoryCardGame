# HW2_MemoryCardGame
遊戲初始畫面：
進入遊戲後，畫面中央會顯示 16 張蓋上的撲克牌背面。左下方顯示「嘗試步數：0」，右下角設有「重新開始」按鈕。
<img width="540" height="691" alt="image" src="https://github.com/user-attachments/assets/7f3eefa7-06ed-4d77-9386-d53f961cd33b" />
<br>
2.進行翻牌與記憶：
玩家用滑鼠點擊任意一張尚未翻開的卡片，系統會即時顯示卡片正面的花色與數字。接著點擊第二張卡片，此時左下角的「嘗試步數」會自動加 1。
<img width="540" height="686" alt="image" src="https://github.com/user-attachments/assets/2079738c-3d18-482b-9203-613354b122fd" />
<br>
3.判定配對結果：
配對成功：若兩張卡片圖案完全一致，系統會播放正確音效 (correct.wav)，並將這兩張牌將固定維持翻開狀態，無法再被點擊。
配對失敗：若兩張卡片圖案不同，系統會播放錯誤音效 (wrong.wav)，卡片會在停留約 1.2 秒後自動蓋回背面，考驗玩家記住卡片位置的能力。
<img width="542" height="687" alt="image" src="https://github.com/user-attachments/assets/95f02d07-208f-4a48-b1e5-8c7b1a75aabc" />
<br>
4.遊戲通關與重置：
當全部 8 組卡片皆成功配對時，系統會播放過關音效 (win.wav)，並跳出「遊戲結束」提示視窗，顯示玩家總共耗費的步數。
玩家可隨時點擊右下角的「重新開始」按鈕，系統會重新洗牌並將步數歸零，展開新局。
<img width="546" height="685" alt="image" src="https://github.com/user-attachments/assets/4574e906-f34d-420a-9bd2-0235ff46eb98" />
