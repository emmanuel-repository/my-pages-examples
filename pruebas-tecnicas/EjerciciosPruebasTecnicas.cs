using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Ejercicios comunes de pruebas técnicas — versión C# (.NET 6+)
/// Ejecutar:
///   dotnet run          (dentro de un proyecto console)
/// o compilar el archivo en un proyecto creado con: dotnet new console
/// </summary>
public static class EjerciciosPruebasTecnicas
{
    /* ---------- 1. Strings ---------- */

    public static bool EsPalindromo(string str)
    {
        string limpio = Regex.Replace(str.ToLower(), "[^a-z0-9ñ]", "");
        char[] chars = limpio.ToCharArray();
        Array.Reverse(chars);
        return limpio == new string(chars);
    }

    public static Dictionary<char, int> ContarCaracteres(string str)
    {
        var conteo = new Dictionary<char, int>();
        foreach (char c in str)
            conteo[c] = conteo.TryGetValue(c, out int v) ? v + 1 : 1;
        return conteo;
    }

    public static string InvertirString(string str)
    {
        var resultado = new StringBuilder();
        for (int i = str.Length - 1; i >= 0; i--)
            resultado.Append(str[i]);
        return resultado.ToString();
    }

    public static bool SonAnagramas(string a, string b)
    {
        static string Normalizar(string s) =>
            new string(s.ToLower().OrderBy(c => c).ToArray());
        return Normalizar(a) == Normalizar(b);
    }

    public static string FizzBuzz(int n)
    {
        var lineas = new List<string>();
        for (int i = 1; i <= n; i++)
        {
            if (i % 15 == 0) lineas.Add("FizzBuzz");
            else if (i % 3 == 0) lineas.Add("Fizz");
            else if (i % 5 == 0) lineas.Add("Buzz");
            else lineas.Add(i.ToString());
        }
        return string.Join(", ", lineas);
    }

    /* ---------- 2. Arrays ---------- */

    public static int Maximo(int[] arr)
    {
        int max = arr[0];
        foreach (int n in arr) if (n > max) max = n;
        return max;
    }

    public static List<int> EliminarDuplicados(int[] arr)
    {
        var set = new HashSet<int>();
        var resultado = new List<int>();
        foreach (int n in arr)
            if (set.Add(n)) resultado.Add(n);
        return resultado;
    }

    public static int[] BubbleSort(int[] arr)
    {
        int[] copia = (int[])arr.Clone();
        for (int i = 0; i < copia.Length; i++)
        {
            for (int j = 0; j < copia.Length - i - 1; j++)
            {
                if (copia[j] > copia[j + 1])
                    (copia[j], copia[j + 1]) = (copia[j + 1], copia[j]);
            }
        }
        return copia;
    }

    public static int[]? TwoSum(int[] arr, int objetivo)
    {
        var vistos = new Dictionary<int, int>();
        for (int i = 0; i < arr.Length; i++)
        {
            int complemento = objetivo - arr[i];
            if (vistos.TryGetValue(complemento, out int idx))
                return new[] { idx, i };
            vistos[arr[i]] = i;
        }
        return null;
    }

    // Flatten: aplana una lista anidada (object puede ser int o List<object>)
    public static List<object> Flatten(List<object> arr)
    {
        var resultado = new List<object>();
        foreach (object val in arr)
        {
            if (val is List<object> lista)
                resultado.AddRange(Flatten(lista));
            else
                resultado.Add(val);
        }
        return resultado;
    }

    // GroupBy propio: agrupa una lista por el valor de una propiedad (selector)
    public static Dictionary<TKey, List<T>> MiGroupBy<T, TKey>(List<T> arr, Func<T, TKey> selector)
        where TKey : notnull
    {
        var acc = new Dictionary<TKey, List<T>>();
        foreach (T item in arr)
        {
            TKey key = selector(item);
            if (!acc.ContainsKey(key)) acc[key] = new List<T>();
            acc[key].Add(item);
        }
        return acc;
    }

    /* ---------- 3. Lógica / matemáticas ---------- */

    public static long FibonacciIterativo(int n)
    {
        long a = 0, b = 1;
        for (int i = 0; i < n; i++)
            (a, b) = (b, a + b);
        return a;
    }

    public static long FibonacciRecursivo(int n)
    {
        if (n <= 1) return n;
        return FibonacciRecursivo(n - 1) + FibonacciRecursivo(n - 2);
    }

