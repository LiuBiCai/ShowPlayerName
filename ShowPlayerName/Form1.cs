using AppNerve.Expands.StringExpand;
using AppNerve.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShowPlayerName
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        playerInfos playersInfo = JsonConvert.DeserializeObject<playerInfos>(
                    Encoding.UTF8.GetString(Resource.players));
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {           
           // nameBox.Text=playersInfo.GetName(int.Parse(idBox.Text));
        }
        public class playerInfos
        {
            public playerInfo[] LegendsPlayers { get; set; }
            public playerInfo[] Players { get; set; }

            public string GetName(int id)
            {
                foreach (var player in LegendsPlayers)
                {
                    if (player.id == id)
                        return player.l;
                }

                foreach (var player in Players)
                {
                    if (player.id == id)
                        return player.l;
                }

                return "";
            }

            public List<playerInfo> GetIdByNameAndRating(string name,int rating)
            {
                List<playerInfo> ls = new List<playerInfo>();
                name = name.ToLower();
                foreach(var player in LegendsPlayers)
                {
                    if(player.r==rating||rating==0)
                    {
                        if (player.f.ToLower().Contains(name) || player.l.ToLower().Contains(name))
                        {
                            ls.Add(player);
                        }
                    }
                    
                }
                foreach (var player in Players)
                {
                    if (player.r == rating||rating==0)
                    {
                        if (player.f.ToLower().Contains(name) || player.l.ToLower().Contains(name))
                        {
                            ls.Add(player);
                        }
                    }

                }
                return ls;
            }

        }
        public class playerInfo
        {
            public string f { get; set; }
            public int id { get; set; }
            public string l { get; set; }
            public int r { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rating = 0;
            int.TryParse(raingBox.Text, out rating);
            List<playerInfo> players = playersInfo.GetIdByNameAndRating(nameBox.Text, rating);
            if(players.Count>0)
            {
                listView.Items.Clear();
                foreach (var player in players)
                {
                    HttpClient client = new HttpClient();
                    Response data = client.GetData("https://www.futbin.com/21/playerPrices?player=" + player.id);
                    FutbinPrice futbinPrice = JsonConvert.DeserializeObject<FutbinPrice>(data.Html.Match("\"ps\":({.*?})"));
                    //添加行
                    var item = new ListViewItem();
                    //item.ImageIndex = 1;
                    item.Text = player.id.ToString();
                    item.SubItems.Add(player.f + " " + player.l);
                    item.SubItems.Add(player.r.ToString());
                    item.SubItems.Add(futbinPrice.MinPrice);
                    item.SubItems.Add(futbinPrice.MaxPrice);
                    item.SubItems.Add(futbinPrice.LCPrice);
                    listView.BeginUpdate();
                    listView.Items.Add(item);
                    listView.Items[listView.Items.Count - 1].EnsureVisible();//滚动到最后
                    listView.EndUpdate();


                }
                idBox.Text = listView.Items[0].Text;
            }
         

           
            
            
           
        }

        private void listView_Click(object sender, EventArgs e)
        {
            idBox.Text = listView.SelectedItems[0].Text;
        }

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    public class FutbinPrice
    {
        public string LCPrice { get; set; }

        public string LCPrice2 { get; set; }

        public string LCPrice3 { get; set; }

        public string LCPrice4 { get; set; }

        public string LCPrice5 { get; set; }

        public string updated { get; set; }

        public string MinPrice { get; set; }

        public string MaxPrice { get; set; }

        public string PRP { get; set; }
    }
}
