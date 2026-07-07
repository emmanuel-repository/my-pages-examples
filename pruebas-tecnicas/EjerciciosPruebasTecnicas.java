import java.util.*;
import java.util.concurrent.*;
import java.util.function.*;
import java.util.regex.Pattern;

/**
 * Ejercicios comunes de pruebas técnicas — versión Java
 * Compilar y ejecutar:
 *   javac EjerciciosPruebasTecnicas.java
 *   java EjerciciosPruebasTecnicas
 */
public class EjerciciosPruebasTecnicas {

    /* ---------- 1. Strings ---------- */

    static boolean esPalindromo(String str) {
        String limpio = str.toLowerCase().replaceAll("[^a-z0-9ñ]", "");
        return limpio.equals(new StringBuilder(limpio).reverse().toString());
    }

    static Map<Character, Integer> contarCaracteres(String str) {
        Map<Character, Integer> conteo = new LinkedHashMap<>();
        for (char c : str.toCharArray()) {
            conteo.merge(c, 1, Integer::sum);
        }
        return conteo;
    }

    static String invertirString(String str) {
        String resultado = "";
        for (int i = str.length() - 1; i >= 0; i--) {
            resultado += str.charAt(i);
        }
        return resultado;
    }

    static boolean sonAnagramas(String a, String b) {
        char[] ca = a.toLowerCase().toCharArray();
        char[] cb = b.toLowerCase().toCharArray();
        Arrays.sort(ca);
        Arrays.sort(cb);
        return Arrays.equals(ca, cb);
    }

    static String fizzBuzz(int n) {
        List<String> lineas = new ArrayList<>();
        for (int i = 1; i <= n; i++) {
            if (i % 15 == 0) lineas.add("FizzBuzz");
            else if (i % 3 == 0) lineas.add("Fizz");
            else if (i % 5 == 0) lineas.add("Buzz");
            else lineas.add(String.valueOf(i));
        }
        return String.join(", ", lineas);
    }

    /* ---------- 2. Arrays ---------- */

    static int maximo(int[] arr) {
        int max = arr[0];
        for (int n : arr) if (n > max) max = n;
        return max;
    }

    static List<Integer> eliminarDuplicados(int[] arr) {
        Set<Integer> set = new LinkedHashSet<>();
        for (int n : arr) set.add(n);
        return new ArrayList<>(set);
    }

    static int[] bubbleSort(int[] arr) {
        int[] copia = Arrays.copyOf(arr, arr.length);
        for (int i = 0; i < copia.length; i++) {
            for (int j = 0; j < copia.length - i - 1; j++) {
                if (copia[j] > copia[j + 1]) {
                    int tmp = copia[j];
                    copia[j] = copia[j + 1];
                    copia[j + 1] = tmp;
                }
            }
        }
        return copia;
    }

    static int[] twoSum(int[] arr, int objetivo) {
        Map<Integer, Integer> vistos = new HashMap<>();
        for (int i = 0; i < arr.length; i++) {
            int complemento = objetivo - arr[i];
            if (vistos.containsKey(complemento)) {
                return new int[]{vistos.get(complemento), i};
            }
            vistos.put(arr[i], i);
        }
        return null;
    }

    // flatten: aplana una lista anidada (List que puede contener List o Integer)
    static List<Object> flatten(List<?> arr) {
        List<Object> resultado = new ArrayList<>();
        for (Object val : arr) {
            if (val instanceof List<?> lista) {
                resultado.addAll(flatten(lista));
            } else {
                resultado.add(val);
            }
        }
        return resultado;
    }

    // groupBy: agrupa una lista de mapas por el valor de una propiedad
    static Map<Object, List<Map<String, Object>>> groupBy(List<Map<String, Object>> arr, String prop) {
        Map<Object, List<Map<String, Object>>> acc = new LinkedHashMap<>();
        for (Map<String, Object> item : arr) {
            Object key = item.get(prop);
            acc.computeIfAbsent(key, k -> new ArrayList<>()).add(item);
        }
        return acc;
    }

    /* ---------- 3. Lógica / matemáticas ---------- */

    static long fibonacciIterativo(int n) {
        long a = 0, b = 1;
        for (int i = 0; i < n; i++) {
            long temp = a;
            a = b;
            b = temp + b;
        }
        return a;
    }

    static long fibonacciRecursivo(int n) {
        if (n <= 1) return n;
        return fibonacciRecursivo(n - 1) + fibonacciRecursivo(n - 2);
    }

