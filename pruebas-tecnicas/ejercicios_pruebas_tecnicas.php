<?php
/**
 * Ejercicios comunes de pruebas técnicas — versión PHP (8.0+)
 * Ejecutar en terminal:
 *   php ejercicios_pruebas_tecnicas.php
 */

/* ---------- 1. Strings ---------- */

function esPalindromo(string $str): bool {
    $limpio = preg_replace('/[^a-z0-9ñ]/u', '', mb_strtolower($str));
    // strrev no es seguro con multibyte (ñ), invertimos por caracteres
    $invertido = implode('', array_reverse(mb_str_split($limpio)));
    return $limpio === $invertido;
}

function contarCaracteres(string $str): array {
    $conteo = [];
    foreach (mb_str_split($str) as $char) {
        $conteo[$char] = ($conteo[$char] ?? 0) + 1;
    }
    return $conteo;
}

function invertirString(string $str): string {
    $resultado = '';
    $chars = mb_str_split($str);
    for ($i = count($chars) - 1; $i >= 0; $i--) {
        $resultado .= $chars[$i];
    }
    return $resultado;
}

function sonAnagramas(string $a, string $b): bool {
    $normalizar = function (string $str): string {
        $chars = mb_str_split(mb_strtolower($str));
        sort($chars);
        return implode('', $chars);
    };
    return $normalizar($a) === $normalizar($b);
}

function fizzBuzz(int $n): string {
    $lineas = [];
    for ($i = 1; $i <= $n; $i++) {
        if ($i % 15 === 0)     $lineas[] = 'FizzBuzz';
        elseif ($i % 3 === 0)  $lineas[] = 'Fizz';
        elseif ($i % 5 === 0)  $lineas[] = 'Buzz';
        else                   $lineas[] = (string) $i;
    }
    return implode(', ', $lineas);
}

/* ---------- 2. Arrays ---------- */

function maximo(array $arr): int {
    $max = $arr[0];
    foreach ($arr as $n) {
        if ($n > $max) $max = $n;
    }
    return $max;
}

function eliminarDuplicados(array $arr): array {
    return array_values(array_unique($arr));
}

function bubbleSort(array $arr): array {
    $copia = $arr; // en PHP los arrays se copian por valor
    $len = count($copia);
    for ($i = 0; $i < $len; $i++) {
        for ($j = 0; $j < $len - $i - 1; $j++) {
            if ($copia[$j] > $copia[$j + 1]) {
                [$copia[$j], $copia[$j + 1]] = [$copia[$j + 1], $copia[$j]];
            }
        }
    }
    return $copia;
}

function twoSum(array $arr, int $objetivo): ?array {
    $vistos = [];
    foreach ($arr as $i => $valor) {
        $complemento = $objetivo - $valor;
        if (isset($vistos[$complemento])) {
            return [$vistos[$complemento], $i];
        }
        $vistos[$valor] = $i;
    }
    return null;
}

function flatten(array $arr): array {
    $resultado = [];
    foreach ($arr as $val) {
        if (is_array($val)) {
            $resultado = array_merge($resultado, flatten($val));
        } else {
            $resultado[] = $val;
        }
    }
    return $resultado;
}

function groupBy(array $arr, string $prop): array {
    $acc = [];
    foreach ($arr as $item) {
        $key = $item[$prop];
        $acc[$key][] = $item;
    }
    return $acc;
}

/* ---------- 3. Lógica / matemáticas ---------- */

function fibonacciIterativo(int $n): int {
    [$a, $b] = [0, 1];
    for ($i = 0; $i < $n; $i++) {
        [$a, $b] = [$b, $a + $b];
    }
    return $a;
}

function fibonacciRecursivo(int $n): int {
    if ($n <= 1) return $n;
    return fibonacciRecursivo($n - 1) + fibonacciRecursivo($n - 2);
}

function esPrimo(int $n): bool {
    if ($n < 2) return false;
    for ($i = 2; $i <= sqrt($n); $i++) {
        if ($n % $i === 0) return false;
    }
    return true;
}

function primerosPrimos(int $n): array {
    $primos = [];
    $num = 2;
    while (count($primos) < $n) {
        if (esPrimo($num)) $primos[] = $num;
        $num++;
    }
    return $primos;
}

function factorial(int $n): int {
    return $n <= 1 ? 1 : $n * factorial($n - 1);
}

/* ---------- 4. Objetos / estructuras ---------- */

function esEmailValido(string $email): bool {
    return preg_match('/^[^\s@]+@[^\s@]+\.[^\s@]+$/', $email) === 1;
}

function createPassword(int $longitud, bool $isMayus, bool $withNum, bool $withSimbolos): string {
    $minusculas = 'abcdefghijklmnñopqrstuvwxyz';
    $mayusculas = mb_strtoupper($minusculas);
    $numeros    = '0123456789';
    $simbolos   = '!@$%)*(#+';

    $pool = $minusculas;
    if ($isMayus)      $pool .= $mayusculas;
    if ($withNum)      $pool .= $numeros;
    if ($withSimbolos) $pool .= $simbolos;

    $chars = mb_str_split($pool);
    $password = '';
    for ($i = 0; $i < $longitud; $i++) {
        $password .= $chars[random_int(0, count($chars) - 1)];
    }
    return $password;
}

