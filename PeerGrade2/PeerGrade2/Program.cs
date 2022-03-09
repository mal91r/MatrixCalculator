using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PeerGrade2
{

    static class Program
    {
        /// <summary>
        /// Метод Мэйн. 
        /// Отсюда идет запуск программы и повторный запуск.
        /// </summary>
        static void Main()
        {
            Game.Greeting();
            ConsoleKeyInfo btn;
            //Переменная для чтения нажатия клавиши 
            do
            {
                Game.Menu();
                Console.WriteLine("Для завершения игры нажмите \"esc\", для продолжения нажмите любую другую клавишу.");
                btn = Console.ReadKey();
                // Чтение нажатия клавиши          
                Console.WriteLine();
            }
            while (!(btn.Key == ConsoleKey.Escape));

        }

    }
    public class Game
    {
        /// <summary>
        /// Меню игры.
        /// Тут просходит выбор операции и вызов нужной функции.
        /// </summary>
        public static void Menu()
        {
            int operation = Game.Operation();

            switch (operation)
            {
                case 1:
                    MatrixTrace();
                    break;
                case 2:
                    MatrixTransposition();
                    break;
                case 3:
                    MatrixSum();
                    break;
                case 4:
                    MatrixDifference();
                    break;
                case 5:
                    MatrixMultiplication();
                    break;
                case 6:
                    MatrixNumberMultiplication();
                    break;
                case 7:
                    MatrixDeterminant();
                    break;
                case 8:
                    GaussMethod();
                    break;
            }


        }

        /// <summary>
        /// Метод для приветствия игроков. Вызывается всего 1 раз.
        /// </summary>
        public static void Greeting()
        {
            Console.WriteLine("Привет! Перед тобой калькулятор матриц. Итак, давай начнем..");
        }

        /// <summary>
        /// Метод, который отвечает за генерацию матриц. 
        /// Есть 3 способа генерации, для каждрого из них вызывается свой метод. 
        /// До и после этого идет обработка эксепшнов.
        /// </summary>
        /// <returns></returns>
        public static double[,] Generation()
        {
            Console.WriteLine("Выберите способ ввода матрицы: \n1. Ввод вручную. \n2. Автоматическая генерация. \n3. Чтение из файла.");
            int command;
            while (!int.TryParse(Console.ReadLine(), out command) || command < 1 || command > 3)
            {
                Console.WriteLine("Ошибка. Введите номер команды(число от 1 до 3).");
            }
            Console.WriteLine("P.S. Если в числе есть ведущие нули, то программа прочитает число без них, т.е. число 0023 будет представлено как 23.");

            double[,] matrix = new double[,] { };
            switch (command)
            {
                case 1:
                    matrix = InputGeneration();
                    break;
                case 2:
                    matrix = RandomGeneration();
                    break;
                case 3:
                    matrix = FileGeneration();
                    break;
            }

            if (matrix.Length == 0)
            {

            }
            if (matrix.Length != 0)
            {
                Console.WriteLine("Вот матрица, получаенная на вход.");
                Output(matrix);
            }


            return matrix;
        }

        /// <summary>
        /// Чтение матрицы из файла.
        /// Тут в 40 строчек уложиться не получилось, но это невозможно, имхо, так что не бейте.
        /// </summary>
        /// <returns></returns>
        public static double[,] FileGeneration()
        {
            Console.WriteLine("Для чтения из файла необходимо соблюдать некоторые правила:\n" +
                              "1. Файл должен лежать в той же папке, что и программа.\n" +
                              "2. Файл должен содержать в первой строке через пробел параметры матрицы(размер).\n" +
                              "3. В следующих строках через пробел должны быть записаны элементы матрицы.\n" +
                              "4. не должно быть пустых строчек, иначе файл не прочитается.\n" +
                              "На этом всё, можете приступать к работе.");
            //Правила чтения из матрицы. 
            //Ну а че, правила везде должны быть.
            double[,] answer = new double[,] { };
            Console.WriteLine("Введите имя файла.");
            string fileName = Console.ReadLine();
            string path = $"../../../{fileName}";
            //Узнаю имя файла и генерирую путь.
            try
            {
                string[] lines = File.ReadAllLines(path);
                int n = 0, m = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] += " ";
                }
                //Читаю построчно информацию из файла

                string[] size = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (size.Length > 2)
                {
                    Console.WriteLine("Некорректный файл.");
                    return answer;
                }
                //В первой строке передаются параметры. Если там не два числа, то получается что-то пошло не так..
                else
                {
                    if (!int.TryParse(size[0], out n) || !int.TryParse(size[1], out m))
                    {
                        Console.WriteLine("Некорректный файл. В первой строке должны содержаться размеры матрицы.");
                        return answer;
                    }
                    //Если там не число, то тем более грустно.
                }
                if (lines.Length - 1 != n)
                {
                    Console.WriteLine("Размер не соответствует содержимому.");
                    return answer;
                }
                //Проверка соответствия количествка строчек параметру, переданному выше.

                if (n < 1 || m < 1 || n > 10 || m > 10)
                {
                    Console.WriteLine("Некорректные параметры матрицы.");
                    return answer;
                }
                //Размеры должны быть от 1 до 10.
                double[,] a = new double[n, m];
                for (int i = 1; i <= a.GetLength(0); i++)
                {
                    string[] b = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    if (b.Length != m)
                    {
                        Console.WriteLine("Размер не соответствует содержимому.");
                        return answer;
                    }
                    //Перевожу строчки в массивы стрингов.
                    for (int j = 0; j < m; j++)
                    {
                        if (!double.TryParse(b[j], out double x))
                        {
                            Console.WriteLine("Элементы матрицы должны быть числами.");
                            return answer;
                        }

                        if (x > 1000)
                        {
                            Console.WriteLine("Слишком большие элементы матрицы. Числа не должны превышать 1000.");
                            return answer;
                        }
                        a[i - 1, j] = x;
                    }
                    //Перевожу массив стрингов в матрицу чисел.
                }

                answer = a;
            }
            catch
            {
                Console.WriteLine("Ошибка.");
                return answer;
            }
            //Возвращаю ответ.
            return answer;
        }


        /// <summary>
        /// Вывод матриц.
        /// Тут реализован красивый вывод.
        /// </summary>
        /// <param name="matrix"></param>
        public static void Output(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0, 9}", matrix[i, j]);
                }
                Console.WriteLine();

            }
        }

        /// <summary>
        /// Реализация ввода через консоль.
        /// Тут ввод происходит в формате 1 число на 1 строке, подроднее в README.txt.
        /// </summary>
        /// <returns></returns>
        public static double[,] InputGeneration()
        {
            Console.WriteLine("Введите размеры матрицы. Каждый параметр не может превышать 10.");
            Console.WriteLine("Введите n.");
            int n = SizeGeneration();
            Console.WriteLine("Введите m.");
            int m = SizeGeneration();
            double[,] matrix = new double[n, m];
            Console.WriteLine($"Сейчас Вам необходимо ввести числа - элементы матрицы. Каждое число вводится с новой строки.\n" +
                              $"Количество ожидаемых чисел - {n * m}");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    double x;
                    Console.WriteLine($"Введено чисел - {i * m + j}/{m * n}");
                    while (!double.TryParse(Console.ReadLine(), out x) || x > 1000)
                    {
                        Console.WriteLine("Ошибка. Введите вещественное число. Число должно быть меньше 1000.");
                    }

                    matrix[i, j] = x;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Генерация матрицы и заполнение ее случаными вещественным числами.
        /// Обращаю внимание, что знак элемента так же гененрируется случайным образом.
        /// </summary>
        /// <returns></returns>
        public static double[,] RandomGeneration()
        {
            Console.WriteLine("Введите размеры матрицы. Каждый параметр не может превышать 10.");
            Console.WriteLine("Введите n.");
            int n = SizeGeneration();
            Console.WriteLine("Введите m.");
            int m = SizeGeneration();
            double[,] matrix = new double[n, m];
            Random rand = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    double x = rand.NextDouble() * 1000;
                    x *= Math.Pow(-1, rand.Next(1, 3));
                    x = Math.Round(x, 3);
                    matrix[i, j] = x;
                }
            }

            return matrix;
        }


        /// <summary>
        /// Генерация размера матриц.
        /// Тут проиходит ввод и проверка на корректность.
        /// </summary>
        /// <returns></returns>
        public static int SizeGeneration()
        {
            int n;
            while (!int.TryParse(Console.ReadLine(), out n) || n > 10 || n < 1)
            {
                Console.WriteLine("Ошибка. Неверный формат ввода. Необходимо ввести целое число от 1 до 10");
            }

            return n;
        }

        /// <summary>
        /// Выбор операции.
        /// Пользователю предоставляется перечень на выбор, он вводит нужную цифру, она возращается в Меню и вызывается нужный метод.
        /// </summary>
        /// <returns></returns>
        public static int Operation()
        {
            Console.WriteLine("Для начала игры введите номер команды из слудющего списка:\n" +
                "1. нахождение следа матрицы;\n" +
                "2. транспонирование матрицы;\n" +
                "3. сумма двух матриц;\n" +
                "4. разность двух матриц;\n" +
                "5. произведение двух матриц;\n" +
                "6. умножение матрицы на число;\n" +
                "7. нахождение определителя матрицы.\n" +
                "8. решение СЛАУ методом Гаусса.");
            int numberOfOperation;
            while (!int.TryParse(Console.ReadLine(), out numberOfOperation) || numberOfOperation > 8 || numberOfOperation < 1)
            {
                Console.WriteLine("Ошибка! Необходимо ввести число от 1 до 8.");
            }
            return numberOfOperation;
        }
        /// <summary>
        /// Тут я ищу определитель.
        /// Функция вызывается из Метода Гаусса, основная функция ниже.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Determinant(double[][] matrix)
        {
            int size = matrix.Length;
            //Создаю вспомогательный массив.
            double[][] helper = new double[1][];
            helper[0] = new double[size];
            double det = 1;
            //Определяем переменную EPS(эпсилон).
            const double EPS = 1E-9;
            //Проходим по строкам.
            for (int i = 0; i < size; ++i)
            {
                //Присваиваем k номер строки.
                int k = i;
                //Идем по строке от i+1 до конца.
                for (int j = i + 1; j < size; ++j)
                    //Проверяем.
                    if (Math.Abs(matrix[j][i]) > Math.Abs(matrix[k][i]))
                        //Если равенство выполняется то k присваиваем j.
                        k = j;
                //Если равенство выполняется то определитель приравниваем 0 и выходим из программы.
                if (Math.Abs(matrix[k][i]) < EPS)
                {
                    det = 0;
                    break;
                }
                //Меняем местами a[i] и a[k].
                helper[0] = matrix[i];
                matrix[i] = matrix[k];
                matrix[k] = helper[0];
                //Если i не равно k.
                if (i != k)
                    //То меняем знак определителя.
                    det = -det;
                //Умножаем det на элемент a[i][i].
                det *= matrix[i][i];
                //Идем по строке от i+1 до конца.
                for (int j = i + 1; j < size; ++j)
                    //Каждый элемент делим на a[i][i].
                    matrix[i][j] /= matrix[i][i];
                //Идем по столбцам.
                for (int j = 0; j < size; ++j)
                    //Проверяем.
                    if ((j != i) && (Math.Abs(matrix[j][i]) > EPS))
                        //Если да, то идем по k от i+1.
                        for (k = i + 1; k < size; ++k)
                            matrix[j][k] -= matrix[i][k] * matrix[j][i];
            }
            //Выводим результат.
            return Math.Round(det, 3);
            //Округляю, чтобы не было траблов с представлением вещественных.
        }

        /// <summary>
        /// Вот это уже настоящий метод для поиска определителя, он вызывается из Меню.
        /// </summary>
        public static void MatrixDeterminant()
        {
            string s;
            string[] str;
            double det = 1;
            //Определяем переменную EPS(эпсилон).
            const double EPS = 1E-9;
            //Размерность матрицы.
            int size;
            //Вводим размер.
            Console.WriteLine("Введите размерность матрицы.");

            while (!int.TryParse(Console.ReadLine(), out size) || size > 10 || size < 1)
            {
                Console.WriteLine("Некорректный ввод. Введите число от 1 до 10.");
            }
            //Определяем массив размером nxn.
            double[][] matrix = new double[size][];
            double[][] helper = new double[1][];
            helper[0] = new double[size];
            //Заполняем его.
            Console.WriteLine($"Введите элементы построчно. Количество элементов на каждой строчке через пробел - {size}.");
            //Ниже реализован ввод матрицы.
            for (int i = 0; i < size; i++)
            {
                s = Console.ReadLine();
                str = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                while (str.Length != size)
                {
                    Console.WriteLine("Ошибка ввода. Введите строку заново.");
                    s = Console.ReadLine();
                    str = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                }
                matrix[i] = new double[size];
                for (int j = 0; j < size; j++)
                {
                    if (!double.TryParse(str[j], out matrix[i][j]) || matrix[i][j] > 1000)
                    {
                        Console.WriteLine("Ошибка. Один или несколько из элементов невозможно преобразовать к числовому виду. " +
                                          "Введите строку заново.");
                        i--;
                        break;

                    }
                }
            }
            //Проходим по строкам.
            for (int i = 0; i < size; ++i)
            {
                //Присваиваем k номер строки.
                int k = i;
                //Идем по строке от i+1 до конца.
                for (int j = i + 1; j < size; ++j)
                    //Проверяем.
                    if (Math.Abs(matrix[j][i]) > Math.Abs(matrix[k][i]))
                        //Если равенство выполняется то k присваиваем j.
                        k = j;
                //Если равенство выполняется то определитель приравниваем 0 и выходим из программы.
                if (Math.Abs(matrix[k][i]) < EPS)
                {
                    det = 0;
                    break;
                }
                //Меняем местами a[i] и a[k].
                helper[0] = matrix[i];
                matrix[i] = matrix[k];
                matrix[k] = helper[0];
                //Если i не равно k.
                if (i != k)
                    //То меняем знак определителя.
                    det = -det;
                //Умножаем det на элемент a[i][i].
                det *= matrix[i][i];
                //Идем по строке от i+1 до конца.
                for (int j = i + 1; j < size; ++j)
                    //Каждый элемент делим на a[i][i].
                    matrix[i][j] /= matrix[i][i];
                //Идем по столбцам.
                for (int j = 0; j < size; ++j)
                    //Проверяем.
                    if ((j != i) && (Math.Abs(matrix[j][i]) > EPS))
                        //Если да, то идем по k от i+1.
                        for (k = i + 1; k < size; ++k)
                            matrix[j][k] -= matrix[i][k] * matrix[j][i];
            }
            //Выводим результат.
            Console.WriteLine(Math.Round(det, 3));
            //Округляю, чтобы не было траблов с представлением вещественных.
        }

        /// <summary>
        /// Ниже реализован метод Гаусса для решения СЛАУ.
        /// </summary>
        public static void GaussMethod()
        {
            double s;
            Console.WriteLine("Введите размерность системы");
            int n;
            //Вводится размер матрицы.
            while (!int.TryParse(Console.ReadLine(), out n) || n > 10 || n < 1)
            {
                Console.WriteLine("Ошибка. Введите число от 1 до 10.");
            }
            //Обработка неверного ввода.
            double[][] coeff = new double[n][];
            double[] freeCoeff = new double[n];
            double[] x = new double[n];
            string[] str;
            //Создаются массивы с коэффициентами, свободными коэффициентами, решениями и строками.
            for (int i = 0; i < n; i++)
                x[i] = 0;
            Console.WriteLine("Введите построчно коэффициенты системы(не больше 100). Каждый элемент вводится с новой строки.");
            for (int i = 0; i < n; i++)
            {
                string line = Console.ReadLine();
                str = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                while (str.Length != n)
                {
                    Console.WriteLine("Ошибка ввода. Введите строку заново.");
                    line = Console.ReadLine();
                    str = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                }
                //Считываение строки, сплит в массив строк.
                coeff[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    if (!double.TryParse(str[j], out coeff[i][j]) || coeff[i][j] > 1000)
                    {
                        Console.WriteLine("Ошибка. Один или несколько из элементов невозможно преобразовать к числовому виду. " +
                                          "Введите строку заново.");
                        i--;
                        break;

                    }
                }
                //Преобразование массива строк в массив символов.
            }
            Console.WriteLine("Введите свободные коэффициенты(не больше 100). Каждый из них введите с новой строчки." +
                              " При попытке ввода чисел через пробел программа будет приниматься число без пробела.");
            for (int i = 0; i < n; i++)
            {
                double d;
                while (!double.TryParse(Console.ReadLine(), out d) || d > 100)
                {
                    Console.WriteLine("Ошибка. Введите число не превышающее 100.");
                }

                freeCoeff[i] = d;
            }
            //Ввод свободных коэффициентов.
            bool b = Determinant(coeff) != 0;

            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        coeff[i][j] = coeff[i][j] - coeff[k][j] * (coeff[i][k] / coeff[k][k]);
                    }
                    freeCoeff[i] = freeCoeff[i] - freeCoeff[k] * coeff[i][k] / coeff[k][k];
                }
            }
            //Реализация метода Гаусса.
            for (int k = n - 1; k >= 0; k--)
            {
                s = 0;
                for (int j = k + 1; j < n; j++)
                    s = s + coeff[k][j] * x[j];
                x[k] = (freeCoeff[k] - s) / coeff[k][k];
            }
            //Получение корней.
            if (b)
            {
                Console.WriteLine("Система имеет следующие корни");
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"x{i + 1}={x[i]}");
                }

            }
            //Если корни есть, то выводятся корни.
            else
            {
                string solution = "";
                for (int i = 0; i < n; i++)
                {
                    if (coeff[i][i] != 0)
                    {
                        solution = "Бесконечное число решений.";
                    }
                }
                if (solution == "") solution = "Нет решений.";
                Console.WriteLine(solution);
            }
            //Иначе выводится "Нет решений" или "Бесконечное число решений".

        }

        /// <summary>
        /// Умножение матрицы на число. 
        /// Тут все просто, ввод матрицы, ввод коэффициента, умножение поэлементно.
        /// </summary>
        public static void MatrixNumberMultiplication()
        {
            Console.WriteLine("Задание матрицы.");
            double[,] matrix = Generation();
            if (matrix.Length != 0)
            {
                Console.WriteLine(
                    "Введите число, на которое надо умножить матрицу. Число не может превышать 100 по модулю.");
                double number;
                while (!double.TryParse(Console.ReadLine(), out number) || number > 100 || number < -100)
                {
                    Console.WriteLine("Ошибка. Необходимо ввести число не превосходящее по модулю 100.");
                }
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] *= number;
                        matrix[i, j] = Math.Round(matrix[i, j], 3);
                    }
                }
                Output(matrix);
            }
        }


        /// <summary>
        /// Умножение матрицы на матрицу.
        /// </summary>
        public static void MatrixMultiplication()
        {
            Console.WriteLine("Задание первой матрицы.");
            double[,] matrixA = Generation();
            //Генерация первой матрицы.
            //Если она приняла ислам, то вызов экснепшена.
            if (matrixA.Length != 0)
            {
                Console.WriteLine("Задание второй матрицы.");
                double[,] matrixB = Generation();
                //Генерация второй матрицы.
                //Если она не приняла ислам, то работаем дальше.
                if (matrixB.Length != 0)
                {
                    if (matrixA.GetLength(1) == matrixB.GetLength(0))
                    {
                        double[,] matrixC = new double[matrixA.GetLength(0), matrixB.GetLength(1)];
                        for (int i = 0; i < matrixC.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrixC.GetLength(1); j++)
                            {
                                for (int k = 0; k < matrixA.GetLength(1); k++)
                                {
                                    matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                                }
                                matrixC[i, j] = Math.Round(matrixC[i, j]);
                            }
                        }
                        //Реализовано умножение матриц по определению. 
                        Console.WriteLine("Операция прошла успешно.");
                        Output(matrixC);
                    }
                    else
                    {
                        Console.WriteLine(
                            "Ошибка. Количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");
                    }
                }
                else
                {
                    Console.WriteLine("Так как вторая матрица представленна некорректно, то невозможно выполнить операцию.");
                }
            }
            else
            {
                Console.WriteLine("Так как первая матрица представленна некорректно, то невозможно выполнить операцию.");
            }

        }

        /// <summary>
        /// Разность матриц. 
        /// В методе <40 строк, так что комментов внутри не будет, думаю там и так все понятно.
        /// Обработаны все возможные ошибки, для каждого вызывается свой эксепшн.
        /// </summary>
        public static void MatrixDifference()
        {
            Console.WriteLine("Задание первой матрицы.");
            double[,] matrixA = Generation();
            if (matrixA.Length != 0)
            {
                Console.WriteLine("Задание второй матрицы.");
                double[,] matrixB = Generation();
                if (matrixB.Length != 0)
                {
                    if (matrixA.GetLength(0) == matrixB.GetLength(0) && matrixA.GetLength(1) == matrixB.GetLength(1))
                    {
                        double[,] matrixC = new double[matrixA.GetLength(0), matrixA.GetLength(1)];
                        for (int i = 0; i < matrixC.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrixC.GetLength(1); j++)
                            {
                                matrixC[i, j] = Math.Round(matrixA[i, j] - matrixB[i, j], 3);
                            }
                        }
                        Console.WriteLine("Операция прошла успешно.");
                        Output(matrixC);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка. Матрицы должны быть одного размера.");
                    }
                }
                else
                {
                    Console.WriteLine("Так как вторая матрица представленна некорректно, то невозможно выполнить операцию.");
                }
            }
            else
            {
                Console.WriteLine("Так как первая матрица представленна некорректно, то невозможно выполнить операцию.");
            }

        }

        /// <summary>
        /// Разность матриц. 
        /// В методе <40 строк, так что комментов внутри не будет, думаю там и так все понятно.
        /// Обработаны все возможные ошибки, для каждого вызывается свой эксепшн. 
        /// </summary>
        public static void MatrixSum()
        {
            Console.WriteLine("Задание первой матрицы.");
            double[,] matrixA = Generation();
            if (matrixA.Length != 0)
            {
                Console.WriteLine("Задание второй матрицы.");
                double[,] matrixB = Generation();
                if (matrixB.Length != 0)
                {
                    if (matrixA.GetLength(0) == matrixB.GetLength(0) && matrixA.GetLength(1) == matrixB.GetLength(1))
                    {
                        double[,] matrixC = new double[matrixA.GetLength(0), matrixA.GetLength(1)];
                        for (int i = 0; i < matrixC.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrixC.GetLength(1); j++)
                            {
                                matrixC[i, j] = Math.Round(matrixA[i, j] + matrixB[i, j], 3);
                            }
                        }
                        Console.WriteLine("Операция прошла успешно.");
                        Output(matrixC);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка. Матрицы должны быть одного размера.");
                    }
                }
                else
                {
                    Console.WriteLine("Так как вторая матрица представленна некорректно, то невозможно выполнить операцию.");
                }
            }
            else
            {
                Console.WriteLine("Так как первая матрица представленна некорректно, то невозможно выполнить операцию.");
            }
        }

        /// <summary>
        /// Транспонирование матрицы. Создается матрица зеркального размера, заполняется тоже зеркально.
        /// Получаенная матрица и является ответом.
        /// </summary>
        public static void MatrixTransposition()
        {
            double[,] matrix = Generation();
            if (matrix.Length != 0)
            {
                double[,] matrixT = new double[matrix.GetLength(1), matrix.GetLength(0)];
                for (int i = 0; i < matrixT.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixT.GetLength(1); j++)
                    {
                        matrixT[i, j] = matrix[j, i];
                    }
                }

                Console.WriteLine("Транспонирование прошло успешно.");
                Output(matrixT);
            }
        }

        /// <summary>
        /// След матрицы. По определению след матрицы - сумма элементов главной диагонали, так что просто прохожу по ним и суммирую.
        /// </summary>
        public static void MatrixTrace()
        {
            double[,] matrix = Generation();
            if (matrix.Length != 0)
            {
                if (matrix.GetLength(0) != matrix.GetLength(1))
                {
                    Console.WriteLine("Невозможно вычислить след. Матрица должна быть квадратной.");
                }
                else
                {
                    double sum = 0;
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        sum += matrix[i, i];
                    }
                    Console.WriteLine($"След данной матрицы равен {sum}.");
                }
            }
        }
    }
}
