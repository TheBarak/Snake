using System;
using System.Collections.Generic;
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

using System.Windows.Threading;//שימוש חדש

namespace Snake
{
    public partial class MainWindow : Window
    {
        private List<Ellipse> snakeBody = new List<Ellipse>();// מסמל את הגוף של הנחש שמחזיק אובייקטים מסוג אליפסה
        private int speed = 20;//כמות שינוי בפעם אחת באחד הצירים
        private int diraction = 1;//כיוון התנועה כאשר: 1=ימין ,2=למטה ,3=שמאל , 4=למעלה
        private int score = 0;//ניקוד
        private int index = 0;//עוזר הגדלת הגוף- למעשה שומר את המספר אחרי הזנב 
        private int mone = 0;//בודק את תחילת המשחק כדי שהטיימר יתחיל רק פעם אחת
        public MainWindow()
        {
            InitializeComponent();
        }
        public void Increase()//פעולה המגדילה את הגוף 
        {
                index++;//עולה "לזנב" שאמור לגדול עכשיו
                snakeBody.Add(new Ellipse());//מוסיף אליפסה חדשה לגוף הנחש
                snakeBody[index].Visibility = Visibility.Visible;//משנה את הראות של הזנב לניראה
                snakeBody[index].Fill = new SolidColorBrush(Colors.LightGreen);//קובע צבע
                snakeBody[index].Width = 19;//קובע רוחב
                snakeBody[index].Height = 19;//קובע אורך
                canvas.Children.Add(snakeBody[index]);//פעולה שמוסיפה לקאנבאס את הזנב
                /*
                תאורטית אין צורך בפעולה הבאה היא רק קובעת היכן יופיע האובייקט החדש כלומר נקודות ציון
                 היא מונעת את הופעתו של האובייקט ב 0,0 והיבהוב סתמי על המסך בעת יצירתו
                */
                 //snakeBody[index].Margin = new Thickness { Left = snakeBody[index - 1].Margin.Left, Top = snakeBody[index - 1].Margin.Top };
                 //משווה את את האובייקט החדש לנקודות הציון של האובייקט הקודם
        }