    public static bool EsPrimo(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i <= Math.Sqrt(n); i++)
            if (n % i == 0) return false;
        return true;
    }

    public static List<int> PrimerosPrimos(int n)
    {
        var primos = new List<int>();
        int num = 2;
        while (primos.Count < n)
        {
            if (EsPrimo(num)) primos.Add(num);
            num++;
        }
        return primos;
    }

    public static long Factorial(int n) =>
        n <= 1 ? 1 : n * Factorial(n - 1);

    /* ---------- 4. Objetos / estructuras ---------- */

    private static readonly Regex EmailRegex =
        new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);

    public static bool EsEmailValido(string email) => EmailRegex.IsMatch(email);

    public static string CreatePassword(int longitud, bool isMayus, bool withNum, bool withSimbolos)
    {
        const string minusculas = "abcdefghijklmnñopqrstuvwxyz";
        string mayusculas = minusculas.ToUpper();
        const string numeros = "0123456789";
        const string simbolos = "!@$%)*(#+";
        string pool = minusculas;
        if (isMayus) pool += mayusculas;
        if (withNum) pool += numeros;
        if (withSimbolos) pool += simbolos;

        var random = new Random();
        var password = new StringBuilder();
        for (int i = 0; i < longitud; i++)
            password.Append(pool[random.Next(pool.Length)]);
        return password.ToString();
    }

    // Debounce: retrasa la ejecución hasta que pasen `delayMs` sin nuevas llamadas
    public static Action Debounce(Action fn, int delayMs)
    {
        CancellationTokenSource? cts = null;
        return () =>
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            var token = cts.Token;
            Task.Delay(delayMs, token).ContinueWith(t =>
            {
                if (!t.IsCanceled) fn();
            }, TaskScheduler.Default);
        };
    }

    // Throttle: ejecuta como máximo una vez cada `limitMs`
    public static Action Throttle(Action fn, int limitMs)
    {
        long ultimaEjecucion = 0;
        return () =>
        {
            long ahora = Environment.TickCount64;
            if (ahora - Interlocked.Read(ref ultimaEjecucion) >= limitMs)
            {
                Interlocked.Exchange(ref ultimaEjecucion, ahora);
                fn();
            }
        };
    }

    public static List<TR> MiMap<T, TR>(List<T> arr, Func<T, TR> callback)
    {
        var resultado = new List<TR>();
        foreach (T item in arr) resultado.Add(callback(item));
        return resultado;
    }

    public static List<T> MiFilter<T>(List<T> arr, Func<T, bool> callback)
    {
        var resultado = new List<T>();
        foreach (T item in arr)
            if (callback(item)) resultado.Add(item);
        return resultado;
    }

    public static TA MiReduce<T, TA>(List<T> arr, Func<TA, T, TA> callback, TA inicial)
    {
        TA acumulador = inicial;
        foreach (T item in arr) acumulador = callback(acumulador, item);
        return acumulador;
    }

    // DeepClone: clona recursivamente Dictionary / List / valores
    public static object? DeepClone(object? obj)
    {
        if (obj is Dictionary<string, object?> mapa)
        {
            var clon = new Dictionary<string, object?>();
            foreach (var kv in mapa) clon[kv.Key] = DeepClone(kv.Value);
            return clon;
        }
        if (obj is List<object?> lista)
        {
            var clon = new List<object?>();
            foreach (var item in lista) clon.Add(DeepClone(item));
            return clon;
        }
        return obj; // tipos por valor y strings (inmutables)
    }

    /* ---------- 5. Async (equivalente a Promises con Task) ---------- */

    public static Task Delay(int ms) => Task.Delay(ms);

    // Equivalente a un Promise.all propio (sin usar Task.WhenAll)
    public static Task<List<T>> MiPromiseAll<T>(List<Task<T>> tareas)
    {
        var tcs = new TaskCompletionSource<List<T>>();
        var resultados = new T[tareas.Count];
        int completadas = 0;

        if (tareas.Count == 0)
        {
            tcs.SetResult(new List<T>());
            return tcs.Task;
        }

        for (int i = 0; i < tareas.Count; i++)
        {
            int index = i;
            tareas[i].ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    tcs.TrySetException(t.Exception!.InnerExceptions);
                    return;
                }
                resultados[index] = t.Result;
                if (Interlocked.Increment(ref completadas) == tareas.Count)
                    tcs.TrySetResult(resultados.ToList());
            }, TaskScheduler.Default);
        }
        return tcs.Task;
    }

    /* ---------- 6. Estructuras de datos ---------- */

    public class Pila<T>
    {
        private readonly List<T> items = new();
        public void Push(T item) => items.Add(item);
        public T? Pop()
        {
            if (items.Count == 0) return default;
            T item = items[^1];
            items.RemoveAt(items.Count - 1);
            return item;
        }
        public T? Peek() => items.Count == 0 ? default : items[^1];
        public bool EstaVacia() => items.Count == 0;
    }

    public class Cola<T>
    {
        private readonly List<T> items = new();
        public void Enqueue(T item) => items.Add(item);
        public T? Dequeue()
        {
            if (items.Count == 0) return default;
            T item = items[0];
            items.RemoveAt(0);
            return item;
        }
        public T? Frente() => items.Count == 0 ? default : items[0];
        public bool EstaVacia() => items.Count == 0;
    }

    public static int BusquedaBinaria(int[] arr, int objetivo)
    {
        int inicio = 0, fin = arr.Length - 1;
        while (inicio <= fin)
        {
            int medio = (inicio + fin) / 2;
            if (arr[medio] == objetivo) return medio;
            if (arr[medio] < objetivo) inicio = medio + 1;
            else fin = medio - 1;
        }
        return -1;
    }

    public class Nodo
    {
        public int Valor;
        public Nodo? Siguiente;
        public Nodo(int valor) => Valor = valor;
    }

    public class ListaEnlazada
    {
        public Nodo? Cabeza;
        public void Agregar(int valor)
        {
            var nodo = new Nodo(valor);
            if (Cabeza == null) { Cabeza = nodo; return; }
            Nodo actual = Cabeza;
            while (actual.Siguiente != null) actual = actual.Siguiente;
            actual.Siguiente = nodo;
        }
        public List<int> Recorrer()
        {
            var valores = new List<int>();
            Nodo? actual = Cabeza;
            while (actual != null) { valores.Add(actual.Valor); actual = actual.Siguiente; }
            return valores;
        }
    }

    /* ---------- Demos ---------- */

    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== 1. Strings ===");
        Console.WriteLine($"EsPalindromo(\"Anita lava la tina\") → {EsPalindromo("Anita lava la tina")}");
        Console.WriteLine($"EsPalindromo(\"hola mundo\") → {EsPalindromo("hola mundo")}");
        Console.WriteLine($"ContarCaracteres(\"banana\") → {JsonSerializer.Serialize(ContarCaracteres("banana").ToDictionary(kv => kv.Key.ToString(), kv => kv.Value))}");
        Console.WriteLine($"InvertirString(\"hola\") → {InvertirString("hola")}");
        Console.WriteLine($"SonAnagramas(\"roma\", \"amor\") → {SonAnagramas("roma", "amor")}");
        Console.WriteLine($"SonAnagramas(\"hola\", \"chao\") → {SonAnagramas("hola", "chao")}");
        Console.WriteLine($"FizzBuzz(20) → {FizzBuzz(20)}");

        Console.WriteLine("\n=== 2. Arrays ===");
        Console.WriteLine($"Maximo([3,7,2,9,4]) → {Maximo(new[] { 3, 7, 2, 9, 4 })}");
        Console.WriteLine($"EliminarDuplicados([1,2,2,3,3,3]) → [{string.Join(", ", EliminarDuplicados(new[] { 1, 2, 2, 3, 3, 3 }))}]");
        Console.WriteLine($"BubbleSort([5,3,8,1,2]) → [{string.Join(", ", BubbleSort(new[] { 5, 3, 8, 1, 2 }))}]");
        Console.WriteLine($"TwoSum([2,7,11,15], 9) → [{string.Join(", ", TwoSum(new[] { 2, 7, 11, 15 }, 9) ?? Array.Empty<int>())}]");

        var anidado = new List<object> { 1, new List<object> { 2, 3, new List<object> { 4, 5 } }, 6 };
        Console.WriteLine($"Flatten([1,[2,3,[4,5]],6]) → [{string.Join(", ", Flatten(anidado))}]");

        var personas = new List<(string Nombre, int Edad)>
        {
            ("Ana", 25), ("Luis", 25), ("Eva", 30)
        };
        var agrupado = MiGroupBy(personas, p => p.Edad);
        foreach (var kv in agrupado)
            Console.WriteLine($"GroupBy edad {kv.Key} → [{string.Join(", ", kv.Value.Select(p => p.Nombre))}]");

        Console.WriteLine("\n=== 3. Lógica / matemáticas ===");
        Console.WriteLine($"FibonacciIterativo(10) → {FibonacciIterativo(10)}");
        Console.WriteLine($"FibonacciRecursivo(10) → {FibonacciRecursivo(10)}");
        Console.WriteLine($"EsPrimo(17) → {EsPrimo(17)}");
        Console.WriteLine($"PrimerosPrimos(5) → [{string.Join(", ", PrimerosPrimos(5))}]");
        Console.WriteLine($"Factorial(5) → {Factorial(5)}");

        Console.WriteLine("\n=== 4. Objetos / estructuras ===");
        Console.WriteLine($"EsEmailValido(\"correo@dominio.com\") → {EsEmailValido("correo@dominio.com")}");
        Console.WriteLine($"EsEmailValido(\"correo-invalido\") → {EsEmailValido("correo-invalido")}");
        Console.WriteLine($"CreatePassword(6, true, true, true) → {CreatePassword(6, true, true, true)}");
        Console.WriteLine($"CreatePassword(12, false, true, false) → {CreatePassword(12, false, true, false)}");
        Console.WriteLine($"CreatePassword(10, true, false, true) → {CreatePassword(10, true, false, true)}");

        Console.WriteLine($"MiMap([1,2,3], x => x*2) → [{string.Join(", ", MiMap(new List<int> { 1, 2, 3 }, x => x * 2))}]");
        Console.WriteLine($"MiFilter([1,2,3,4], x => x%2==0) → [{string.Join(", ", MiFilter(new List<int> { 1, 2, 3, 4 }, x => x % 2 == 0))}]");
        Console.WriteLine($"MiReduce([1,2,3,4], (a,x)=>a+x, 0) → {MiReduce(new List<int> { 1, 2, 3, 4 }, (a, x) => a + x, 0)}");

        var original = new Dictionary<string, object?>
        {
            ["a"] = 1,
            ["b"] = new Dictionary<string, object?>
            {
                ["c"] = 2,
                ["d"] = new List<object?> { 3, 4 }
            }
        };
        var copia = (Dictionary<string, object?>)DeepClone(original)!;
        ((Dictionary<string, object?>)copia["b"]!)["c"] = 999;
        Console.WriteLine($"DeepClone: original.b.c → {((Dictionary<string, object?>)original["b"]!)["c"]} | copia.b.c → {((Dictionary<string, object?>)copia["b"]!)["c"]}");

        // Demo debounce / throttle
        var buscarDebounced = Debounce(() => Console.WriteLine("  [debounce] búsqueda ejecutada (1 vez tras 5 llamadas)"), 200);
        for (int i = 0; i < 5; i++) buscarDebounced();
        var scrollThrottled = Throttle(() => Console.WriteLine("  [throttle] evento procesado"), 200);
        for (int i = 0; i < 5; i++) scrollThrottled(); // solo la primera pasa
        await Task.Delay(400); // esperar a que el debounce dispare

        Console.WriteLine("\n=== 5. Async (Task, equivalente a Promises) ===");
        Console.WriteLine("inicio...");
        await Delay(1000);
        Console.WriteLine("después de 1 segundo ✅");

        Task<string> p1 = Delay(300).ContinueWith(_ => "A");
        Task<string> p2 = Delay(100).ContinueWith(_ => "B");
        Task<string> p3 = Delay(200).ContinueWith(_ => "C");
        var resultado = await MiPromiseAll(new List<Task<string>> { p1, p2, p3 });
        Console.WriteLine($"MiPromiseAll([p1,p2,p3]) → [{string.Join(", ", resultado)}]");

        Console.WriteLine("\n=== 6. Estructuras de datos ===");
        var pila = new Pila<int>();
        pila.Push(1); pila.Push(2); pila.Push(3);
        Console.WriteLine($"Pila: Push 1,2,3 → Pop() → {pila.Pop()} | Peek() → {pila.Peek()}");

        var cola = new Cola<string>();
        cola.Enqueue("a"); cola.Enqueue("b"); cola.Enqueue("c");
        Console.WriteLine($"Cola: Enqueue a,b,c → Dequeue() → {cola.Dequeue()} | Frente() → {cola.Frente()}");

        Console.WriteLine($"BusquedaBinaria([1,3,5,7,9,11], 7) → {BusquedaBinaria(new[] { 1, 3, 5, 7, 9, 11 }, 7)}");

        var lista = new ListaEnlazada();
        lista.Agregar(10); lista.Agregar(20); lista.Agregar(30);
        Console.WriteLine($"ListaEnlazada.Recorrer() → [{string.Join(", ", lista.Recorrer())}]");
    }
}