    static boolean esPrimo(int n) {
        if (n < 2) return false;
        for (int i = 2; i <= Math.sqrt(n); i++) {
            if (n % i == 0) return false;
        }
        return true;
    }

    static List<Integer> primerosPrimos(int n) {
        List<Integer> primos = new ArrayList<>();
        int num = 2;
        while (primos.size() < n) {
            if (esPrimo(num)) primos.add(num);
            num++;
        }
        return primos;
    }

    static long factorial(int n) {
        return n <= 1 ? 1 : n * factorial(n - 1);
    }

    /* ---------- 4. Objetos / estructuras ---------- */

    static final Pattern EMAIL_REGEX = Pattern.compile("^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$");

    static boolean esEmailValido(String email) {
        return EMAIL_REGEX.matcher(email).matches();
    }

    static String createPassword(int longitud, boolean isMayus, boolean withNum, boolean withSimbolos) {
        String minusculas = "abcdefghijklmnñopqrstuvwxyz";
        String mayusculas = minusculas.toUpperCase();
        String numeros = "0123456789";
        String simbolos = "!@$%)*(#+";
        StringBuilder pool = new StringBuilder(minusculas);
        if (isMayus) pool.append(mayusculas);
        if (withNum) pool.append(numeros);
        if (withSimbolos) pool.append(simbolos);
        StringBuilder password = new StringBuilder();
        Random random = new Random();
        for (int i = 0; i < longitud; i++) {
            password.append(pool.charAt(random.nextInt(pool.length())));
        }
        return password.toString();
    }

    // debounce: retrasa la ejecución hasta que pasen `delayMs` sin nuevas llamadas
    static Runnable debounce(Runnable fn, long delayMs, ScheduledExecutorService scheduler) {
        final ScheduledFuture<?>[] pendiente = new ScheduledFuture<?>[1];
        return () -> {
            if (pendiente[0] != null) pendiente[0].cancel(false);
            pendiente[0] = scheduler.schedule(fn, delayMs, TimeUnit.MILLISECONDS);
        };
    }

    // throttle: ejecuta como máximo una vez cada `limitMs`
    static Runnable throttle(Runnable fn, long limitMs) {
        final long[] ultimaEjecucion = {0};
        return () -> {
            long ahora = System.currentTimeMillis();
            if (ahora - ultimaEjecucion[0] >= limitMs) {
                ultimaEjecucion[0] = ahora;
                fn.run();
            }
        };
    }

    static <T, R> List<R> miMap(List<T> arr, Function<T, R> callback) {
        List<R> resultado = new ArrayList<>();
        for (T item : arr) resultado.add(callback.apply(item));
        return resultado;
    }

    static <T> List<T> miFilter(List<T> arr, Predicate<T> callback) {
        List<T> resultado = new ArrayList<>();
        for (T item : arr) if (callback.test(item)) resultado.add(item);
        return resultado;
    }

    static <T, A> A miReduce(List<T> arr, BiFunction<A, T, A> callback, A inicial) {
        A acumulador = inicial;
        for (T item : arr) acumulador = callback.apply(acumulador, item);
        return acumulador;
    }

    // deepClone: clona recursivamente estructuras de Map / List / valores
    @SuppressWarnings("unchecked")
    static Object deepClone(Object obj) {
        if (obj instanceof Map<?, ?> mapa) {
            Map<Object, Object> clon = new LinkedHashMap<>();
            for (Map.Entry<?, ?> e : mapa.entrySet()) {
                clon.put(e.getKey(), deepClone(e.getValue()));
            }
            return clon;
        }
        if (obj instanceof List<?> lista) {
            List<Object> clon = new ArrayList<>();
            for (Object item : lista) clon.add(deepClone(item));
            return clon;
        }
        return obj; // tipos inmutables (String, Integer, etc.)
    }

    /* ---------- 5. Async (equivalente a Promises) ---------- */

    static CompletableFuture<Void> delay(long ms) {
        CompletableFuture<Void> future = new CompletableFuture<>();
        CompletableFuture.delayedExecutor(ms, TimeUnit.MILLISECONDS)
                .execute(() -> future.complete(null));
        return future;
    }

