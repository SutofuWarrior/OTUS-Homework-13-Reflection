using System;
using System.Diagnostics;
using System.Text.Json;
using ReflectionLib;

namespace ReflectionApp
{
    class Program
    {
        static void Main()
        {
            var pet = new PetClass
            {
                Field1 = 1,
                Field2 = "12324",
                Field3 = 45.67m,
                Field4 = DateTime.Now
            };

            int iterations = 100000;

            var time1 = TestSerialize(pet, iterations, false);
            var time2 = TestSerialize(pet, iterations, true);

            var time3 = TestSerializeJson(pet, iterations, false);
            var time4 = TestSerializeJson(pet, iterations, true);

            string csv = Serializer.SerializeFromObjectToCSV(pet);
            var time5 = TestDeserialize(csv, iterations);

            string json = JsonSerializer.Serialize(pet);
            var time6 = TestDeserializeJson<PetClass>(json, iterations);

            Console.WriteLine($"Сериализация без вывода в консоль: {time1}");
            Console.WriteLine($"Сериализация с выводом в консоль: {time2}");

            Console.WriteLine($"Сериализация в JSON без вывода в консоль: {time3}");
            Console.WriteLine($"Сериализация в JSON с выводом в консоль: {time4}");

            Console.WriteLine($"Десериализация: {time5}");
            Console.WriteLine($"Десериализация из JSON: {time6}");

            Console.ReadLine();
        }

        static long TestSerialize<T>(T pet, int iterations, bool showInConsole = false)
            where T: class
        {
            var timer = Stopwatch.StartNew();
            string csv;

            for (int i = 0; i <= iterations; i++)
            {
                csv = Serializer.SerializeFromObjectToCSV(pet);
                
                if (showInConsole)
                {
                    Console.WriteLine(i);
                    Console.WriteLine(csv);
                }
            }

            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static long TestSerializeJson<T>(T pet, int iterations, bool showInConsole = false)
            where T : class
        {
            string json;
            var timer = Stopwatch.StartNew();

            for (int i = 0; i <= iterations; i++)
            {
                json = JsonSerializer.Serialize(pet);

                if (showInConsole)
                {
                    Console.WriteLine(i);
                    Console.WriteLine(json);
                }
            }

            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static long TestDeserialize(string csv, int iterations)
        {
            var timer = Stopwatch.StartNew();

            for (int i = 0; i <= iterations; i++)
            {
                _ = Serializer.DeserializeFromCSVToObject(csv);
                //Console.WriteLine(i);
            }

            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static long TestDeserializeJson<T>(string json, int iterations)
        {
            var timer = Stopwatch.StartNew();

            for (int i = 0; i <= iterations; i++)
            {
                _ = JsonSerializer.Deserialize(json, typeof(T));
                //Console.WriteLine(i);
            }

            timer.Stop();
            return timer.ElapsedMilliseconds;
        }
    }
}
