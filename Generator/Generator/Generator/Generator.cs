using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KruispuntGroep4.Generator
{
	/// <summary>
	/// Class used to send messages to Controller and shows logs of Simulator.
	/// </summary>
	public class Generator : Form
	{
		private ComboBox cbJsonType;	// ComboBox used to choose JSON type.
		private IContainer components = null;	// Required designer variable.
		private JsonGenerator jsonGenerator = new JsonGenerator();	// JsonGenerator used to generate JSON data file.
		private TextBox tbJsonGenerator;	// TextBox used to contain a box to input text.

		public Generator()
		{
			InitializeComponent();

			// Generate GUI
			Button btnJsonGenerator = new Button();
			btnJsonGenerator.Click += new EventHandler(Generate);
			btnJsonGenerator.Location = new Point(21 * Font.Height - 3, Font.Height - 6);
			btnJsonGenerator.Parent = this;
			btnJsonGenerator.Size = new Size(9 * Font.Height, 2 * Font.Height);
			btnJsonGenerator.Text = Strings.GenerateJSON;

			cbJsonType = new ComboBox();
			cbJsonType.Items.Add(Strings.JsonTypeInput);
			cbJsonType.Items.Add(Strings.JsonTypeDetector);
			cbJsonType.Items.Add(Strings.JsonTypeStoplight);
			cbJsonType.Location = new Point(14 * Font.Height, Font.Height - 3);
			cbJsonType.Parent = this;
			cbJsonType.Size = new Size(6 * Font.Height, 2 * Font.Height);
			cbJsonType.Text = Strings.JsonTypeInput;

			FormBorderStyle = FormBorderStyle.FixedSingle;

			Label lblJsonGenerator = new Label();
			lblJsonGenerator.AutoSize = true;
			lblJsonGenerator.Location = new Point(Font.Height - 3, Font.Height);
			lblJsonGenerator.Parent = this;
			lblJsonGenerator.Text = Strings.JsonGenerator;

			MaximizeBox = false;

			Size = new Size(31 * Font.Height, 5 * Font.Height);

			tbJsonGenerator = new TextBox();
			tbJsonGenerator.Location = new Point(9 * Font.Height, Font.Height - 3);
			tbJsonGenerator.MaxLength = 5;
			tbJsonGenerator.Parent = this;
			tbJsonGenerator.Size = new Size(4 * Font.Height, 2 * Font.Height);

			Text = Strings.Generator;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Generator));
			this.SuspendLayout();
			// 
			// Generator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(288, 266);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Generator";
			this.Text = "Generator";
			this.ResumeLayout(false);

		}

		#endregion

		/// <summary>
		/// Generates JSON input file.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void Generate(object sender, EventArgs e)
		{
			int nrOfInputs = -1;

			bool onlyNumbers = true;

			if (!tbJsonGenerator.Text.Equals(string.Empty))
			{
				foreach (char c in tbJsonGenerator.Text)
				{
					if (!char.IsNumber(c))
						onlyNumbers = false;
				}

				if (onlyNumbers)
					nrOfInputs = int.Parse(tbJsonGenerator.Text);
			}

			if (nrOfInputs > 0)
			{
				for (int i = 0; i < nrOfInputs; i++)
				{
					jsonGenerator.GenerateJSON(cbJsonType.SelectedItem.ToString(), i, nrOfInputs);
				}

				jsonGenerator.SaveJSONFile(cbJsonType.SelectedItem.ToString());
			}
		}
	}
}