/*
 * Nota sobre debounce/throttle:
 * PHP clásico se ejecuta por petición (sin event loop), por lo que debounce
 * no aplica igual que en JS (necesitarías ReactPHP/Swoole). Un throttle sí es
 * común en backend (rate limiting): limitar cuántas veces se ejecuta algo
 * por intervalo de tiempo. Aquí una versión simple en memoria:
 */
function throttle(callable $fn, float $limitSegundos): callable {
    $ultimaEjecucion = 0.0;
    return function (...$args) use ($fn, $limitSegundos, &$ultimaEjecucion) {
        $ahora = microtime(true);
        if ($ahora - $ultimaEjecucion >= $limitSegundos) {
            $ultimaEjecucion = $ahora;
            return $fn(...$args);
        }
        return null; // llamada ignorada
    };
}

function miMap(array $arr, callable $callback): array {
    $resultado = [];
    foreach ($arr as $i => $item) {
        $resultado[] = $callback($item, $i, $arr);
    }
    return $resultado;
}

function miFilter(array $arr, callable $callback): array {
    $resultado = [];
    foreach ($arr as $i => $item) {
        if ($callback($item, $i, $arr)) $resultado[] = $item;
    }
    return $resultado;
}

function miReduce(array $arr, callable $callback, $inicial) {
    $acumulador = $inicial;
    foreach ($arr as $i => $item) {
        $acumulador = $callback($acumulador, $item, $i, $arr);
    }
    return $acumulador;
}

/*
 * deepClone: en PHP los ARRAYS ya se copian por valor ($b = $a crea copia).
 * El problema real son los OBJETOS (se copian por referencia al handle).
 * Esta función clona recursivamente objetos y arrays que contengan objetos.
 */
function deepClone(mixed $valor): mixed {
    if (is_object($valor)) {
        $clon = clone $valor;
        foreach (get_object_vars($clon) as $prop => $v) {
            $clon->$prop = deepClone($v);
        }
        return $clon;
    }
    if (is_array($valor)) {
        return array_map('deepClone', $valor);
    }
    return $valor;
}

/* ---------- 5. "Async" en PHP ---------- */

/*
 * PHP no tiene Promises nativas (para eso existen ReactPHP, Amp o Swoole/fibers).
 * El equivalente síncrono de delay() es usleep(). Con Fibers (PHP 8.1+) se puede
 * simular concurrencia cooperativa. Aquí mostramos delay síncrono y un
 * "promiseAll" secuencial que ejecuta una lista de callables y junta resultados.
 */
function delayMs(int $ms): void {
    usleep($ms * 1000);
}

function miPromiseAll(array $tareas): array {
    // Cada tarea es un callable que devuelve un valor.
    $resultados = [];
    foreach ($tareas as $index => $tarea) {
        $resultados[$index] = $tarea();
    }
    return $resultados;
}

/* ---------- 6. Estructuras de datos ---------- */

class Pila {
    private array $items = [];
    public function push(mixed $item): void { $this->items[] = $item; }
    public function pop(): mixed { return array_pop($this->items); }
    public function peek(): mixed { return end($this->items) ?: null; }
    public function estaVacia(): bool { return count($this->items) === 0; }
}

class Cola {
    private array $items = [];
    public function enqueue(mixed $item): void { $this->items[] = $item; }
    public function dequeue(): mixed { return array_shift($this->items); }
    public function frente(): mixed { return $this->items[0] ?? null; }
    public function estaVacia(): bool { return count($this->items) === 0; }
}

function busquedaBinaria(array $arr, int $objetivo): int {
    $inicio = 0;
    $fin = count($arr) - 1;
    while ($inicio <= $fin) {
        $medio = intdiv($inicio + $fin, 2);
        if ($arr[$medio] === $objetivo) return $medio;
        if ($arr[$medio] < $objetivo) $inicio = $medio + 1;
        else $fin = $medio - 1;
    }
    return -1;
}

class Nodo {
    public ?Nodo $siguiente = null;
    public function __construct(public mixed $valor) {}
}

class ListaEnlazada {
    private ?Nodo $cabeza = null;

    public function agregar(mixed $valor): void {
        $nodo = new Nodo($valor);
        if ($this->cabeza === null) {
            $this->cabeza = $nodo;
            return;
        }
        $actual = $this->cabeza;
        while ($actual->siguiente !== null) {
            $actual = $actual->siguiente;
        }
        $actual->siguiente = $nodo;
    }

    public function recorrer(): array {
        $valores = [];
        $actual = $this->cabeza;
        while ($actual !== null) {
            $valores[] = $actual->valor;
            $actual = $actual->siguiente;
        }
        return $valores;
    }
}

/* ---------- Demos ---------- */

function j(mixed $v): string {
    return json_encode($v, JSON_UNESCAPED_UNICODE);
}

