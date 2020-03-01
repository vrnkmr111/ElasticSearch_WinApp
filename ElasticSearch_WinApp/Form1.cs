using ElasticSearch_WinApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElasticSearch_WinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var ElasticConn = new ElasticConn();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            var myJson = new
            {
                id = textBox1.Text,   
                name = textBox2.Text,
                original_voice_actor = textBox3.Text,
                animated_debut = textBox4.Text
            };
            var response = ElasticConn.EsClient().Index(myJson, i => i
                          .Index("disney")
                          .Type("character")
                          .Id(myJson.id)
                          );
            if (response.Result.ToString().ToLower() == "created")
                MessageBox.Show("Inserted Successfully");
            else
                MessageBox.Show("Failed");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var response = ElasticConn.EsClient().Search<DocumentAttributescs>(s => s
      .Index("disney")
      .Type("character")
      .Query(q => q.Term(t => t.Field("_id").Value(textBox1.Text)))); //Search based on _id               


            var docAttrs = new List<DocumentAttributescs>();
            docAttrs.Clear();
            foreach (var hit in response.Hits)
            {
                var docAttr = new DocumentAttributescs();
                docAttr.id  = hit.Id.ToString();
                docAttr.name = hit.Source.name.ToString();
                docAttr.original_voice_actor = hit.Source.original_voice_actor.ToString();
                docAttr.animated_debut = hit.Source.animated_debut.ToString();
                docAttrs.Add(docAttr);
            }
            dataGridView1.DataSource = docAttrs;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var response = ElasticConn.EsClient().Search<DocumentAttributescs>(s => s
                .Index("disney")
                .Type("character")
                .From(0)
                .Size(1000)
                .Query(q => q.MatchAll()));
            var docAttrs = new List<DocumentAttributescs>();
            docAttrs.Clear();
            foreach (var hit in response.Hits)
            {
                var docAttr = new DocumentAttributescs();
                docAttr.id = hit.Id.ToString();
                docAttr.name = hit.Source.name.ToString();
                docAttr.original_voice_actor = hit.Source.original_voice_actor.ToString();
                docAttr.animated_debut = hit.Source.animated_debut.ToString();
                docAttrs.Add(docAttr);
            }
            var columns = from t in docAttrs
                          orderby t.id
                          select new
                          {
                              Name = t.name,
                              OriginalVoiceActor = t.original_voice_actor,
                              AnimatedDebut = t.animated_debut
                          };
            dataGridView1.DataSource = columns.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var response = ElasticConn.EsClient().Delete<DocumentAttributescs>(textBox1.Text, d => d
      .Index("disney")
      .Type("character"));

            if (response.Result.ToString().ToLower() == "deleted")
                MessageBox.Show("Deleted Successfully");
            else
                MessageBox.Show("data not found");
        }
    }
}
