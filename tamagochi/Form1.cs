﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tamagochi
{
    public partial class Form1 : Form
    {

        public PictureBox[] queue;
        public PictureBox[] stack;
        public Form1()
        {
            InitializeComponent();

            new Settings();
            stack = new PictureBox[] { pbStack1, pbStack2, pbStack3, pbStack4, pbStack5, pbStack6 };
            queue = new PictureBox[] { pbQueue1, pbQueue2, pbQueue3, pbQueue4, pbQueue5, pbQueue6 };
            

            gameTimer.Interval = 10000 / Settings.speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            queueTimer.Interval = 1000 / Settings.queue_speed;
            queueTimer.Tick += UpdateQueue;
            gameTimer.Start();

            stackTimer.Interval = 10000 / Settings.stack_speed;
            stackTimer.Tick += UpdateStack;
            stackTimer.Start();

            init_game();
        }


        public void UpdateQueue(object sender, EventArgs e)
        {
            show_commands();
        }

        public void show_commands()
        {
            for (int i = 0; i < queue.Length; i++)
            {
                if (Settings.commands.Elements[i] != null)
                {
                    KeyValuePair<Actions, Image> cur_elem =
                        (KeyValuePair<Actions, Image>)Settings.commands.Elements[i];
                    queue[i].Image = cur_elem.Value;
                    queue[i].SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    queue[i].Image = null;
                }
            }
        }

        public void action()
        {
            KeyValuePair<Actions, Image>? cur_elem = Settings.commands.Dequeue();
            if (cur_elem == null)
            {
                return;
            }

            KeyValuePair<Actions, Image> cur_elem_new = (KeyValuePair<Actions, Image>)cur_elem;

            switch (cur_elem_new.Key)
            {
                case Actions.eat:
                    eating();
                    break;

                case Actions.sleep:
                    sleeping();
                    break;

                case Actions.game:
                    happy();
                    break;

                case Actions.clear:
                    clearing();
                    break;
            }

            set_scales();
            Settings.is_game_over = is_die();

        }


        private void init_game()
        {
            new Settings();
            init_scale(lblEatCur, lblEatMax, Settings.eat);
            init_scale(lblSleepCur, lblSleepMax, Settings.sleep);
            init_scale(lblHappyCur, lblHappyMax, Settings.happy);
            init_scale(lblClearCur, lblClearMax, Settings.clear);
            init_scale(lblHPCur, lblHPMax, Settings.HP);

            lblGameOver.Visible = false;
        }
        private void init_scale(Label lbl_cur, Label lbl_max, Scale scale)
        {
            lbl_cur.Text = scale.current_value.ToString();
            lbl_max.Text = scale.max_value.ToString();
        }

        private Scale add_value(int add_value, Scale scale)
        {
            scale.current_value += add_value;
            if (scale.current_value > scale.max_value)
            {
                scale.current_value = scale.max_value;
            }

            return scale;
        }

        private Scale dif_value(int dif_value, Scale scale)
        {
            scale.current_value -= dif_value;
            if (scale.current_value < 0)
            {
                scale.current_value = 0;
            }

            return scale;
        }

        private bool is_die()
        {
            int cur_life = (int)(0.25 * Settings.eat.current_value +
                0.25 * Settings.sleep.current_value +
                0.25 * Settings.clear.current_value +
                0.25 * Settings.happy.current_value);

            Settings.HP.current_value = cur_life;

            if (Settings.HP.current_value == 0
               || Settings.happy.current_value == 0
               || Settings.eat.current_value == 0
               || Settings.clear.current_value == 0
               || Settings.sleep.current_value == 0)
            {
                return true;
            }

            return false;
        }

        private void set_scales()
        {
            lblClearCur.Text = Settings.clear.current_value.ToString();
            lblSleepCur.Text = Settings.sleep.current_value.ToString();
            lblEatCur.Text = Settings.eat.current_value.ToString();
            lblHPCur.Text = Settings.HP.current_value.ToString();
            lblHappyCur.Text = Settings.happy.current_value.ToString();
        }

        private void eating()
        {
            Settings.eat = add_value(Settings.add, Settings.eat);
            Settings.happy = add_value(Settings.add, Settings.happy);
            Settings.clear = dif_value(Settings.dif, Settings.clear);
            Settings.sleep = dif_value(Settings.dif, Settings.sleep);


        }

        private void sleeping()
        {
            Settings.sleep = add_value(Settings.add, Settings.sleep);
            Settings.happy = dif_value(Settings.dif, Settings.happy);
            Settings.clear = dif_value(Settings.dif, Settings.clear);
            Settings.eat = dif_value(Settings.dif, Settings.eat);


        }

        private void happy()
        {
            Settings.happy = add_value(Settings.add, Settings.happy);
            Settings.clear = dif_value(Settings.dif, Settings.clear);
            Settings.eat = dif_value(Settings.dif, Settings.eat);
            Settings.sleep = dif_value(Settings.dif, Settings.sleep);


        }

        private void clearing()
        {
            Settings.clear = add_value(Settings.add, Settings.clear);
            Settings.happy = dif_value(Settings.dif, Settings.happy);
            Settings.clear = dif_value(Settings.dif, Settings.clear);
            Settings.eat = dif_value(Settings.dif, Settings.eat);


        }

        private void game_over_actions()
        {
            pbImage.BackgroundImage = Properties.Resources.die;
            lblGameOver.Visible = true;
            btnClear.Enabled = false;
            btnEat.Enabled = false;
            btnSleep.Enabled = false;
            btnHappy.Enabled = false;
            btnAction.Enabled = false;
            btnStackAction.Enabled = false;
        }

        private void btnEat_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int cur_value = random.Next(0, 2);

            if (cur_value == 0)
            {
                Settings.commands.Enqueue(
    new KeyValuePair<Actions, Image>(Actions.eat, Properties.Resources.l_cz8V42WxA));
            }
            else
            {
                Settings.stack_commands.Push(
                    new KeyValuePair<Actions, Image>(Actions.eat, Properties.Resources.l_cz8V42WxA));
            }


            
            eating();
            set_scales();
            Settings.is_game_over = is_die();
            
        }

        private void btnSleep_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int cur_value = random.Next(0, 2);

            if (cur_value == 0)
            {
                Settings.commands.Enqueue(
    new KeyValuePair<Actions, Image>(Actions.sleep, Properties.Resources.vLMH3Ussw6E));
            }
            else
            {
                Settings.stack_commands.Push(
                    new KeyValuePair<Actions, Image>(Actions.sleep, Properties.Resources.vLMH3Ussw6E));
            }


            
            sleeping();
            set_scales();
            Settings.is_game_over = is_die();
            
        }

        private void btnHappy_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int cur_value = random.Next(0, 2);

            if (cur_value == 0)
            {
                Settings.commands.Enqueue(
    new KeyValuePair<Actions, Image>(Actions.game, Properties.Resources._1AKUCL1lihY));
            }
            else
            {
                Settings.stack_commands.Push(
                    new KeyValuePair<Actions, Image>(Actions.game, Properties.Resources._1AKUCL1lihY));
            }


            
            happy();
            set_scales();
            Settings.is_game_over = is_die();
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int cur_value = random.Next(0, 2);

            if (cur_value == 0)
            {
                Settings.commands.Enqueue(
    new KeyValuePair<Actions, Image>(Actions.clear, Properties.Resources.p3sWi7EL8kc));
            }
            else
            {
                Settings.stack_commands.Push(
                    new KeyValuePair<Actions, Image>(Actions.clear, Properties.Resources.p3sWi7EL8kc));
            }

            
            clearing();
            set_scales();
            Settings.is_game_over = is_die();
            
        }


        private void generate_actions(Random random)
        {
            List<Action> actions = new List<Action>() { dec_eat, dec_sleep, dec_clear, dec_happy };
            int index = random.Next(0, actions.Count);
            actions[index]();
        }

        private void dec_eat()
        {
            Settings.eat.current_value -= Settings.default_dif;
            set_scales();
            Settings.is_game_over = is_die();
        }

        private void dec_sleep()
        {
            Settings.sleep.current_value -= Settings.default_dif;
            set_scales();
            Settings.is_game_over = is_die();
        }

        private void dec_clear()
        {
            Settings.clear.current_value -= Settings.default_dif;
            set_scales();
            Settings.is_game_over = is_die();
        }

        private void dec_happy()
        {
            Settings.happy.current_value -= Settings.default_dif;
            set_scales();
            Settings.is_game_over = is_die();
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.is_game_over)
            {
                game_over_actions();
            }
            else
            {
                Random random = new Random();
                int is_action = random.Next(0, 2);
                if (is_action == 1)
                {
                    generate_actions(random);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            action();
        }

        private void UpdateStack(object sender, EventArgs e)
        {
            show_stack_commands();
        }

        private void show_stack_commands()
        {
            for (int i = 0; i < stack.Length; i++)
            {
                if (Settings.stack_commands.Elements[i] != null)
                {
                    KeyValuePair<Actions, Image> cur_elem =
                        (KeyValuePair<Actions, Image>)Settings.stack_commands.Elements[i];
                    stack[i].Image = cur_elem.Value;
                    stack[i].SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    stack[i].Image = null;
                }
            }
        }

        private void stack_action()
        {
            KeyValuePair<Actions, Image>? cur_elem = Settings.stack_commands.Pop();
            if (cur_elem == null) { return; }

            KeyValuePair<Actions, Image> cur_elem_new =
                (KeyValuePair<Actions, Image>)cur_elem;

            switch (cur_elem_new.Key)
            {
                case Actions.eat:
                    eating();
                    break;

                case Actions.sleep:
                    sleeping();
                    break;

                case Actions.game:
                    happy();
                    break;

                case Actions.clear:
                    clearing();
                    break;
            }

            set_scales();
            Settings.is_game_over = is_die();
        }

        private void btnStackAction_Click(object sender, EventArgs e)
        {
            stack_action();
        }
    }
    
}