    // Equivalente a un Promise.all propio
    static <T> CompletableFuture<List<T>> miPromiseAll(List<CompletableFuture<T>> futures) {
        CompletableFuture<List<T>> resultado = new CompletableFuture<>();
        List<T> resultados = new ArrayList<>(Collections.nCopies(futures.size(), null));
        int[] completadas = {0};
        if (futures.isEmpty()) {
            resultado.complete(resultados);
            return resultado;
        }
        for (int i = 0; i < futures.size(); i++) {
            final int index = i;
            futures.get(i).whenComplete((valor, error) -> {
                if (error != null) {
                    resultado.completeExceptionally(error);
                    return;
                }
                synchronized (resultados) {
                    resultados.set(index, valor);
                    completadas[0]++;
                    if (completadas[0] == futures.size()) resultado.complete(resultados);
                }
            });
        }
        return resultado;
    }

    /* ---------- 6. Estructuras de datos ---------- */

    static class Pila<T> {
        private final List<T> items = new ArrayList<>();
        void push(T item) { items.add(item); }
        T pop() { return items.isEmpty() ? null : items.remove(items.size() - 1); }
        T peek() { return items.isEmpty() ? null : items.get(items.size() - 1); }
        boolean estaVacia() { return items.isEmpty(); }
    }

    static class Cola<T> {
        private final List<T> items = new ArrayList<>();
        void enqueue(T item) { items.add(item); }
        T dequeue() { return items.isEmpty() ? null : items.remove(0); }
        T frente() { return items.isEmpty() ? null : items.get(0); }
        boolean estaVacia() { return items.isEmpty(); }
    }

    static int busquedaBinaria(int[] arr, int objetivo) {
        int inicio = 0, fin = arr.length - 1;
        while (inicio <= fin) {
            int medio = (inicio + fin) / 2;
            if (arr[medio] == objetivo) return medio;
            if (arr[medio] < objetivo) inicio = medio + 1;
            else fin = medio - 1;
        }
        return -1;
    }

    static class Nodo {
        int valor;
        Nodo siguiente;
        Nodo(int valor) { this.valor = valor; }
    }

    static class ListaEnlazada {
        Nodo cabeza;
        void agregar(int valor) {
            Nodo nodo = new Nodo(valor);
            if (cabeza == null) { cabeza = nodo; return; }
            Nodo actual = cabeza;
            while (actual.siguiente != null) actual = actual.siguiente;
            actual.siguiente = nodo;
        }
        List<Integer> recorrer() {
            List<Integer> valores = new ArrayList<>();
            Nodo actual = cabeza;
            while (actual != null) { valores.add(actual.valor); actual = actual.siguiente; }
            return valores;
        }
    }

    /* ---------- Demos ---------- */

