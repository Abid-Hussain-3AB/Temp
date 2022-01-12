using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<int> x_arr;
        private List<int> y_arr;
        private int head_x = 125, head_y = 50;
        private int food_x = 200, food_y = 200;
        private int food_size = 15;
        private int length = 3;
        private int food_counter = 0;
        private int bonus_after = 3;
        private int score = 0;
        private Random rand;
        enum direction { LEFT, RIGHT, UP, DOWN }
        direction snake_dir = direction.RIGHT;
        private bool show_bonus_food = false;
        private int bonus_food_time = 0;
        private int bonus_x, bonus_y;
        public Form1()
        {
            InitializeComponent();
            x_arr = new List<int>() { 100, 75, 50 };
            y_arr = new List<int>() { 50, 50, 50 };
            rand = new Random();
            timer1.Interval = 130;
            this.BackColor = Color.Black;
        }
        private void main_paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();
            //score
            g.DrawString(score.ToString(),new Font("Ariel",20),new SolidBrush(Color.White),5,5);
            //draw food
            g.FillEllipse(new SolidBrush(Color.SaddleBrown), food_x, food_y, food_size, food_size);
            
            //draw snake body
            for (int i = 0; i < length; ++i)
                g.FillEllipse(new SolidBrush(Color.Green), new Rectangle(x_arr.ElementAt<int>(i), y_arr.ElementAt<int>(i), 20, 20));
             g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(head_x, head_y, 20, 20));
           

            //ending condetions
            if (detect_wall_Collission() || detect_self_Collission()){
                endGame();
            }
            //food eaten or not?
            if(approach_food()){
                x_arr.Add(x_arr.ElementAt<int>(length - 1) - 25);
                y_arr.Add(y_arr.ElementAt<int>(length - 1) - 25);
                length++;
                generate_food();
                score++;
                food_counter++;
            }
            ///bonus eaten or not?
            if (approach_bonus())
            {
                x_arr.Add(x_arr.ElementAt<int>(length - 1) - 25);
                y_arr.Add(y_arr.ElementAt<int>(length - 1) - 25);
                length++;
                show_bonus_food = false;
                bonus_x = -50;
                bonus_y = -50;
                score += 5;
            }
            //draw bonus now or not?
            if (food_counter == bonus_after){
                show_bonus_food = true;
                if (timer1.Interval > 30)
                    timer1.Interval -= 10;
                bonus_x = rand.Next(0, this.Width - 50);
                bonus_y = rand.Next(0, this.Height - 50);
                food_counter = 0;
            }
            //draw bonus food
            if (show_bonus_food){
                g.FillEllipse(new SolidBrush(Color.DarkSalmon), bonus_x, bonus_y, food_size + 5, food_size + 5);
            }
            ///bonus auto disappear
            if (bonus_food_time == 200){
                show_bonus_food = false;
                bonus_x = -50;
                bonus_y = -50;
                bonus_food_time = 0;
            }
            bonus_food_time++;
        }
        private void endGame()
        {
            timer1.Stop();
            MessageBox.Show("Your score: " + score.ToString(), "Game over!!!");
            this.Close();
        }
        private bool detect_self_Collission()
        {
            for (int i = 0; i < length - 1;++i )
                if (head_x == x_arr[i] && head_y == y_arr[i])
                    return true;
            return false;
        }
        private bool approach_bonus(){
            if (head_x <= bonus_x + food_size + 5 && head_x + 20 >= bonus_x && head_y <= bonus_y + food_size + 5 && head_y + 20 >= bonus_y)
                return true;
            return false;
        }
        private void generate_food()
        {
            food_x = rand.Next(0, this.Width - 50);
            food_y = rand.Next(0, this.Height - 50);
        }
        private bool approach_food()
        {
            if (head_x <= food_x+food_size && head_x + 20 >= food_x && head_y <= food_y+food_size && head_y+20 >= food_y)
                return true;
            return false;
        }
        private bool detect_wall_Collission()
        {
            //         size  margin                       size  margin   
            if (head_x + 20 + 15 >= this.Width || head_y + 20 + 45 >= this.Height || head_x < 0 || head_y < 0)
                return true;
            return false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = length - 1; i > 0; --i)   //updating body list
            {
                x_arr[i] = x_arr.ElementAt<int>(i - 1);
                y_arr[i] = y_arr.ElementAt<int>(i - 1);
            }
            x_arr[0] = head_x;
            y_arr[0] = head_y;
            if (snake_dir == direction.RIGHT)
                head_x += 25;
            else if (snake_dir == direction.LEFT)
                head_x -= 25;
            if (snake_dir == direction.UP)
                head_y -= 25;
            else if (snake_dir == direction.DOWN)
                head_y += 25;
            Invalidate();
        }
        private void change_dir(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && snake_dir != direction.LEFT)
                snake_dir = direction.RIGHT;
            else if (e.KeyCode == Keys.Left && snake_dir != direction.RIGHT)
                snake_dir = direction.LEFT;
            else if (e.KeyCode == Keys.Up && snake_dir != direction.DOWN)
                snake_dir = direction.UP;
            else if (e.KeyCode == Keys.Down && snake_dir != direction.UP)
                snake_dir = direction.DOWN;
        }
    }
}
