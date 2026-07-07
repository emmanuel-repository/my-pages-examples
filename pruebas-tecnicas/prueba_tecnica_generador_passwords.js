function createPassword(longitud, isMayus, withNum, withSimbolos) {
  const minusculas = 'abcdefghijklmnñopqrstuvwxyz';
  const mayusculas = minusculas.toUpperCase();
  const numeros = '0123456789';
  const simbolos = '!@$%)*(#+';

  let pool = minusculas;
  let obligatorios = []; // al menos un carácter de cada tipo activo

  obligatorios.push(minusculas[Math.floor(Math.random() * minusculas.length)]);

  if (isMayus) {
    pool += mayusculas;
    obligatorios.push(mayusculas[Math.floor(Math.random() * mayusculas.length)]);
  }
  if (withNum) {
    pool += numeros;
    obligatorios.push(numeros[Math.floor(Math.random() * numeros.length)]);
  }
  if (withSimbolos) {
    pool += simbolos;
    obligatorios.push(simbolos[Math.floor(Math.random() * simbolos.length)]);
  }

  // rellenamos el resto al azar desde el pool completo
  let resto = [];
  for (let i = 0; i < longitud - obligatorios.length; i++) {
    resto.push(pool[Math.floor(Math.random() * pool.length)]);
  }

  // mezclamos obligatorios + resto para que no queden siempre al inicio
  return [...obligatorios, ...resto]
    .sort(() => Math.random() - 0.5)
    .join('');
}
