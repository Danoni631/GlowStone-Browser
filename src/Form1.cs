using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace Glowstone
{
    public partial class Form1 : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem menuLanguage;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuHistory;
        private ToolStripMenuItem menuFav;
        private Panel toolBar;
        private Panel secondToolBar;
        private Button btnBack;
        private Button btnForward;
        private Button btnStop;
        private Button btnRefresh;
        private Button btnHome;
        private TextBox txtAddress;
        private Label lblAddr;
        private Button btnGo;
        private Button btnFav;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private WebView2 webViewer;


        private string currentLang = "en";
        private Dictionary<string, Dictionary<string, string>> translations;

        private readonly Color ieClassicGray = Color.FromArgb(241, 239, 226);


        public class HistoryEntry
        {
            public DateTime Time { get; set; }
            public string Url
            {
                get; set;
            }
        }

        public class FavEntry
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
        public Form1()
        {
            InitializeTranslations();
            DetectSystemLanguage();
            InitializeComponentLayout();
            InitializeWebView();
        }

        private List<HistoryEntry> historyls = new List<HistoryEntry>();
        private List<FavEntry> favoritesList = new List<FavEntry>();

        private void InitializeTranslations()
        {
            translations = new Dictionary<string, Dictionary<string, string>>();

            translations["pt"] = new Dictionary<string, string>
            {
                { "Title", "GlowStone versão 1.02" },
                { "Back", " ⇦ Voltar" },
                { "Forward", "⇨ Avançar" },
                { "Stop", "✕ Parar" },
                { "Refresh", "↻ Atualizar" },
                { "Home", "⌂ Inicial" },
                { "Go", "Ir" },
                { "Addr", "Endereço" },
                { "StatusDone", "Concluído" },
                { "StatusLoading", "Abrindo a página {0}..." },
                { "MenuLang", "&Idioma" },
                { "MenuHelp", "&Ajuda" },
                { "MenuHis", "&Histórico" },
                { "HisTitle", "Histórico de navegação" },
                { "HistoryColTime", "Hora" },
                { "HistoryColUrl", "Endereço (URL)" },
                { "HistoryEmpty", "Nenhum histórico registrado ainda." },
                { "AboutTitle " , "GlowStone - Sobre"},
                { "AboutText", "GlowStone versão 1.02\n\nNavegador feito do zero de código aberto pra ser uma imitação estável do Internet Explorer com motor Edge/WebView2 pra trazer um desempenho melhor do que o Internet Explorer original.\n\nO Internet Explorer original é feito pela Microsoft e esse navegador não tem nenhum código dele.\n\n© Danoni631 - 2026-2026" },
            };

            translations["en"] = new Dictionary<string, string>
            {
                { "Title", "GlowStone version 1.02" },
                { "Back", " ⇦ Back" },
                { "Forward", "⇨ Forward" },
                { "Stop", "✕ Stop" },
                { "Refresh", "↻ Refresh" },
                { "Home", "⌂ Home" },
                { "Go", "Go" },
                { "Addr", "Address" },
                { "StatusDone", "Done" },
                { "StatusLoading", "Opening page {0}..." },
                { "MenuLang", "&Language" },
                { "MenuHelp", "&Help" },
                { "MenuHis", "&History" },
                { "HisTitle", "Browsing history" },
                { "HistoryColTime", "Time" },
                { "HistoryColUrl", "Addres (URL)" },
                { "HistoryEmpty", "No historys registred here." },
                { "AboutTitle", "GlowStone - About"},
                { "AboutText", "GlowStone version 1.02\n\nBrowser made of 0 and open-source to be a more stable version of Internet Explorer with engine Edge/WebView2 to have a perfomance better than original Internet Explorer.\n\nThe original Internet Explorer was made by Microsoft and this browser don't have any code of him.\n\n© Danoni631 - 2026-2026" },
            };

            translations["es"] = new Dictionary<string, string>
            {
                { "Title", "GlowStone versión 1.02" },
                { "Back", " ⇦ Atrás" },
                { "Forward", "⇨ Adelante" },
                { "Stop", "✕ Detener" },
                { "Refresh", "↻ Actualizar" },
                { "Home", "⌂ Inicio" },
                { "Go", "Ir" },
                { "Addr", "Dirección" },
                { "StatusDone", "Listo" },
                { "StatusLoading", "Abriendo la página {0}..." },
                { "MenuLang", "&Idioma" },
                { "MenuHelp", "&Ayuda" },
                { "MenuHis", "&Historial"},
                { "HisTitle", "Historial de navegación" },
                { "HistoryColTime", "Hora" },
                { "HistoryColUrl", "Direción (URL)" },
                { "HistoryEmpty", "Aún no hay historial registrado." },
                { "AboutTitle", "GlowStone - Sobre" },
                { "AboutText", "GlowStone versión 1.02\n\nNavegador hecho desde cero, de código abierto, para ser una imitación estable del Internet Explorer con motor Edge/WebView2 y así lograr un mejor rendimiento que el Internet Explorer original.\n\nEl Internet Explorer original fue hecho por Microsoft y este navegador no tiene nada de su código.\n\n© Danoni631 - 2026-2026" },
            };
        }

        private void DetectSystemLanguage()
        {
            string sysLang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            if (translations.ContainsKey(sysLang))
                currentLang = sysLang;
            else
                currentLang = "en";
        }

        private void InitializeComponentLayout()
        {
            this.Size = new Size(1024, 768);
            this.BackColor = ieClassicGray;
            this.Icon = SystemIcons.Shield;

            menuStrip = new MenuStrip { BackColor = ieClassicGray };
            menuLanguage = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuFav = new ToolStripMenuItem();

            var menuPt = new ToolStripMenuItem("Português", null, (s, e) => ChangeLanguage("pt"));
            var menuEn = new ToolStripMenuItem("English", null, (s, e) => ChangeLanguage("en"));
            var menuEs = new ToolStripMenuItem("Español", null, (s, e) => ChangeLanguage("es"));

            var menuAbout = new
            ToolStripMenuItem
            (
                "Sobre | About | Sobre",
                null, (s, e) => ShowAboutBox()
            );

            menuHistory = new ToolStripMenuItem("", null, (s, e) => ShowHistoryWindow());

            menuLanguage.DropDownItems.AddRange(new ToolStripItem[] { menuPt, menuEn, menuEs });
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout });


            menuStrip.Items.Add(menuLanguage);
            menuStrip.Items.Add(menuFav);
            menuStrip.Items.Add(menuHistory);
            menuStrip.Items.Add(menuHelp);

            toolBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = ieClassicGray,
                Padding = new Padding(5)
            };

            secondToolBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = ieClassicGray,
                Padding = new Padding(5)
            };

            btnBack = CreateIEButton(0);
            btnBack.Click += (s, e) =>
            {
                if
                (
                    webViewer?.CoreWebView2 != null &&
                    webViewer.CanGoBack
                )
                {
                    webViewer.GoBack();
                }
            };

            btnForward = CreateIEButton(80);
            btnForward.Click += (s, e) =>
            {
                if
                (
                    webViewer?.CoreWebView2 != null &&
                    webViewer.CanGoForward
                )
                {
                    webViewer.GoForward();
                }
            };

            btnStop = CreateIEButton(160);
            btnStop.Click += (s, e) => webViewer?.CoreWebView2?.Stop();

            btnRefresh = CreateIEButton(230);
            btnRefresh.Click += (s, e) => webViewer?.Reload();

            btnHome = CreateIEButton(310);
            btnHome.Click += (s, e) =>
            webViewer?.CoreWebView2?.Navigate
            (
                "file://C:/users/danun/source/repos/GlowStone/GlowStone/homepage/main.html"
            );

            lblAddr = new Label
            {
                Location = new Point(10, 12),
                Width = 20,
                Font = new Font("Tahoma", 10),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };

            txtAddress = new TextBox
            {
                Location = new Point(80, 12),
                Width = 50,
                Font = new Font("Tahoma", 10),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            txtAddress.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) NavigateToUrl(); };

            btnGo = new Button
            {
                Location = new Point(140, 10),
                Size = new Size(40, 25),
                Font = new Font("Tahoma", 8, FontStyle.Bold),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                FlatStyle = FlatStyle.System
            };


            btnGo.Click += (s, e) => NavigateToUrl();

            toolBar.Controls.AddRange
            (
                new Control[]
                {
                    btnBack, btnForward, btnStop,
                    btnRefresh, btnHome
                }
            );

            secondToolBar.Controls.AddRange
            (
                new Control[]
                {
                    txtAddress, lblAddr, btnGo
                }
            );

            statusStrip = new StatusStrip { BackColor = ieClassicGray };
            statusLabel = new ToolStripStatusLabel { Font = new Font("Tahoma", 8) };
            statusStrip.Items.Add(statusLabel);

            webViewer = new WebView2 { Dock = DockStyle.Fill };

            webViewer.NavigationStarting += (s, e) =>
            {
                statusLabel.Text = string.Format(translations[currentLang]["StatusLoading"], e.Uri);
            };

            webViewer.NavigationCompleted += (s, e) =>
            {
                statusLabel.Text = translations[currentLang]["StatusDone"];
                if (webViewer.Source != null)
                    txtAddress.Text = webViewer.Source.ToString();
            };

            ApplyLanguageStrings();

            this.Controls.Add(webViewer);
            this.Controls.Add(secondToolBar);
            this.Controls.Add(toolBar);
            this.Controls.Add(menuStrip);
            this.Controls.Add(statusStrip);
            this.MainMenuStrip = menuStrip;
        }

        private Button CreateIEButton(int xPosition)
        {
            return new Button
            {
                Location = new Point(xPosition, 8),
                Size = new Size(75, 28),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Font = new Font("Tahoma", 8.5f),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ieClassicGray
            };
        }

        private void ApplyLanguageStrings()
        {
            var langDict = translations[currentLang];

            this.Text = langDict["Title"];
            menuLanguage.Text = langDict["MenuLang"];
            menuHelp.Text = langDict["MenuHelp"];
            //btnFav.Text = langDict["MenuFav"];
            menuHistory.Text = langDict["MenuHis"];
            btnBack.Text = langDict["Back"];
            btnForward.Text = langDict["Forward"];
            btnStop.Text = langDict["Stop"];
            btnRefresh.Text = langDict["Refresh"];
            btnHome.Text = langDict["Home"];
            lblAddr.Text = langDict["Addr"];
            btnGo.Text = langDict["Go"];

            if (webViewer?.CoreWebView2 == null || !webViewer.CanGoBack && !webViewer.CanGoForward)
            {
                statusLabel.Text = langDict["StatusDone"];
            }
        }

        private void ChangeLanguage(string langCode)
        {
            if (translations.ContainsKey(langCode))
            {
                currentLang = langCode;
                ApplyLanguageStrings();
            }
        }

        private async void InitializeWebView()
        {
            await webViewer.EnsureCoreWebView2Async(null);
            //YOU CAN CHANGE THE PATH TO LOAD HOMEPAGE
            webViewer.CoreWebView2.Navigate
            ("file://C:/users/danun/source/repos/GlowStone/GlowStone/homepage/main.html");
        }

        private void NavigateToUrl()
        {
            string url = txtAddress.Text.Trim();
            if (string.IsNullOrEmpty(url)) return;

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }

            webViewer?.CoreWebView2?.Navigate(url);
        }

        private void ShowHistoryWindow()
        {
            var langDict = translations[currentLang];

            Form historyForm = new Form
            {
                Text = langDict["HistoryTitle"],
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = ieClassicGray,
                FormBorderStyle = FormBorderStyle.SizableToolWindow
            };

            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Tahoma", 9f)
            };

            listView.Columns.Add(langDict["HistoryColTime"], 120);
            listView.Columns.Add(langDict["HistoryColUrl"], 440);

            for (int i = historyls.Count - 1; i >= 0; i--)
            {
                var entry = historyls[i];
                var item = new ListViewItem(entry.Time.ToString("HH:mm:ss"));
                item.SubItems.Add(entry.Url);
                listView.Items.Add(item);
            }

            listView.DoubleClick += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    string targetUrl = listView.SelectedItems[0].SubItems[1].Text;
                    webViewer?.CoreWebView2?.Navigate(targetUrl);
                    historyForm.Close();
                }
            };

            historyForm.Controls.Add(listView);
            historyForm.ShowDialog(this);
        }

        private void ShowAboutBox()
        {
            var langDict = translations[currentLang];
            
            MessageBox.Show
            (
                langDict["AboutText"],
                langDict["AboutTitle"],
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }

    public static class PromptDialog
    {
        public static string Show(string text, string caption, string defaultValue = "")
        {
            Form prompt = new Form()
            {
                Width = 420,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(241, 239, 226)
            };

            Label textLabel = new Label()
            {
                Left = 15, Top = 15,
                Text = text, AutoSize = true,
                Font = new Font("Tahoma", 8.5f)
            };

            TextBox textBox = new TextBox()
            {
                Left = 15, Top = 38,
                Width = 375,
                Text = defaultValue,
                Font = new Font("Tahoma", 9f)
            };

            Button confirmation = new Button()
            {
                Text = "OK", Left = 225,
                Width = 80, Top = 75,
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.System
            };

            Button cancel = new Button()
            {
                Text = "Cancelar | Cancel | Cancelar",
                Left = 310, Width = 80,
                Top = 75, DialogResult = DialogResult.Cancel,
                FlatStyle = FlatStyle.System
            };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
    }
}