    public static void main(String[] args) throws Exception {
        System.out.println("=== 1. Strings ===");
        System.out.println("esPalindromo(\"Anita lava la tina\") → " + esPalindromo("Anita lava la tina"));
        System.out.println("esPalindromo(\"hola mundo\") → " + esPalindromo("hola mundo"));
        System.out.println("contarCaracteres(\"banana\") → " + contarCaracteres("banana"));
        System.out.println("invertirString(\"hola\") → " + invertirString("hola"));
        System.out.println("sonAnagramas(\"roma\", \"amor\") → " + sonAnagramas("roma", "amor"));
        System.out.println("sonAnagramas(\"hola\", \"chao\") → " + sonAnagramas("hola", "chao"));
        System.out.println("fizzBuzz(20) → " + fizzBuzz(20));

        System.out.println("\n=== 2. Arrays ===");
        System.out.println("maximo([3,7,2,9,4]) → " + maximo(new int[]{3, 7, 2, 9, 4}));
        System.out.println("eliminarDuplicados([1,2,2,3,3,3]) → " + eliminarDuplicados(new int[]{1, 2, 2, 3, 3, 3}));
        System.out.println("bubbleSort([5,3,8,1,2]) → " + Arrays.toString(bubbleSort(new int[]{5, 3, 8, 1, 2})));
        System.out.println("twoSum([2,7,11,15], 9) → " + Arrays.toString(twoSum(new int[]{2, 7, 11, 15}, 9)));
        System.out.println("flatten([1,[2,3,[4,5]],6]) → " +
                flatten(List.of(1, List.of(2, 3, List.of(4, 5)), 6)));

        List<Map<String, Object>> personas = List.of(
                new LinkedHashMap<>(Map.of("nombre", "Ana", "edad", 25)),
                new LinkedHashMap<>(Map.of("nombre", "Luis", "edad", 25)),
                new LinkedHashMap<>(Map.of("nombre", "Eva", "edad", 30))
        );
        System.out.println("groupBy(personas, \"edad\") → " + groupBy(personas, "edad"));

        System.out.println("\n=== 3. Lógica / matemáticas ===");
        System.out.println("fibonacciIterativo(10) → " + fibonacciIterativo(10));
        System.out.println("fibonacciRecursivo(10) → " + fibonacciRecursivo(10));
        System.out.println("esPrimo(17) → " + esPrimo(17));
        System.out.println("primerosPrimos(5) → " + primerosPrimos(5));
        System.out.println("factorial(5) → " + factorial(5));

        System.out.println("\n=== 4. Objetos / estructuras ===");
        System.out.println("esEmailValido(\"correo@dominio.com\") → " + esEmailValido("correo@dominio.com"));
        System.out.println("esEmailValido(\"correo-invalido\") → " + esEmailValido("correo-invalido"));
        System.out.println("createPassword(6, true, true, true) → " + createPassword(6, true, true, true));
        System.out.println("createPassword(12, false, true, false) → " + createPassword(12, false, true, false));
        System.out.println("createPassword(10, true, false, true) → " + createPassword(10, true, false, true));

        System.out.println("miMap([1,2,3], x -> x*2) → " + miMap(List.of(1, 2, 3), x -> x * 2));
        System.out.println("miFilter([1,2,3,4], x -> x%2==0) → " + miFilter(List.of(1, 2, 3, 4), x -> x % 2 == 0));
        System.out.println("miReduce([1,2,3,4], (a,x)->a+x, 0) → " + miReduce(List.of(1, 2, 3, 4), (a, x) -> a + x, 0));

        Map<String, Object> original = new LinkedHashMap<>();
        original.put("a", 1);
        Map<String, Object> b = new LinkedHashMap<>();
        b.put("c", 2);
        b.put("d", new ArrayList<>(List.of(3, 4)));
        original.put("b", b);
        @SuppressWarnings("unchecked")
        Map<String, Object> copia = (Map<String, Object>) deepClone(original);
        ((Map<String, Object>) copia.get("b")).put("c", 999);
        System.out.println("deepClone: original.b.c → " + ((Map<?, ?>) original.get("b")).get("c")
                + " | copia.b.c → " + ((Map<?, ?>) copia.get("b")).get("c"));

        // Demo debounce / throttle
        ScheduledExecutorService scheduler = Executors.newSingleThreadScheduledExecutor();
        Runnable buscarDebounced = debounce(() -> System.out.println("  [debounce] búsqueda ejecutada (1 vez tras 5 llamadas)"), 200, scheduler);
        for (int i = 0; i < 5; i++) buscarDebounced.run();
        Runnable scrollThrottled = throttle(() -> System.out.println("  [throttle] evento procesado"), 200);
        for (int i = 0; i < 5; i++) scrollThrottled.run(); // solo la primera pasa
        Thread.sleep(400); // esperar a que el debounce dispare
        scheduler.shutdown();

        System.out.println("\n=== 5. Async (CompletableFuture) ===");
        System.out.println("inicio...");
        delay(1000).join();
        System.out.println("después de 1 segundo ✅");

        CompletableFuture<String> p1 = delay(300).thenApply(v -> "A");
        CompletableFuture<String> p2 = delay(100).thenApply(v -> "B");
        CompletableFuture<String> p3 = delay(200).thenApply(v -> "C");
        System.out.println("miPromiseAll([p1,p2,p3]) → " + miPromiseAll(List.of(p1, p2, p3)).join());

        System.out.println("\n=== 6. Estructuras de datos ===");
        Pila<Integer> pila = new Pila<>();
        pila.push(1); pila.push(2); pila.push(3);
        System.out.println("Pila: push 1,2,3 → pop() → " + pila.pop() + " | peek() → " + pila.peek());

        Cola<String> cola = new Cola<>();
        cola.enqueue("a"); cola.enqueue("b"); cola.enqueue("c");
        System.out.println("Cola: enqueue a,b,c → dequeue() → " + cola.dequeue() + " | frente() → " + cola.frente());

        System.out.println("busquedaBinaria([1,3,5,7,9,11], 7) → " + busquedaBinaria(new int[]{1, 3, 5, 7, 9, 11}, 7));

        ListaEnlazada lista = new ListaEnlazada();
        lista.agregar(10); lista.agregar(20); lista.agregar(30);
        System.out.println("ListaEnlazada.recorrer() → " + lista.recorrer());
    }
}
