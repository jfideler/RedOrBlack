using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641
namespace RedOrBlack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CardDeck _deck;
        private CardDeck.Stages _stage;
        private enum Choices
        {
            Left = 1,
            Right = 2
        }
        private const string _imageUrl = "ms-appx:///Assets/images/{0}.png";
       
        public MainPage()
        {
			this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

			btnPrev.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			btnNext.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            _deck = new CardDeck();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            tbAppHeadline.Text = "dare to shuffle...";
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            QuitTheApp();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {

            EvaluateChoice(Choices.Left);
            NextStage();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            EvaluateChoice(Choices.Right);
            NextStage();
        }

        private bool IsOver()
        {
            List<int> overs = new List<int>();
            overs.Add(12);
            overs.Add(1);
            overs.Add(6);
            overs.Add(7);
            overs.Add(8);
            overs.Add(9);
            overs.Add(10);

            //3/5 or 7/9 
            return overs.Contains(_deck.SelectedCard.Value);

        }

        private void EvaluateChoice(Choices choice)
        {
            bool shouldLeave;

            switch (choice)
            {
                case Choices.Left:

                    shouldLeave = (_deck.Stage == CardDeck.Stages.Color && _deck.SelectedCard.IsBlack)
                        || (_deck.Stage == CardDeck.Stages.Suit && (_deck.SelectedCard.Suit == CardDeck.Suits.Clubs || _deck.SelectedCard.Suit == CardDeck.Suits.Diamonds))
                        || (_deck.Stage == CardDeck.Stages.Face && !_deck.SelectedCard.IsFace)
                        || (_deck.Stage == CardDeck.Stages.ChickOrDude && !_deck.SelectedCard.IsMale)
                        || (_deck.Stage == CardDeck.Stages.OddOrEven && !_deck.SelectedCard.IsOdd)
                        || (_deck.Stage == CardDeck.Stages.OverUnder && this.IsOver());

                    if (shouldLeave)
                    {
                        _deck.Stage = CardDeck.Stages.GameOver;
                    }

                    break;


                case Choices.Right:

                    shouldLeave = (_deck.Stage == CardDeck.Stages.Color && !_deck.SelectedCard.IsBlack)
                        || (_deck.Stage == CardDeck.Stages.Suit && (_deck.SelectedCard.Suit == CardDeck.Suits.Spades || _deck.SelectedCard.Suit == CardDeck.Suits.Hearts))
                        || (_deck.Stage == CardDeck.Stages.Face && _deck.SelectedCard.IsFace)
                        || (_deck.Stage == CardDeck.Stages.ChickOrDude && _deck.SelectedCard.IsMale)
                        || (_deck.Stage == CardDeck.Stages.OddOrEven && _deck.SelectedCard.IsOdd)
                        || (_deck.Stage == CardDeck.Stages.OverUnder && !this.IsOver());

                    if (shouldLeave)
                    {
                        _deck.Stage = CardDeck.Stages.GameOver;
                    }
                    break;


            }

        }

        private void NextStage()
        {

            switch (_deck.Stage)
            {
                case CardDeck.Stages.Start:
                    _deck.Stage = CardDeck.Stages.Color;
                    tbAppHeadline.Text = "Red or Black?";

                    btnPrev.Content = "Red...";
                    btnNext.Content = "Black...";

                    break;

                case CardDeck.Stages.Color:

                    lstHints.Items.Add((_deck.SelectedCard.IsBlack ? "Black" : "Red") + "...  So far so good! ");
                    _deck.Stage = CardDeck.Stages.Suit;
                    if (_deck.SelectedCard.IsBlack)
                    {
                        OptionsBorder.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                        tbAppHeadline.Text = "Spade or Club?";
                        btnPrev.Content = "Spade...";
                        btnNext.Content = "Club...";
                    }
                    else
                    {
                        OptionsBorder.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 46, 18));
                        tbAppHeadline.Text = "Heart or Diamond?";
                        btnPrev.Content = "Heart...";
                        btnNext.Content = "Diamond...";
                    }
                    break;

                case CardDeck.Stages.Suit:

                    imgSuit.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(String.Format(_imageUrl, _deck.SelectedCard.Suit.ToString())));
                    lstHints.Items.Add(_deck.SelectedCard.Suit + "  Way to go! ");
                    tbAppHeadline.Text = "Face? (yes or no)";
                    _deck.Stage = CardDeck.Stages.Face;
                    btnPrev.Content = "Yes...";
                    btnNext.Content = "No...";
                    break;

                case CardDeck.Stages.Face:
                    lstHints.Items.Add("Face card? " + _deck.SelectedCard.IsFace + "  (Whew!)");
                    if (_deck.SelectedCard.IsFace)
                    {
                        _deck.Stage = CardDeck.Stages.ChickOrDude;
                        tbAppHeadline.Text = "Dude or Chick?";
                        btnPrev.Content = "Dude...";
                        btnNext.Content = "Chick...";
                    }
                    else
                    {
                        _deck.Stage = CardDeck.Stages.OddOrEven;
                        tbAppHeadline.Text = "Odd or Even?";
                        btnPrev.Content = "Odd...";
                        btnNext.Content = "Even...";
                    }
                    break;

                case CardDeck.Stages.OddOrEven:
                    lstHints.Items.Add((_deck.SelectedCard.IsOdd ? "An odd one" : "Even keel now..."));
                    _deck.Stage = CardDeck.Stages.OverUnder;
                    if (_deck.SelectedCard.IsOdd)
                    {
                        tbAppHeadline.Text = "3/5 or 7/9";
                        btnPrev.Content = "3/5...";
                        btnNext.Content = "7/9...";
                    }
                    else
                    {
                        tbAppHeadline.Text = "2/4 or 6/8/10";
                        btnPrev.Content = "2/4...";
                        btnNext.Content = "6/8/10...";
                    }

                    break;

                case CardDeck.Stages.ChickOrDude:
                    lstHints.Items.Add((_deck.SelectedCard.IsMale ? "A dude" : "A chick") + "...  who knew? ");
                    if (_deck.SelectedCard.IsFace)
                    {
                        _deck.Stage = CardDeck.Stages.OverUnder;
                        tbAppHeadline.Text = "Jack/King or Queen/Ace?";
                        btnPrev.Content = "J/K?";
                        btnNext.Content = "Q/A?";
                    }


                    break;

                case CardDeck.Stages.OverUnder:
                    lstHints.Items.Add((this.IsOver() ? "Over the hump..." : "Under the wire") + "!!");
                    tbAppName.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    tbAppHeadline.Text = "how close where you?";
                    _deck.Stage = CardDeck.Stages.Assertion;
                    btnPrev.Content = "?";
                    btnNext.Content = "?";
                    RevealCard(false);

                    break;

                case CardDeck.Stages.Assertion:
                    _deck.Stage = CardDeck.Stages.GameOver;
                    Assertion();
                    break;

                case CardDeck.Stages.GameOver:
                    tbAppName.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    tbAppHeadline.Text = "Sucks to be wrong...";
                    RevealCard(false);
                    break;

                default:
                    tbAppHeadline.Text = "dare to shuffle!";
                    break;
            }


        }

        private void Assertion()
        {
            tbAppHeadline.Text = _deck.SelectedCard.DescriptionLong;


            if (_deck.SelectedCard.IsFace)
            {
                if (_deck.SelectedCard.IsMale)
                {
                    if (_deck.SelectedCard.IsBlack)
                    {
                        if (_deck.SelectedCard.Suit == CardDeck.Suits.Spades)
                        {
                            tbAppHeadline.Text = "Jack or King of Spades?";
                        }
                        else
                        {
                            tbAppHeadline.Text = "Jack or King of Clubs?";
                        }
                    }
                    else //red face male
                    {
                        if (_deck.SelectedCard.Suit == CardDeck.Suits.Hearts)
                        {
                            tbAppHeadline.Text = "Jack or King of Hearts?";
                        }
                        else
                        {
                            tbAppHeadline.Text = "Jack or King of Diamonds?";
                        }
                    }
                }
                else
                    if (_deck.SelectedCard.IsBlack)
                    {
                        if (_deck.SelectedCard.Suit == CardDeck.Suits.Spades)
                        {
                            tbAppHeadline.Text = "Queen or Ace of Spades?";
                        }
                        else
                        {
                            tbAppHeadline.Text = "Queen or Ace of of Clubs?";
                        }
                    }
                    else //red face female
                    {
                        if (_deck.SelectedCard.Suit == CardDeck.Suits.Hearts)
                        {
                           tbAppHeadline.Text = "Queen or Ace of Hearts?";
                        }
                        else
                        {
                            tbAppHeadline.Text = "Queen or Ace of Diamonds?";
                        }
                    }
            }
            else
            {
                if (_deck.SelectedCard.IsBlack)
                {
                    if (_deck.SelectedCard.Suit == CardDeck.Suits.Spades)
                    {
                        if (_deck.SelectedCard.IsOdd) //odd spade
                        {
                            if (_deck.SelectedCard.Value > 6)
                            {
                                tbAppHeadline.Text = "7 or 9 of Spades?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "3,5 of Spades?";
                            }
                        }
                        else //even spade
                        {
                            if (_deck.SelectedCard.Value > 5)
                            {
                                tbAppHeadline.Text = "6,8 or 10 of Spades?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "2,4 of Spades?";
                            }
                        }
                    }
                    else //clubs
                    {
                        if (_deck.SelectedCard.IsOdd) //odd club
                        {
                            if (_deck.SelectedCard.Value > 6)
                            {
                                tbAppHeadline.Text = "7 or 9 of clubs?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "3,5 of Clubs?";
                            }
                        }
                        else //even spade
                        {
                            if (_deck.SelectedCard.Value > 5)
                            {
                                tbAppHeadline.Text = "6,8 or 10 of Clubs?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "2,4 of Clubs?";
                            }
                        }
                    }
                }
                else
                    if (_deck.SelectedCard.Suit == CardDeck.Suits.Hearts)
                    {
                        if (_deck.SelectedCard.IsOdd) //odd heart
                        {
                            if (_deck.SelectedCard.Value > 6)
                            {
                                tbAppHeadline.Text = "7 or 9 of Hearts?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "3,5 of Hearts?";
                            }
                        }
                        else //even heart
                        {
                            if (_deck.SelectedCard.Value > 5)
                            {
                                tbAppHeadline.Text = "6,8 or 10 of Hearts?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "2,4 of Hearts?";
                            }
                        }
                    }
                    else //diamonds
                    {
                        if (_deck.SelectedCard.IsOdd) //odd diamond
                        {
                            if (_deck.SelectedCard.Value > 6)
                            {
                                tbAppHeadline.Text = "7 or 9 of diamonds?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "3,5 of diamonds?";
                            }
                        }
                        else //even spade
                        {
                            if (_deck.SelectedCard.Value > 5)
                            {
                                tbAppHeadline.Text = "6,8 or 10 of diamonds?";
                            }
                            else
                            {
                                tbAppHeadline.Text = "2,4 of diamonds?";
                            }
                        }
                    }
            }
        }

		public void QuitTheApp()
		{
			Application.Current.Exit();
		}

        private void Shuffle()
        {
            lstHints.Items.Clear();
            btnPrev.Visibility = Windows.UI.Xaml.Visibility.Visible;
            btnNext.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _deck.ShuffleAndSelect();
            tbAppName.Text = _deck.SelectedCard.DescriptionLong;
            tbAppName.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            OptionsBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            OptionsBorder.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
            imgSuit.Source = null;
            txtCardBanner.Text = "";
            txtCardValueTopRight.Text = "";
            txtCardValueBottomRight.Text = "";
            txtCardValueBottomLeft.Text = "";

            //RevealCard(true);

            NextStage();
        }

        private void ClearTheDeck()
        {
            lstHints.Items.Clear();
            btnPrev.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnNext.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            tbAppName.Text = _deck.SelectedCard.DescriptionLong;
            OptionsBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            
            imgSuit.Source = null;
        }

        private void RevealCard(bool buttons)
        {
            if (!buttons)
            {
                btnPrev.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnNext.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            imgSuit.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(String.Format(_imageUrl, _deck.SelectedCard.Suit.ToString())));
            var cardValue = _deck.SelectedCard.Value.ToString();
            var cardColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            if (_deck.SelectedCard.Value == 1)
            {
                cardValue = "A";
            }
            else if (_deck.SelectedCard.Value > 10)
            {
                cardValue = _deck.SelectedCard.DescriptionShort;
            }

            if (!_deck.SelectedCard.IsBlack)
            {
                cardColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 46, 18));
            }
            txtCardBanner.Foreground = cardColor;
            txtCardValueTopRight.Foreground = cardColor;
            txtCardValueBottomRight.Foreground = cardColor;
            txtCardValueBottomLeft.Foreground = cardColor;

            txtCardBanner.Text = cardValue;
            txtCardValueTopRight.Text = cardValue;
            txtCardValueBottomRight.Text = cardValue;
            txtCardValueBottomLeft.Text = cardValue;
        }

        private void Reveal_Click(object sender, RoutedEventArgs e)
        {
            RevealCard(true);
        }
    }
}
