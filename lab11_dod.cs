using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelegateProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class TemperatureSensor
        {
            public string Name { get; set; }
            public double CurrentTemp { get; private set; }
            public bool IsFailed { get; private set; }

            private Random rnd = new Random();

            public delegate void SensorEventHandler(string message);

            private SensorEventHandler handlers;

            public void Register(SensorEventHandler h)
            {
                handlers += h;
            }

            public void Read()
            {
                if (IsFailed)
                {
                    handlers?.Invoke($"{Name}: Сенсор не працює.");
                    return;
                }

                int chance = rnd.Next(0, 50);
                if (chance == 5)
                {
                    IsFailed = true;
                    handlers?.Invoke($"{Name}: КРИТИЧНА ПОМИЛКА! Сенсор відмовив.");
                    return;
                }

                CurrentTemp = 15 + rnd.NextDouble() * 40; // 15..55°C

                if (CurrentTemp < 30)
                    handlers?.Invoke($"{Name}: Норма ({CurrentTemp:F1}°C).");
                else if (CurrentTemp < 45)
                    handlers?.Invoke($"{Name}: Попередження — висока температура ({CurrentTemp:F1}°C).");
                else
                    handlers?.Invoke($"{Name}: НЕБЕЗПЕКА! Критична температура ({CurrentTemp:F1}°C)!");
            }
        }

        private void OnSensorMessage(string msg)
        {
            label1.Text += msg + "\n";
        }

        private void OnSystemLog(string msg)
        {
            label2.Text += msg + "\n";
        }

        TemperatureSensor sensor;

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            label2.Text = "";

            sensor = new TemperatureSensor
            {
                Name = "TempSensor-X1"
            };

            sensor.Register(OnSensorMessage);
            sensor.Register(OnSystemLog);

            OnSensorMessage("Система моніторингу запущена.\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sensor?.Read();
        }
    }
}
