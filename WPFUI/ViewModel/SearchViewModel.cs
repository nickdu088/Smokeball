using System.ComponentModel;
using System.Dynamic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using WPFUI.Command;

namespace WPFUI.ViewModel
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly string WEB_API_END_POINT = "http://localhost:5000/search/{0}/{1}";
        private readonly HttpClient _httpClient;

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new SearchCommand(()=>DoSearch()));
            }
            set { _searchCommand = value; }
        }

        private string keywords;
        private string url;
        private string result;
        public string Keywords
        {
            get
            {
                return keywords;
            }
            set
            {
                keywords = value;
                OnPropertyChanged("Keywords");
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }

        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        #region INotifyPropertyChanged Members  
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


        public SearchViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void DoSearch()
        {
            if (!string.IsNullOrEmpty(Keywords) && !string.IsNullOrEmpty(Url))
            {
                string friendlyKeywords = HttpUtility.JavaScriptStringEncode(Keywords);
                string friendlyUrl = HttpUtility.JavaScriptStringEncode(Url);
                var downloadHtmlTask = Task.Run(async () => await _httpClient.GetAsync(string.Format(WEB_API_END_POINT, friendlyKeywords, friendlyUrl)));
                HttpResponseMessage result = downloadHtmlTask.Result;
                var getResultTask = Task.Run(async () => await result.Content.ReadAsStringAsync());
                string jsonString = getResultTask.Result;
                dynamic obj = JsonSerializer.Deserialize<ExpandoObject>(jsonString);

                Result = obj.result.ToString();
            }
            else
            {
                Result = "keywords or url cannot be empty!";
            }
        }

    }
}
