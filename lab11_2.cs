using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Delegate_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Car
        {
            public int CurrentSpeed { get; set; }
            public int MaxSpeed { get; set; }
            public string PetName { get; set; }
            private bool carIsDead; // поле для перевірки, чи автомобіль не зламався
            static public double distance = 0; // додане статичне поле
            public Car() // конструктор класу
            {
                MaxSpeed = 100;
            }
            public Car(string name, int maxSp, int currSp) // конструктор з параметрами
            {
                MaxSpeed = maxSp;
                CurrentSpeed = currSp;
                PetName = name;
            }
            // Оголошення делегата у класі Car
            public delegate void CarEngineHandler(string msgForCaller);
            // Оголошення закритої змінної listOfHandlers типу делегат
            CarEngineHandler listOfHandlers;
            // Додавання методу для доступу до змінної listOfHandlers ззовні
            public void RegisterWithCarEngine(CarEngineHandler metodToCall)
            {
                // Змінній типу делегат присвоюємо метод, що має сигнатуру, яка вказана при оголошенні делегата
                listOfHandlers += metodToCall; // змінений оператор
            }
            /* Метод для зміни поточної швидкості автомобіля. Він буде викликати процес створення повідомлення і додавання його до тексту мітки. Залежно від швидкості автомобіля, будуть генеруватись різні повідомлення */
            public void Accselerate(int delta)
            {
                if (carIsDead)
                {
                    /* Змінна типу делегат listOfHandlers запускає метод, вказаний при її створенні з параметрами, заданими в операторі звертання до змінної. Перевіримо, чи передано метод у змінну listOfHandlers і якщо так, то викликаємо метод, адреса якого записана у змінній listOfHandlers */
                    if (listOfHandlers != null)
                        listOfHandlers("На жаль, автомобіль зламався");
                }
                else
                {
                    distance += CurrentSpeed * 0.16; // до шляху пробігу додаєм шлях пробігу за 10 хвилин.
                                                     // 0.16 години це приблизно 10 хвилин
                    CurrentSpeed += delta;
                    // Перевіряємо, чи передано метод у змінну listOfHandlers, а також
                    // перевіряємо, чи не занадто велика швидкість і якщо так, то видаємо повідомлення
                    if ((MaxSpeed - CurrentSpeed <= 10) && (CurrentSpeed < MaxSpeed) && listOfHandlers != null)
                    // Змінена умова
                    // Викликаємо метод, записаний у делегаті за допомогою змінної типу делегат з новими параметрами
                    {
                        listOfHandlers("Увага! Занадто велика швидкість!");
                    }
                    else
                    {
                        if (CurrentSpeed >= MaxSpeed)
                            carIsDead = true;
                        else
                            listOfHandlers("Поточна швидкість=" + CurrentSpeed.ToString());
                    }
                }
            }
        }
        // Перший метод для делегатів, який видає повідомлення у текст мітки label1
        public void OnCarEngineEvent1(string msg)
        {
            label1.Text = label1.Text + msg + " \n";
        }
        // Другий метод для делегата, він видає повідомлення про довжину шляху пробігу автомобіля
        public void OnCarEngineEvent2(string msg)
        {
            label2.Text = label2.Text + "Пробіг:" + Car.distance.ToString() + "км. \n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Створюємо об'єкт типу Car
            Car myCar = new Car("Старенький Запорожець", 100, 0);
            // Створюємо метод, адресу якого будемо передавати делегату
            // Створимо змінну типу делегат і зашлемо в неї адресу методу, який буде викликатись через цю змінну
            // Змінено
            Car.CarEngineHandler myDelegat1 = new Car.CarEngineHandler(OnCarEngineEvent1);
            // Додано
            Car.CarEngineHandler myDelegat2 = new Car.CarEngineHandler(OnCarEngineEvent2);
            /* Звернемось до методу RegisterWithCarEngine, щоб вказати метод, який повинен викликатись (зареєструвати) */
            myCar.RegisterWithCarEngine(myDelegat1);
            myCar.RegisterWithCarEngine(myDelegat2);
            // Ми можемо викликати метод OnCarEngineEvent і поза делегатом
            OnCarEngineEvent1("Стартуємо");
            // Змінюємо швидкість автомобіля і відслідковуємо, що буде
            for (int i = 0; i < 11; i++)
                myCar.Accselerate(10);
        }
    }
}
