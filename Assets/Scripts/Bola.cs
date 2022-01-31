using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bola : MonoBehaviour {

  //Velocidad
  AudioSource fuenteDeAudio;
  [SerializeField] private float velocidad = 50.0f;
  [SerializeField] private int golesIzquierda = 0;
  [SerializeField] private int golesDerecha = 0;
  [SerializeField] private AudioClip audioGol, audioRaqueta, audioRebote;
  //Cajas de texto de los contadores
  [SerializeField] private Text contadorIzquierda;
  [SerializeField] private Text contadorDerecha;
  [SerializeField] private Text resultado;
  [SerializeField] private Text temporizador;
  private float tiempo = 180;

  //Se ejecuta al arrancar
  void Start () {
    contadorIzquierda.text = golesIzquierda.ToString();
    contadorDerecha.text = golesDerecha.ToString();
    //Velocidad inicial hacia la derecha
    GetComponent<Rigidbody2D>().velocity = Vector2.right * velocidad;
    fuenteDeAudio = GetComponent<AudioSource>();
    resultado.enabled = false;
    Time.timeScale = 1;
    
  }
  /* Añadir como dos nuevos métodos ANTEs de la última llave de cierre } de la clase */
  void Update () {

  //Incremento la velocidad de la bola
  velocidad = velocidad + 2 * Time.deltaTime;
  if (tiempo >= 0){
  tiempo -= Time.deltaTime; //Le resto el tiempo transcurrido en cada frame
  temporizador.text = formatearTiempo(tiempo); //Formateo el tiempo y lo escribo en la caja de texto
  }
  //Si se ha acabado el tiempo, compruebo quién ha ganado y se acaba el juego
  else{
    temporizador.text = "00:00"; //Para evitar valores negativos	
    //Compruebo quién ha ganado
    if (golesIzquierda > golesDerecha){
      //Escribo y muestro el resultado
      resultado.text = "¡Jugador Izquierda GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
    }
    else if (golesDerecha > golesIzquierda){
      //Escribo y muestro el resultado
      resultado.text = "¡Jugador Derecha GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
    }
    else{
      //Escribo y muestro el resultado
      resultado.text = "¡EMPATE!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
    }
    //Muestro el resultado, pauso el juego y devuelvo true
    resultado.enabled = true;
    Time.timeScale = 0; //Pausa
  }
}
//Se ejecuta al colisionar
void OnCollisionEnter2D(Collision2D micolision){

  //transform.position es la posición de la bola
  //micolision contiene toda la información de la colisión
  //Si la bola colisiona con la raqueta:
  //micolision.gameObject es la raqueta
  //micolision.transform.position es la posición de la raqueta

  //Si choca con la raqueta izquierda
  if (micolision.gameObject.name == "Raqueta Izquierda"){
    fuenteDeAudio.clip = audioRaqueta;
    fuenteDeAudio.Play();
    //Valor de x
    int x = 1;

    //Valor de y
    int y = direccionY(transform.position, micolision.transform.position);

    //Vector de dirección
    Vector2 direccion = new Vector2(x, y);

    //Aplico velocidad
    GetComponent<Rigidbody2D>().velocity = direccion * velocidad;

  }

  //Si choca con la raqueta derecha
  if (micolision.gameObject.name == "Raqueta Derecha"){
    fuenteDeAudio.clip = audioRaqueta;
    fuenteDeAudio.Play();
    //Valor de x
    int x = -1;

    //Valor de y
    int y = direccionY(transform.position, micolision.transform.position);

    //Vector de dirección
    Vector2 direccion = new Vector2(x, y);

    //Aplico velocidad
    GetComponent<Rigidbody2D>().velocity = direccion * velocidad;

  }
  if (micolision.gameObject.name == "Arriba" || micolision.gameObject.name == "Abajo"){

  //Reproduzco el sonido del rebote
  fuenteDeAudio.clip = audioRebote;
  fuenteDeAudio.Play();

  }
}

//Método para calcular la direccion de Y (deevuelve un número entero int)
int direccionY(Vector2 posicionBola, Vector2 posicionRaqueta){

  if (posicionBola.y > posicionRaqueta.y){
    return 1; //Si choca por la parte superior de la raqueta, sale hacia arriba
  }
  else if (posicionBola.y < posicionRaqueta.y){
    return -1; //Si choca por la parte inferior de la raqueta, sale hacia abajo
  }
  else{
    return 0; //Si choca por la parte central de la raqueta, sale en horizontal
  }
}
bool comprobarFinal(){

  //Si el de la izquierda ha llegado a 5
  if (golesIzquierda == 5){
    //Escribo y muestro el resultado
    resultado.text = "¡Jugador Izquierda GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
    //Muestro el resultado, pauso el juego y devuelvo true
    resultado.enabled = true;
    Time.timeScale = 0; //Pausa
    return true;
  }
  //Si el de le aderecha a llegado a 5
  else if (golesDerecha == 5){
    //Escribo y muestro el resultado
    resultado.text = "¡Jugador Derecha GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
    //Muestro el resultado, pauso el juego y devuelvo true
    resultado.enabled = true;
    Time.timeScale = 0; //Pausa
    return true;
  }
  //Si ninguno ha llegado a 5, continúa el juego
  else{
    return false;
  }
}

public void reiniciarBola(string direccion){

  //Posición 0 de la bola
  transform.position = Vector2.zero;
  //Vector2.zero es lo mismo que new Vector2(0,0);

  //Velocidad inicial de la bola
  velocidad = 30;

  //Velocidad y dirección
  if (direccion == "Derecha"){
    //Incremento goles al de la derecha
    golesDerecha++;
    //Lo escribo en el marcador
    contadorDerecha.text = golesDerecha.ToString();
    //Reinicio la bola
    if (!comprobarFinal()){
    GetComponent<Rigidbody2D>().velocity = Vector2.right * velocidad;
  //Vector2.right es lo mismo que new Vector2(1,0)
  }
}
  else if (direccion == "Izquierda"){
    //Incremento goles al de la izquierda
    golesIzquierda++;
    //Lo escribo en el marcador
    contadorIzquierda.text = golesIzquierda.ToString();
    //Reinicio la bola
  if (!comprobarFinal()){
    GetComponent<Rigidbody2D>().velocity = Vector2.left * velocidad;
  //Vector2.left es lo mismo que new Vector2(-1,0)
  }	
}
  fuenteDeAudio.clip = audioGol;
  fuenteDeAudio.Play();
}
string formatearTiempo(float tiempo){

  //Formateo minutos y segundos a dos dígitos
	string minutos = Mathf.Floor(tiempo / 60).ToString("00");
 	string segundos = Mathf.Floor(tiempo % 60).ToString("00");
    
	//Devuelvo el string formateado con : como separador
	return minutos + ":" + segundos;
  
}
}
