using Services.DTOs;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        UnevaluatedObjectManager UnevaluatedObjectManager = new UnevaluatedObjectManager();
        public string attributeList = "";
        public string programText = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseAttributes_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogAttributes.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Create a stream which points to the file
                Stream fs = openFileDialogAttributes.OpenFile();
                //Create a reader using the stream
                StreamReader reader = new StreamReader(fs);
                //Read Contents
                txtAttributesFileName.Text = openFileDialogAttributes.FileName;
                attributeList = reader.ReadToEnd();
                //Close the reader and the stream
                reader.Close();
            }

        }

        private void btnBrowseProgramText_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogProgramText.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Create a stream which points to the file
                Stream fs = openFileDialogProgramText.OpenFile();
                //Create a reader using the stream
                StreamReader reader = new StreamReader(fs);
                //Read Contents
                txtProgramTextFileName.Text = openFileDialogProgramText.FileName;
                programText = reader.ReadToEnd();
                //Close the reader and the stream
                reader.Close();
            }
        }

        private void btnRunAnalysis_Click(object sender, EventArgs e)
        {
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);
            UnevaluatedObjectManager.Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.UnevaluatedObjects);
            UnevaluatedObjectManager.Case2Manager.InitializeCase2Objects(UnevaluatedObjectManager.UnevaluatedObjects);

            string results = "Case 1 Objects:\n";
            foreach(CaseObject co in UnevaluatedObjectManager.Case1Manager.Case1Objects)
            {
                results += String.Format("{0}:\n", co.Name);
                foreach(string s in co.MethodNames)
                {
                    results += String.Format("\t{0}\n", s);
                }
            }
            results += "\n\n";
            results += "Case 2 Objects:\n";
            foreach (CaseObject co in UnevaluatedObjectManager.Case2Manager.Case2Objects)
            {
                results += String.Format("{0}:\n", co.Name);
                foreach (string s in co.MethodNames)
                {
                    results += String.Format("\t{0}\n", s);
                }
            }
            txtResults.Text = results;
        }
    }
}