        public void Timer_Tick(object sender, EventArgs e)//פעולה שאחראית על כיוון התנועה והתנועה של הגוף והראש
        {
            for (int i = snakeBody.Count - 1; i >= 0; i--)//ריצה על האורך של הגוף
            {
                if (i == 0)//שינוי ראש בלבד
                {
                    if (diraction == 1)//תזוזה ימינה
                    {
                        snakeBody[i].Margin = new Thickness { Left = snakeBody[i].Margin.Left + this.speed , Top = snakeBody[i].Margin.Top };//מתרחק מהשמאל
                    }
                    if (diraction == 2)//תזוזה למטה
                    {
                        snakeBody[i].Margin = new Thickness { Top = snakeBody[i].Margin.Top + this.speed, Left = snakeBody[i].Margin.Left };//מתרחק מלמעלה
                    }
                    if (diraction == 3)//תזוזה שמאלה
                    {
                        snakeBody[i].Margin = new Thickness { Left = snakeBody[i].Margin.Left - this.speed, Top = snakeBody[i].Margin.Top };//מתקרב שמאלה
                    }
                    if (diraction == 4)//תזוזה למעלה
                    {
                        snakeBody[i].Margin = new Thickness { Top = snakeBody[i].Margin.Top - this.speed, Left = snakeBody[i].Margin.Left };//מתקרב למעלה
                    }
                }
                else//כל שאר הגוף
                {
                    snakeBody[i].Margin = new Thickness { Top = snakeBody[i - 1].Margin.Top, Left = snakeBody[i - 1].Margin.Left };//מעקב שאר הגוף אחד אחרי השני
                    //האובייקט שעכשיו מושווה לנקודות ציון של האובייקט שלפניו ולכן עוקב אחריו
                }
            }
        }
        public void gr_KeyDown(object sender, KeyEventArgs e)//פעןלה שקשורה למקלדת ומשנה את הכיוון בעת לחיצה על מקש
        {
            if (Keyboard.IsKeyDown(Key.Up) && diraction!=2 && !Keyboard.IsKeyDown(Key.Left) && !Keyboard.IsKeyDown(Key.Down) && !Keyboard.IsKeyDown(Key.Right))
             //בעת לחיצה על מקש למעלה וכל עוד הכיוון לא למטה + בודק שמקשים אחרים אינם לחוצים
            {
                diraction = 4;//משנה את הכיוון למעלה
            }
            if (Keyboard.IsKeyDown(Key.Left) && diraction != 1 && !Keyboard.IsKeyDown(Key.Up) && !Keyboard.IsKeyDown(Key.Down) && !Keyboard.IsKeyDown(Key.Right))
             //לחיצה על מקש שמאלה כל עוד כיוון לא ימינה + מקשים אחרים לא לחוצים 
            {
                diraction = 3;//משנה כיוון לשמאל
            }
            if (Keyboard.IsKeyDown(Key.Down) && diraction != 4 && !Keyboard.IsKeyDown(Key.Left) && !Keyboard.IsKeyDown(Key.Up) && !Keyboard.IsKeyDown(Key.Right))
            {
                diraction = 2;//משנה כיוון למטה
            }
            if (Keyboard.IsKeyDown(Key.Right) && diraction != 3 && !Keyboard.IsKeyDown(Key.Left) && !Keyboard.IsKeyDown(Key.Down) && !Keyboard.IsKeyDown(Key.Up))
            {
                diraction = 1;//משנה כיוון לימין
            }
        }
        public void StartTime()// פעולה שמתחילה את הטיימר ואת המשחק
        {
            var timer = new DispatcherTimer();//יוצר טיימר
            timer.Interval = TimeSpan.FromMilliseconds(25);//מכוון מד זמן כל כמה זמן תיקתוק
            timer.Tick += new EventHandler(Timer_Tick);//כל עוד מתקתק מתקיימת פעולה(כל תקתוק פעולה)כ
            timer.Tick += new EventHandler(GameOver);//כל עוד מתקתק מתקיימת פעולה
            timer.Tick += new EventHandler(Eat);//כל עוד מתקתק מתקיימת פעולה
            timer.Start(); //תחילת תיקתוק הטיימר
        }
        public void Eat(object sender , EventArgs e)//פעולה שבודקת אם אכל
        {
            if ((food.Margin.Top < elip.Margin.Top + 10 && food.Margin.Top > elip.Margin.Top - 10 )  &&  ( food.Margin.Left < elip.Margin.Left + 10 && food.Margin.Left > elip.Margin.Left - 10))
            //אם נקודות הציון של של הראש קרובות לנקודות הציון של האוכל
            {
                score++;//ניקוד עולה באחד
                Random rnd = new Random();//רנדומם
                int MaxH = rnd.Next(10, 285);//אורך קאנבאס
                int MaxW = rnd.Next(10, 475);//ארוחב קנבאס
                food.Margin = new Thickness { Left = MaxW, Top = MaxH };//קובע נקודות חדשות לאוכל
                Increase();//מזמן פעולה שמגדילה את הגוף
            }
        }
        public void Start(object sender, RoutedEventArgs e)//הגדרות ראשוניות בתחילת משחק בעת לחיצה על כפתור
        {
            elip.Margin = new Thickness(150,150,0,0);//מיקום מרחק מ: (שמאל,למעלה,ימין,למטה) קובע נקדות חדשות של הראש
            this.score = 0;//ניקוד 0
            this.speed = 7;
            this.diraction = 1;
            this.index = 0;
            //איפוס נתונים
            elip.Fill = new SolidColorBrush(Colors.Green);//צביעה של ראש
            elip.Visibility = Visibility.Visible;//ראות של ראש
            food.Fill = new SolidColorBrush(Colors.Red);//צביעת אוכל
            food.Visibility = Visibility.Visible;//ראות אוכל
            sg.Margin = new Thickness {Left = 1500 , Top = 1500 };//מזיז את הכפתור מחוץ לגבולות "המיין וינדוו"-זאת כדי לא למנוע את התנועה ברגע שמשנים את הראות
            //sg.Visibility = Visibility.Hidden;//לא ידוע מדוע לא עובד ככה
            int MaxH = 0 , MaxW =0;//קןבע מיקום ראשוני של אוכל
            Random rnd = new Random();
            MaxH = rnd.Next(10, 285);
            MaxW = rnd.Next(10, 475);
            food.Margin = new Thickness {Left = MaxW , Top = MaxH };
            //קובע מיקום רנדומלי לאוכל על גבולות המשחק
            snakeBody.Add(elip);//מכניס את הראש לגוף הנחש
            if (this.mone == 0)//מדליק את הטיימר רק בתחילת המשחק הראשון לאחר מכן הוא אינו נדלק בשנית
            {
                StartTime();//מתחיל את הטיימר והמשחק
                this.mone++;
            }
        }



        public void GameOver(object sender , EventArgs e)
        {
            for (int i = snakeBody.Count - 1; i > 0; i-- )//רץ על הגוף
            {
                if (elip.Margin.Left == snakeBody[i].Margin.Left && elip.Margin.Top == snakeBody[i].Margin.Top)//בודק נגיעה בעצמו
                {
                    restart();//מציב כפתור לחזרה נוספת
                    break;
                }
            }
            if (elip.Margin.Top > 290 || elip.Margin.Top < 10 || elip.Margin.Left > 480 || elip.Margin.Left < 10)//בודק יציאה מהקווים
            {
                restart();//מציב כפתור לחזרה נוספת
            }

        }
        public void restart()//חזרה נוספת
        {
            elip.Visibility = Visibility.Hidden;
            food.Visibility = Visibility.Hidden;
            /* 
             מסתיר את האוכל והראש
             */
            sg.Margin = new Thickness { Left = 20, Top = 20 };//מציב את הכפתור הקודם בנוקודת חדשות
            sg.Height = 270;
            sg.Width = 300;//משנה ערכי גובה ורוחב
            sg.Content = "   Game Over \n  Start Again? \n      Score:" + this.score;//משנה את הכתוב + מראה ניקוד
            for (int i = 0; i < snakeBody.Count; i++)//רץ על גוף
            {
                snakeBody[i].Visibility = Visibility.Hidden;//מסתיר כל פריט בגוף שהיה
            }
            snakeBody.RemoveRange(0, snakeBody.Count);//מוחק את גוף הנחש במרחק שנתון לו
            
        }

    }
}
