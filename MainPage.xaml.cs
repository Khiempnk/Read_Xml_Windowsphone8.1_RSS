using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AppReadDataWebBarca
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<ItemsData> lstItemsData = null;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

    
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
           // ReadDataWeb("http://www.fcbarcelona.com/football/list/articles.rss");
          //  PareXmlData();
            ProgressLoad.Visibility = Windows.UI.Xaml.Visibility.Visible;
            lstItemsData = new List<ItemsData>();
            GetPareXmlData();
            HardwareButtons.BackPressed +=HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
           foreach(var chilren in gridwebviewLoadlink.Children )
           {
               if(chilren.Visibility == Windows.UI.Xaml.Visibility.Visible)
               {
                   gridwebviewLoadlink.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                   ProgressLoad.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

               }
           }
           e.Handled = true;
        }
        public async void ReadDataWeb(string url)
        {
            Uri uri = new Uri(url);
            HttpClient httpclient = new HttpClient();
            var data = await httpclient.GetAsync(uri, System.Threading.CancellationToken.None);
            var stringdata = await data.Content.ReadAsStringAsync();
          //  PareXmlData(stringdata);
        }

        public async void GetPareXmlData()
        {

            XmlDocument xmldocument = await XmlDocument.LoadFromUriAsync(new Uri("http://www.fcbarcelona.com/football/list/articles.rss"));
            XmlNodeList nodes = xmldocument.SelectNodes("//rss/channel/item");
            foreach(var item in nodes)
            {
                var title = item.SelectSingleNode("title").InnerText.Replace("\n","");
                var imageurl = item.SelectSingleNode("enclosure").Attributes.GetNamedItem("url").InnerText;
                var link = item.SelectSingleNode("link").InnerText;
                var des = item.SelectSingleNode("description").InnerText;
                des = des.Remove(0, des.IndexOf("<br />")).Replace("<br />","");
                des = des.Remove(des.IndexOf("<br/>")).Replace("\n","");
                var date = item.SelectSingleNode("pubDate").InnerText.Trim();
                lstItemsData.Add(new ItemsData() { Date = date, Description = des, ImageUrl = imageurl, Link = link, Title = title });
            }
            lstData.DataContext = lstItemsData;
            ProgressLoad.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        public async void PareXmlData()
        {

            XmlDocument xmldocument = await XmlDocument.LoadFromUriAsync(new Uri("http://www.fcbarcelona.com/football/list/articles.rss"));
          //  var data = xmldocument.GetXml();
          //  XmlNamespaceManager  xmlname = new XmlNamespaceManager()
     //       string kho = @"<widgets><widget><url>~/Portal/Widgets/ServicesList.ascx</url><castAs>ServicesWidget</castAs><urlType>ascx</urlType><parameters> <PortalCategoryId>3</PortalCategoryId></parameters> </widget><widget><url>www.omegacoder.com</url><castAs>ServicesWidget</castAs><urlType>htm</urlType><parameters><PortalCategoryId>41</PortalCategoryId></parameters></widget></widgets>";
        
            XmlNodeList nodes = xmldocument.SelectNodes("//rss/channel/item");
            foreach (var items in nodes)
           {

               var title = items.SelectSingleNode("title").InnerText;
          //      title = Regex.Replace(title,"")
               var description = items.SelectSingleNode("description").InnerText;
           //    XDocument xdcument = XDocument.Parse(description);

           //    var abcdef = from a in xdcument.Descendants("description") select new { A = a.Descendants("a").First().Value, B = a.Descendants("img") };
            
               //var widgets = from x in xdcument.Descendants("widget")
               //              select new
               //              {
               //                  URL = x.Descendants("url").First().Value,
               //                  Category = x.Descendants("PortalCategoryId").First().Value
               //              };

            //   var ko = from a in xdcument.Descendants("description")
                
               //var hjh = description.GetNamedItem("a").NodeValue;

               //XDocument x = new XDocument();
               //x = XDocument.Parse(description);
               //var data = (from a in x.Elements("description") select new DataItems { strong = (string)a.Element("strong") }).ToList();
               
             //  var khk = _link;
               //foreach (var wd in widgets)
               //{
               //    var ko = wd.URL;
               //}

               var image = items.SelectSingleNode("enclosure").Attributes.GetNamedItem("url").NodeValue;
           }

        }

        private void lstData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var link = (lstData.SelectedItem as ItemsData).Link;
            webviewLoadlink.Navigate(new Uri(link));
            webviewLoadlink.FrameNavigationCompleted += webviewLoadlink_FrameNavigationCompleted;
            gridwebviewLoadlink.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ProgressLoad.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        void webviewLoadlink_FrameNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            ProgressLoad.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }

      
    }
}
