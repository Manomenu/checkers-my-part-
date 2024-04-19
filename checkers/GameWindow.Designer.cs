namespace checkers
{
    partial class GameWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            BoardPictureBox = new PictureBox();
            StartGameButton = new Button();
            BlackSettingsGroupBox = new GroupBox();
            BlackPlayerRealRadioButton = new RadioButton();
            BlackPlayerAIRadioButton = new RadioButton();
            WhiteSettingsGroupBox = new GroupBox();
            WhitePlayerRealRadioButton = new RadioButton();
            WhitePlayerAIRadioButton = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)BoardPictureBox).BeginInit();
            BlackSettingsGroupBox.SuspendLayout();
            WhiteSettingsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // BoardPictureBox
            // 
            BoardPictureBox.BackgroundImage = Properties.Resources.board;
            BoardPictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            BoardPictureBox.Location = new Point(12, 29);
            BoardPictureBox.Margin = new Padding(3, 2, 3, 2);
            BoardPictureBox.Name = "BoardPictureBox";
            BoardPictureBox.Size = new Size(480, 480);
            BoardPictureBox.TabIndex = 0;
            BoardPictureBox.TabStop = false;
            BoardPictureBox.MouseClick += BoardPictureBox_MouseClick;
            // 
            // StartGameButton
            // 
            StartGameButton.Location = new Point(537, 29);
            StartGameButton.Name = "StartGameButton";
            StartGameButton.Size = new Size(75, 23);
            StartGameButton.TabIndex = 1;
            StartGameButton.Text = "Start Game";
            StartGameButton.UseVisualStyleBackColor = true;
            StartGameButton.Click += StartGameButton_Click;
            // 
            // BlackSettingsGroupBox
            // 
            BlackSettingsGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            BlackSettingsGroupBox.Controls.Add(BlackPlayerRealRadioButton);
            BlackSettingsGroupBox.Controls.Add(BlackPlayerAIRadioButton);
            BlackSettingsGroupBox.Location = new Point(535, 58);
            BlackSettingsGroupBox.Name = "BlackSettingsGroupBox";
            BlackSettingsGroupBox.Size = new Size(121, 71);
            BlackSettingsGroupBox.TabIndex = 2;
            BlackSettingsGroupBox.TabStop = false;
            BlackSettingsGroupBox.Text = "Black Settings";
            // 
            // BlackPlayerRealRadioButton
            // 
            BlackPlayerRealRadioButton.AutoSize = true;
            BlackPlayerRealRadioButton.Location = new Point(6, 47);
            BlackPlayerRealRadioButton.Name = "BlackPlayerRealRadioButton";
            BlackPlayerRealRadioButton.Size = new Size(82, 19);
            BlackPlayerRealRadioButton.TabIndex = 1;
            BlackPlayerRealRadioButton.TabStop = true;
            BlackPlayerRealRadioButton.Text = "Real Player";
            BlackPlayerRealRadioButton.UseVisualStyleBackColor = true;
            // 
            // BlackPlayerAIRadioButton
            // 
            BlackPlayerAIRadioButton.AutoSize = true;
            BlackPlayerAIRadioButton.Location = new Point(6, 22);
            BlackPlayerAIRadioButton.Name = "BlackPlayerAIRadioButton";
            BlackPlayerAIRadioButton.Size = new Size(36, 19);
            BlackPlayerAIRadioButton.TabIndex = 0;
            BlackPlayerAIRadioButton.TabStop = true;
            BlackPlayerAIRadioButton.Text = "AI";
            BlackPlayerAIRadioButton.UseVisualStyleBackColor = true;
            // 
            // WhiteSettingsGroupBox
            // 
            WhiteSettingsGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            WhiteSettingsGroupBox.Controls.Add(WhitePlayerRealRadioButton);
            WhiteSettingsGroupBox.Controls.Add(WhitePlayerAIRadioButton);
            WhiteSettingsGroupBox.Location = new Point(535, 139);
            WhiteSettingsGroupBox.Name = "WhiteSettingsGroupBox";
            WhiteSettingsGroupBox.Size = new Size(121, 71);
            WhiteSettingsGroupBox.TabIndex = 3;
            WhiteSettingsGroupBox.TabStop = false;
            WhiteSettingsGroupBox.Text = "White Settings";
            // 
            // WhitePlayerRealRadioButton
            // 
            WhitePlayerRealRadioButton.AutoSize = true;
            WhitePlayerRealRadioButton.Location = new Point(6, 47);
            WhitePlayerRealRadioButton.Name = "WhitePlayerRealRadioButton";
            WhitePlayerRealRadioButton.Size = new Size(82, 19);
            WhitePlayerRealRadioButton.TabIndex = 2;
            WhitePlayerRealRadioButton.TabStop = true;
            WhitePlayerRealRadioButton.Text = "Real Player";
            WhitePlayerRealRadioButton.UseVisualStyleBackColor = true;
            // 
            // WhitePlayerAIRadioButton
            // 
            WhitePlayerAIRadioButton.AutoSize = true;
            WhitePlayerAIRadioButton.Location = new Point(6, 22);
            WhitePlayerAIRadioButton.Name = "WhitePlayerAIRadioButton";
            WhitePlayerAIRadioButton.Size = new Size(36, 19);
            WhitePlayerAIRadioButton.TabIndex = 1;
            WhitePlayerAIRadioButton.TabStop = true;
            WhitePlayerAIRadioButton.Text = "AI";
            WhitePlayerAIRadioButton.UseVisualStyleBackColor = true;
            // 
            // GameWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(676, 540);
            Controls.Add(WhiteSettingsGroupBox);
            Controls.Add(BlackSettingsGroupBox);
            Controls.Add(StartGameButton);
            Controls.Add(BoardPictureBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(3, 2, 3, 2);
            MaximumSize = new Size(692, 579);
            MinimumSize = new Size(692, 579);
            Name = "GameWindow";
            Text = "Checkers";
            KeyDown += GameWindow_KeyDown;
            ((System.ComponentModel.ISupportInitialize)BoardPictureBox).EndInit();
            BlackSettingsGroupBox.ResumeLayout(false);
            BlackSettingsGroupBox.PerformLayout();
            WhiteSettingsGroupBox.ResumeLayout(false);
            WhiteSettingsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox BoardPictureBox;
        private Button StartGameButton;
        private GroupBox BlackSettingsGroupBox;
        private GroupBox WhiteSettingsGroupBox;
        private RadioButton BlackPlayerRealRadioButton;
        private RadioButton BlackPlayerAIRadioButton;
        private RadioButton WhitePlayerAIRadioButton;
        private RadioButton WhitePlayerRealRadioButton;
    }
}
