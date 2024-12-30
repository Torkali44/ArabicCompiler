using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UIEnhancement
{
    public partial class Form1 : Form
    {
        private ListBox lstTokens;
        private TextBox txtInput;
        private TextBox txtOutput;

        private Scanner scanner;

        public Form1()
        {
            InitializeComponent();
            scanner = new Scanner(); 
            CustomizeUI();
        }

        private void CustomizeUI()
        {
            this.Text = "Arabic Compiler";
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E4456"); 
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Size = new System.Drawing.Size(1000, 800);

            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = System.Drawing.ColorTranslator.FromHtml("#24ADB1") 
            };
            Label headerLabel = new Label
            {
                Text = "Arabic Compiler",
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 22, System.Drawing.FontStyle.Bold),
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            headerPanel.Controls.Add(headerLabel);
            this.Controls.Add(headerPanel);

            Label inputLabel = new Label
            {
                Text = "Input:",
                Location = new System.Drawing.Point(30, 120),
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                ForeColor = System.Drawing.Color.White 
            };
            this.Controls.Add(inputLabel);

            txtInput = new TextBox
            {
                Name = "txtInput",
                Multiline = true,
                Size = new System.Drawing.Size(900, 150), 
                Location = new System.Drawing.Point(30, 150),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Segoe UI", 12),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black, 
                ScrollBars = ScrollBars.Vertical 
            };
            this.Controls.Add(txtInput);

            
            lstTokens = new ListBox
            {
                Name = "lstTokens",
                Size = new System.Drawing.Size(400, 300),
                Location = new System.Drawing.Point(30, 320),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Segoe UI", 10),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black 
            };
            this.Controls.Add(lstTokens);

            Panel buttonPanel = new Panel
            {
                Size = new System.Drawing.Size(220, 120),
                Location = new System.Drawing.Point(500, 350)
            };
            Button btnRunScanner = new Button
            {
                Text = "Run Scanner",
                Size = new System.Drawing.Size(200, 50),
                Location = new System.Drawing.Point(10, 10),
                BackColor = System.Drawing.ColorTranslator.FromHtml("#24ADB1"), 
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnRunScanner.FlatAppearance.BorderSize = 0;
            btnRunScanner.Click += BtnRunScanner_Click;

            Button btnRunParser = new Button
            {
                Text = "Run Parser",
                Size = new System.Drawing.Size(200, 50),
                Location = new System.Drawing.Point(10, 70),
                BackColor = System.Drawing.ColorTranslator.FromHtml("#24ADB1"), 
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnRunParser.FlatAppearance.BorderSize = 0;
            btnRunParser.Click += BtnRunParser_Click;

            buttonPanel.Controls.Add(btnRunScanner);
            buttonPanel.Controls.Add(btnRunParser);
            this.Controls.Add(buttonPanel);

            Label outputLabel = new Label
            {
                Text = "Output:",
                Location = new System.Drawing.Point(30, 640),
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                ForeColor = System.Drawing.Color.White 
            };
            this.Controls.Add(outputLabel);

            txtOutput = new TextBox
            {
                Name = "txtOutput",
                Multiline = true,
                Size = new System.Drawing.Size(900, 100), 
                Location = new System.Drawing.Point(30, 670),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Segoe UI", 12),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black, 
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical 
            };
            this.Controls.Add(txtOutput);
        }

        private void BtnRunScanner_Click(object sender, EventArgs e)
        {
            try
            {
                string input = txtInput.Text;

                Scanner scanner = new Scanner();

                var tokens = scanner.Scan(input);

                lstTokens.Items.Clear();

                foreach (var (tokenType, value) in tokens)
                {
                    lstTokens.Items.Add($"{tokenType}: {value}");
                }

                txtOutput.Text = "Scanning completed successfully.";
            }
            catch (Exception ex)
            {
                txtOutput.Text = $"Error: {ex.Message}";
            }
        }

        private void BtnRunParser_Click(object sender, EventArgs e)
        {
            lstTokens.Items.Clear();
            txtOutput.Clear();

            string input = txtInput.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                txtOutput.Text = "No input provided. Please enter input and run the scanner.";
                return;
            }

            List<(string TokenType, string Value)> tokens;
            try
            {
                tokens = scanner.Scan(input);
            }
            catch (Exception ex)
            {
                txtOutput.Text = $"Parser Error: {ex.Message}";
                return;
            }

            try
            {
                Parser parser = new Parser(tokens);

                using (var writer = new System.IO.StringWriter())
                {
                    Console.SetOut(writer);

                    parser.ParseProgram();
                    var parseTreeOutput = writer.ToString();
                    var parseTreeLines = parseTreeOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    foreach (var line in parseTreeLines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            lstTokens.Items.Add(line.Trim());
                        }
                    }
                }

                txtOutput.Text = "Parser ran successfully! Parse Tree is displayed above.";
            }
            catch (Exception ex)
            {
                txtOutput.Text = $"Parsing Error: {ex.Message}";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
