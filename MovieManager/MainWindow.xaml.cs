using Microsoft.Win32;
using MovieManager.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace MovieManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Movie> MoviesList = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> FilteredMoviesList = new ObservableCollection<Movie>();
        
        public ObservableCollection<string> Ratings
        {
            get; set;
        }

            public MainWindow()
        {
            InitializeComponent();
            DataContext = new NameValidator();
            

            MoviesList.Add(new Movie("Title1", "Directory1", Rating.Terrible, Type.Thriller));
            MoviesList.Add(new Movie("Title1", "Directory1", Rating.Terrible, Type.Thriller));
            MoviesList.Add(new Movie("Title1", "Directory1", Rating.Terrible, Type.Thriller));
            MoviesList.Add(new Movie("Title1", "Directory1", Rating.Terrible, Type.Thriller));

            MoviesEdit.ItemsSource = MoviesList;
            Movies.ItemsSource = MoviesList;
            FilteredMovies.ItemsSource = FilteredMoviesList;
        }



        public enum Rating
        {
            Terrible,
            Bad,
            Ok,
            Good,
            Awesome
        }

        public enum Type
        {
            Thriller,
            Comedy,
            Drama,
            Horror
        }

        public class Movie
        {
            public string Title { get; set; }
            public string Director { get; set; }
            public Rating Rating { get; set; }
            public Type Type { get; set; }

            public Movie()
            {

            }

            public Movie(string title, string director, Rating rating, Type type)
            {
                Title = title;
                Director = director;
                Rating = rating;
                Type = type;
            }

        }
        
        [System.Xml.Serialization.XmlRoot("ArrayOfMovie")]
        public class MovieCollection
        {
            [XmlElement("Movie")]
            public List<Movie> Movies { get; set; }
        }

        private void MenuItem_Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help", "Help", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void MenuItem_AddMovie(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                MoviesList.Add(new Movie($"Title {i}", $"Directory {i}", Rating.Terrible, Type.Thriller));
            }
            
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_ClearMovies(object sender, RoutedEventArgs e)
        {
            MoviesList.Clear();
        }

        private void MenuItem_ImportMovies(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml";

            
            Nullable<bool> result = dlg.ShowDialog();

            
            if (result == true)
            {
                string filename = dlg.FileName;
                var deserializer = new XMLSerializer<MovieCollection>(filename);

                MovieCollection movieCollection = null;

                try
                {
                    movieCollection = deserializer.Deserialize();
                }
                catch (InvalidOperationException ex)
                {
                    return;
                }

                if (movieCollection != null)
                {
                    var movies = movieCollection.Movies;
                    if (movies != null)
                    {
                        foreach (var movie in movies)
                        {
                            MoviesList.Add(movie);
                        }
                    }
                }
            }
        }

        private void MenuItem_ExportMovies(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                var serializer = new XMLSerializer<MovieCollection>(filename);

                var movieCollection = new MovieCollection();
                movieCollection.Movies = MoviesList.ToList();

                try
                {
                    serializer.Serialize(movieCollection);
                }
                catch (InvalidOperationException)
                {
                    return;
                }
            }
        }

        private void Button_Click_FindMovies(object sender, RoutedEventArgs e)
        {
            var filteredMovies = new List<Movie>();

            if (TitleCheckBox.IsChecked.HasValue)
            {
                if (TitleCheckBox.IsChecked.Value)
                {
                    filteredMovies = MoviesList.Where(x => x.Title.Contains(TitleTextBox.Text)).ToList();
                }
            }

            if (AuthorCheckBox.IsChecked.HasValue)
            {
                if (AuthorCheckBox.IsChecked.Value)
                {
                    if (!filteredMovies.Any())
                        filteredMovies = MoviesList.Where(x => x.Director.Contains(AuthorTextBox.Text)).ToList();
                    else
                        filteredMovies = filteredMovies.Where(x => x.Director.Contains(AuthorTextBox.Text)).ToList();
                }
            }

            if (RatingCheckBox.IsChecked.HasValue)
            {
                if (RatingCheckBox.IsChecked.Value)
                {
                    if (!filteredMovies.Any())
                        filteredMovies = MoviesList.Where(x => (int)x.Rating == RatingComboBox.SelectedIndex).ToList();
                    else
                        filteredMovies = filteredMovies.Where(x => (int)x.Rating == RatingComboBox.SelectedIndex).ToList();
                }
            }

            if (TypeCheckBox.IsChecked.HasValue)
            {
                if (TypeCheckBox.IsChecked.Value)
                {
                    if (!filteredMovies.Any())
                        filteredMovies = MoviesList.Where(x => (int)x.Type == TypeComboBox.SelectedIndex).ToList();
                    else
                        filteredMovies = filteredMovies.Where(x => (int)x.Type == TypeComboBox.SelectedIndex).ToList();
                }
            }

            FilteredMoviesList.Clear();
            foreach (var movie in filteredMovies)
            {
                FilteredMoviesList.Add(movie);
            }
        }

        private void Button_Click_DeleteMovies(object sender, RoutedEventArgs e)
        {
            var filteredMovies = new List<Movie>();

            if (TitleCheckBox.IsChecked.HasValue)
            {
                if (TitleCheckBox.IsChecked.Value)
                {
                    filteredMovies = MoviesList.Where(x => x.Title.Contains(TitleTextBox.Text)).ToList();
                }
            }

            if (AuthorCheckBox.IsChecked.HasValue)
            {
                if (AuthorCheckBox.IsChecked.Value)
                {
                    filteredMovies = filteredMovies.Where(x => x.Director.Contains(AuthorTextBox.Text)).ToList();
                }
            }

            if (RatingCheckBox.IsChecked.HasValue)
            {
                if (RatingCheckBox.IsChecked.Value)
                {
                    filteredMovies = filteredMovies.Where(x => (int)x.Rating == RatingComboBox.SelectedIndex).ToList();
                }
            }

            if (TypeCheckBox.IsChecked.HasValue)
            {
                if (TypeCheckBox.IsChecked.Value)
                {
                    filteredMovies = filteredMovies.Where(x => (int)x.Type == TypeComboBox.SelectedIndex).ToList();
                }
            }

            foreach (var item in filteredMovies)
            {
                MoviesList.Remove(item);
            }
        }
    }
}
