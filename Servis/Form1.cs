using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using NUnit.Framework;

namespace Servis
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        public OracleConnection con = new OracleConnection();

        public OracleCommand sql = new OracleCommand();
        public OracleDataAdapter da = new OracleDataAdapter();
        public DataSet dt = new DataSet();
        public DataTable dt_doktor2 = new DataTable();

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            

        }
        
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string urun_adi, urun_kodu, urun_aciklama, resim_kodu, etiket;
                string fiyat;

                string link = textEdit1.Text;//Web Site Link
                Uri url = new Uri(link);
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string html = client.DownloadString(url);

                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(html);

                //Urun Adı
                int basla1 = dokuman.ParsedText.IndexOf(@"<table class=""table table-bordered customTable"">");
                int bit1 = dokuman.ParsedText.IndexOf(@"<div class=""modal fade"" id=""sil""  role=""dialog"" aria-labelledby=""myModalLabel"">");
                int uzunluk1 = dokuman.ParsedText.Length;
                string script = dokuman.ParsedText.Substring(basla1, bit1 - basla1);



                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(script);
                //doc.LoadHtml(@"
                //<table class=""table table-bordered customTable"">
                //    <thead>
                //      <tr>
                //        <th>Eczane Adı</th>
                //        <th>Bilgiler</th>
                //      </tr>
                //    </thead>
                //    <tbody>
                //	<tr><td colspan=""12"" class=""ilce-baslik"">ŞAHİNBEY NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=1""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> MERKEZ ECZANESİ		</td>
                //        <td>Eyüpoğlu Mh.Değirmen Sk.10/A (Dr.Ersin Arslan Dev. Hast.Acil Sokağı-Ağız Diş Merkezi Yanı) / <b>ŞAHİNBEY</b><br />Telefon : 0342 230 56 56</td>
                //      </tr>
                //	        <tr>
                //        <td> ISTIKLAL ENES ECZANESI		</td>
                //        <td>İstiklal Mah. 97 nolu Sok. No:34 (Kafadar Sağ.Ocağı Karşısı) / <b>ŞAHİNBEY</b><br />Telefon : 03423395700</td>
                //      </tr>
                //	        <tr>
                //        <td> DÜZTEPE CAN ECZANESI		</td>
                //        <td>NURİPAZARBAŞI MAHALLESİ ÖZDEMİRBEY CADDESİ 14:A/D / ŞAHİNBEY (YAŞAM HASTANESİ KARŞISI) / <b>ŞAHİNBEY</b><br />Telefon : 03422516851</td>
                //      </tr>
                //	        <tr>
                //        <td> ASAY ECZANESİ		</td>
                //        <td>Kemal Köker Cad.No:49/B(Hatem Hastanesi Yanı-Ulu Camii Karşısı) / <b>ŞAHİNBEY</b><br />Telefon : 03423389585</td>
                //      </tr>
                //	        <tr>
                //        <td> RÜZGAR ECZANESİ		</td>
                //        <td>Binevler Mahallesi 81067 Nolu Sokak No:16/A( Eski Ezogelin Emlak Arkası ) / <b>ŞAHİNBEY</b><br />Telefon : 0342 360 05 50</td>
                //      </tr>
                //	        <tr>
                //        <td> YENI UMUT ECZANESI		</td>
                //        <td>Karataş 1 Bölge (Winmar Karşısı-Barbaros Ersaatçi Camii Arkası) / <b>ŞAHİNBEY</b><br />Telefon : 0342 371 60 90</td>
                //      </tr>
                //	        <tr>
                //        <td> SÖNMEZ ECZANESİ 		</td>
                //        <td>Yeditepe mahallesi 85108 nolu sokak sevgi apartmanı altı no:15/B (Özgecan Hanımlar Yüzme Havuzu Arkası, Fatih Karakuş Engelsizler parkı yanı) / ŞAHİNBEY / <b>ŞAHİNBEY</b><br />Telefon : 0342 360 00 66</td>
                //      </tr>
                //	  <tr><td colspan=""12"" class=""ilce-baslik"">ŞEHİTKAMİL NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=2""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> MÜGE ECZANESİ		</td>
                //        <td>Medical Park Hastanesi Acil Karşısı / <b>ŞEHİTKAMİL</b><br />Telefon : 0342 324 36 16</td>
                //      </tr>
                //	        <tr>
                //        <td> BARAN ECZANESI		</td>
                //        <td>Emek Mah. Cemil Alevli Cad. No:17/A (Cahit Nakıpoğlu İ.Ö.O. Yanı) / <b>ŞEHİTKAMİL</b><br />Telefon : 0342 2309411</td>
                //      </tr>
                //	        <tr>
                //        <td> EDA ECZANESİ		</td>
                //        <td>29 Ekim mah. 81053 nolu sok . No:30/c   ( 125.yıl ilkokulu ve 29 Ekim AŞ’m karşısı ) / <b>ŞEHİTKAMİL</b><br />Telefon : 0 342 225 72 72</td>
                //      </tr>
                //	        <tr>
                //        <td> ÖMER ARSLAN ECZANESİ		</td>
                //        <td>SEYRANTEPE MAHALLESİ 65112 NOLU SOKAK NO:1/A (ÖZEL LİV HOSPİTAL YANI  ) / <b>ŞEHİTKAMİL</b><br />Telefon : 0342 408 91 45</td>
                //      </tr>
                //	        <tr>
                //        <td> ARDEN ECZANESİ		</td>
                //        <td>PİR SULTAN MAHALLESİ 59088 NOLU SOKAK NO:4/B (ŞEHİTKAMİL DEVLET HASTANESİ YANI) / <b>ŞEHİTKAMİL</b><br />Telefon : 0342 323 39 23</td>
                //      </tr>
                //	  <tr><td colspan=""12"" class=""ilce-baslik"">NİZİP NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=7""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> FATIH ECZANESI		</td>
                //        <td>HASTANE CAD. NO:40 NİZİP / <b>NİZİP</b><br />Telefon : 0 342 5121431</td>
                //      </tr>
                //	  <tr><td colspan=""12"" class=""ilce-baslik"">OĞUZELİ NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=10""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> KARADAĞ ECZANESİ		</td>
                //        <td>OĞUZLAR MAHALLESİ KARPUZATAN CADDESİ NO:11/A (OĞUZELİ İLÇE DEVLET HASTANESİ KARŞISI)  Oğuzeli/G.ANTEP / <b>OĞUZELİ</b><br />Telefon : 0342 571 21 00</td>
                //      </tr>
                //	  <tr><td colspan=""12"" class=""ilce-baslik"">İSLAHİYE NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=3""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> BULUT ECZANESI		</td>
                //        <td>CUMHURİYET MAHALLESİ HÜKÜMET CADDESİ NO: 10 /C İSLAHİYE (KANAAT MARKET YANI ) / <b>İSLAHİYE</b><br />Telefon : 0342 863 20 45</td>
                //      </tr>
                //	  <tr><td colspan=""12"" class=""ilce-baslik"">ARABAN NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=5""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> MURAT ECZANESİ		</td>
                //        <td>TURGUT ÖZAL MAHALLESİ CEZAEVİ CADDESİ NO:39/E / <b>ARABAN</b><br />Telefon : 0342 611 22 43</td>
                //      </tr>
                //	  <tr><td colspan=""12"" class=""ilce-baslik"">NURDAĞI NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=6""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> ÜNSALAN ÖMÜR		</td>
                //        <td>ATATÜRK MAH.3030 NOLU SOKAK NO:3/ZI / <b>NURDAĞI</b><br />Telefon : 0342 671 27 50</td>
                //      </tr>
                //	  <tr><td colspan=""12"" class=""ilce-baslik"">KİLİS MERKEZ NÖBETÇİ ECZANELER <small class=""pull-right""><a style=""color:#fff; text-decoration: none"" href=""https://www.gaziantepeo.org.tr/nobet-karti/filtrele?tarih=01.03.2021&bolge=20""><i class=""fa fa-print""></i> YAZDIR</a></small></td></tr>      <tr>
                //        <td> ARZU ECZAESİ (KİLİS)		</td>
                //        <td>Kazım Karabekir Mahallesi Funda Sokak No:15/A-1  / <b>KİLİS MERKEZ</b><br />Telefon : 0348 814 22 50 </td>
                //      </tr>
                //	      </tbody>
                //  </table>
                //");




                

                // Using LINQ to parse HTML table smartly 
                var HTMLTableTRList = from table in doc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                                      from row in table.SelectNodes("//tr").Cast<HtmlNode>()
                                      from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
                                      
                                      select new { Table_Name = table.Id, Cell_Text = cell.InnerText };

                // now showing output of parsed HTML table
                int ilce = 0;
                string[] sahinbey = new string[20]; //ilce1
                string[] sehitkamil = new string[20];//ilce2
                string[] nizip = new string[20];//ilce3
                string[] oguzeli = new string[20];//ilce4
                string[] islahiye = new string[20];//ilce5

                string[] araban = new string[20];//ilce6
                string[] nurdag = new string[20];//ilce7
                string[] kilis = new string[20];//ilce8


                DataSet ds = new DataSet();

                DataTable dt = new DataTable("eczane");
                dt.Columns.Add(new DataColumn("ilce", typeof(string)));
                dt.Columns.Add(new DataColumn("adi", typeof(string)));
                dt.Columns.Add(new DataColumn("adres", typeof(string)));
                dt.Columns.Add(new DataColumn("telefon", typeof(string)));




                int kayit;
                kayit = 0;
                string adi, adresi, telefon;
                adi = "";
                adresi = "";
                telefon = "";
                foreach (var cell in HTMLTableTRList)
                {
                    DataRow dr = dt.NewRow();
                    listBox1.Items.Add((cell.Cell_Text));
                    if (ilce == 1)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        { 
                            adi = cell.Cell_Text;
                        }
                        if (kayit==2)
                        {
                            adresi = cell.Cell_Text.Substring(0,cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk= cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :")+9,cell.Cell_Text.Length-9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit==3)
                        {
                            dr["ilce"] = "ŞahinBey";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));
                           
                            kayit = 0;
                        }

                       
                    }



                    if (ilce == 2)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        {
                            adi = cell.Cell_Text;
                        }
                        if (kayit == 2)
                        {
                            adresi = cell.Cell_Text.Substring(0, cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk = cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :") + 9, cell.Cell_Text.Length - 9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit == 3)
                        {
                            dr["ilce"] = "ŞehitKamil";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));

                            kayit = 0;
                        }


                    }

                    if (ilce == 3)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        {
                            adi = cell.Cell_Text;
                        }
                        if (kayit == 2)
                        {
                            adresi = cell.Cell_Text.Substring(0, cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk = cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :") + 9, cell.Cell_Text.Length - 9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit == 3)
                        {
                            dr["ilce"] = "Nizip";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));

                            kayit = 0;
                        }


                    }


                    if (ilce == 4)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        {
                            adi = cell.Cell_Text;
                        }
                        if (kayit == 2)
                        {
                            adresi = cell.Cell_Text.Substring(0, cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk = cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :") + 9, cell.Cell_Text.Length - 9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit == 3)
                        {
                            dr["ilce"] = "Islahiye";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));

                            kayit = 0;
                        }


                    }

                    if (ilce == 5)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        {
                            adi = cell.Cell_Text;
                        }
                        if (kayit == 2)
                        {
                            adresi = cell.Cell_Text.Substring(0, cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk = cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :") + 9, cell.Cell_Text.Length - 9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit == 3)
                        {
                            dr["ilce"] = "Nurdagi";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));

                            kayit = 0;
                        }


                    }
                    if (ilce == 6)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        {
                            adi = cell.Cell_Text;
                        }
                        if (kayit == 2)
                        {
                            adresi = cell.Cell_Text.Substring(0, cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk = cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :") + 9, cell.Cell_Text.Length - 9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit == 3)
                        {
                            dr["ilce"] = "araban";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));

                            kayit = 0;
                        }


                    }
                    if (ilce == 7)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        {
                            adi = cell.Cell_Text;
                        }
                        if (kayit == 2)
                        {
                            adresi = cell.Cell_Text.Substring(0, cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk = cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :") + 9, cell.Cell_Text.Length - 9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit == 3)
                        {
                            dr["ilce"] = "kilis";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));

                            kayit = 0;
                        }


                    }
                    if (ilce == 8)
                    {
                        kayit = kayit + 1;
                        if (kayit == 1)
                        {
                            adi = cell.Cell_Text;
                        }
                        if (kayit == 2)
                        {
                            adresi = cell.Cell_Text.Substring(0, cell.Cell_Text.IndexOf("Telefon :"));
                            int uzunluk = cell.Cell_Text.IndexOf("Telefon :");
                            telefon = cell.Cell_Text.Substring(cell.Cell_Text.IndexOf("Telefon :") + 9, cell.Cell_Text.Length - 9 - cell.Cell_Text.IndexOf("Telefon :"));
                            kayit = kayit + 1;
                        }
                        if (kayit == 3)
                        {
                            dr["ilce"] = "oguzeli";
                            dr["adi"] = adi;
                            dr["adres"] = adresi;
                            dr["telefon"] = telefon;
                            dt.Rows.Add(dr);
                            listBox2.Items.Add((cell.Cell_Text));

                            kayit = 0;
                        }


                    }





                    if (cell.Cell_Text== "ŞAHİNBEY NÖBETÇİ ECZANELER  YAZDIR") { ilce = 1;}
                    if (cell.Cell_Text == "ŞEHİTKAMİL NÖBETÇİ ECZANELER  YAZDIR") { ilce = 2; kayit = 0; }
                    if (cell.Cell_Text == "NİZİP NÖBETÇİ ECZANELER  YAZDIR") { ilce = 3; kayit = 0; }

                    if (cell.Cell_Text == "İSLAHİYE NÖBETÇİ ECZANELER  YAZDIR") { ilce = 4; kayit = 0; }
                    if (cell.Cell_Text == "NURDAĞI NÖBETÇİ ECZANELER  YAZDIR") { ilce = 5; kayit = 0; }
                    if (cell.Cell_Text == "ARABAN NÖBETÇİ ECZANELER  YAZDIR") { ilce = 6; kayit = 0; }
                    if (cell.Cell_Text == "KİLİS MERKEZ NÖBETÇİ ECZANELER  YAZDIR") { ilce = 7; kayit = 0; }
                    if (cell.Cell_Text == "OĞUZELİ NÖBETÇİ ECZANELER  YAZDIR") { ilce = 8; kayit = 0; }




                    


                }
                ds.Tables.Add(dt);
                MessageBox.Show("Veriler Kayıt Edildi");
                try
                {
                    
                    con.ConnectionString = "User Id=OnlineUser;Password=PwdMedPass1;Data Source=orcl";
                    con.Open();
                    foreach (var item in dt)
                    {

                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata:" + hata.Message);
                }
            }
            catch (Exception hata)
            {

                MessageBox.Show(hata.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {





        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string urun_adi, urun_kodu, urun_aciklama, resim_kodu, etiket;
                string fiyat;

                string link = textEdit1.Text;//Web Site Link
                Uri url = new Uri(link);
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string html = client.DownloadString(url);

                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(html);

                //Urun Adı
                int basla = dokuman.ParsedText.IndexOf(@"<table class=""table table-bordered customTable"">");
                int bit = dokuman.ParsedText.IndexOf(@"<div class=""modal fade"" id=""sil""  role=""dialog"" aria-labelledby=""myModalLabel"">");
                int uzunluk = dokuman.ParsedText.Length;
                string script = dokuman.ParsedText.Substring(basla, bit-basla);
                

                //HtmlNodeCollection firma = dokuman.DocumentNode.SelectNodes("//table[@class='table table-bordered customTable']");

                //foreach (var baslik in firma) { listBox1.Items.Add(baslik.InnerText); }

                //HtmlNodeCollection adres = dokuman.DocumentNode.SelectNodes("//div[@class='col-lg-10 col-md-10 col-sm-10 col-xs-10 nopaddingl pull-left kartEczaneBilgi']");
                //foreach (var adresx in adres) { listBox1.Items.Add(adresx.InnerText); }
            }
            catch (Exception hata)
            {

                MessageBox.Show(hata.Message);
            }
        }
       

        private void Form1_Load(object sender, EventArgs e)
        {
            
            string tarih = DateTime.Now.ToShortDateString();
            textEdit1.Text = "https://www.gaziantepeo.org.tr/nobetci-eczaneler/filtrele?tarih=" + tarih + "&bolge=all";


        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        int kontrol;
        private void timer1_Tick(object sender, EventArgs e)
        {
            kontrol = kontrol + 1;
            if (kontrol==5)
            {
                simpleButton2.PerformClick();
            }
        }
    }
}
