﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tamagochi
{

    public enum Actions
    {
        eat,
        sleep,
        game,
        clear
    }
    class Settings
    {
        public static Scale eat;
        public static Scale sleep;
        public static Scale happy;
        public static Scale clear;
        public static Scale HP;
        public static int speed;
        public static int default_dif;
        public static int dif;
        public static int add;
        public static bool is_game_over;

        public static MyQueue commands;
        public static int queue_speed;

        public static MyStack stack_commands;
        public static int stack_speed;

        public  Settings()
        {
            eat = new Scale(100, 100);
            sleep = new Scale(100, 100);
            happy = new Scale(100, 100);
            clear = new Scale(100, 100);
            HP = new Scale(100, 100);
            dif = 5;
            add = 20;
            speed = 10;
            default_dif = 1;
            is_game_over = false;

            commands = new MyQueue();
            queue_speed = 16;

            stack_commands = new MyStack();
            stack_speed = 16;
        }

      
    }
}