echo "=== 1. Strings ===\n";
echo 'esPalindromo("Anita lava la tina") → ' . j(esPalindromo("Anita lava la tina")) . "\n";
echo 'esPalindromo("hola mundo") → ' . j(esPalindromo("hola mundo")) . "\n";
echo 'contarCaracteres("banana") → ' . j(contarCaracteres("banana")) . "\n";
echo 'invertirString("hola") → ' . invertirString("hola") . "\n";
echo 'sonAnagramas("roma", "amor") → ' . j(sonAnagramas("roma", "amor")) . "\n";
echo 'sonAnagramas("hola", "chao") → ' . j(sonAnagramas("hola", "chao")) . "\n";
echo 'fizzBuzz(20) → ' . fizzBuzz(20) . "\n";

echo "\n=== 2. Arrays ===\n";
echo 'maximo([3,7,2,9,4]) → ' . maximo([3, 7, 2, 9, 4]) . "\n";
echo 'eliminarDuplicados([1,2,2,3,3,3]) → ' . j(eliminarDuplicados([1, 2, 2, 3, 3, 3])) . "\n";
echo 'bubbleSort([5,3,8,1,2]) → ' . j(bubbleSort([5, 3, 8, 1, 2])) . "\n";
echo 'twoSum([2,7,11,15], 9) → ' . j(twoSum([2, 7, 11, 15], 9)) . "\n";
echo 'flatten([1,[2,3,[4,5]],6]) → ' . j(flatten([1, [2, 3, [4, 5]], 6])) . "\n";

$personas = [
    ['nombre' => 'Ana',  'edad' => 25],
    ['nombre' => 'Luis', 'edad' => 25],
    ['nombre' => 'Eva',  'edad' => 30],
];
echo 'groupBy(personas, "edad") → ' . j(groupBy($personas, 'edad')) . "\n";

echo "\n=== 3. Lógica / matemáticas ===\n";
echo 'fibonacciIterativo(10) → ' . fibonacciIterativo(10) . "\n";
echo 'fibonacciRecursivo(10) → ' . fibonacciRecursivo(10) . "\n";
echo 'esPrimo(17) → ' . j(esPrimo(17)) . "\n";
echo 'primerosPrimos(5) → ' . j(primerosPrimos(5)) . "\n";
echo 'factorial(5) → ' . factorial(5) . "\n";

echo "\n=== 4. Objetos / estructuras ===\n";
echo 'esEmailValido("correo@dominio.com") → ' . j(esEmailValido("correo@dominio.com")) . "\n";
echo 'esEmailValido("correo-invalido") → ' . j(esEmailValido("correo-invalido")) . "\n";
echo 'createPassword(6, true, true, true) → ' . createPassword(6, true, true, true) . "\n";
echo 'createPassword(12, false, true, false) → ' . createPassword(12, false, true, false) . "\n";
echo 'createPassword(10, true, false, true) → ' . createPassword(10, true, false, true) . "\n";

echo 'miMap([1,2,3], fn($x) => $x*2) → ' . j(miMap([1, 2, 3], fn($x) => $x * 2)) . "\n";
echo 'miFilter([1,2,3,4], fn($x) => $x%2===0) → ' . j(miFilter([1, 2, 3, 4], fn($x) => $x % 2 === 0)) . "\n";
echo 'miReduce([1,2,3,4], fn($a,$x) => $a+$x, 0) → ' . miReduce([1, 2, 3, 4], fn($a, $x) => $a + $x, 0) . "\n";

// Demo deepClone con objetos
$original = new stdClass();
$original->a = 1;
$original->b = new stdClass();
$original->b->c = 2;
$original->b->d = [3, 4];
$copia = deepClone($original);
$copia->b->c = 999;
echo "deepClone: original->b->c → {$original->b->c} | copia->b->c → {$copia->b->c}\n";

// Demo throttle
$procesar = throttle(fn() => print("  [throttle] evento procesado\n"), 0.2);
for ($i = 0; $i < 5; $i++) $procesar(); // solo la primera pasa

echo "\n=== 5. \"Async\" (síncrono con usleep) ===\n";
echo "inicio...\n";
delayMs(1000);
echo "después de 1 segundo ✅\n";
$resultado = miPromiseAll([
    fn() => 'A',
    fn() => 'B',
    fn() => 'C',
]);
echo 'miPromiseAll([...]) → ' . j($resultado) . "\n";

echo "\n=== 6. Estructuras de datos ===\n";
$pila = new Pila();
$pila->push(1); $pila->push(2); $pila->push(3);
echo 'Pila: push 1,2,3 → pop() → ' . $pila->pop() . ' | peek() → ' . $pila->peek() . "\n";

$cola = new Cola();
$cola->enqueue('a'); $cola->enqueue('b'); $cola->enqueue('c');
echo 'Cola: enqueue a,b,c → dequeue() → ' . $cola->dequeue() . ' | frente() → ' . $cola->frente() . "\n";

echo 'busquedaBinaria([1,3,5,7,9,11], 7) → ' . busquedaBinaria([1, 3, 5, 7, 9, 11], 7) . "\n";

$lista = new ListaEnlazada();
$lista->agregar(10); $lista->agregar(20); $lista->agregar(30);
echo 'ListaEnlazada->recorrer() → ' . j($lista->recorrer()) . "\n";
