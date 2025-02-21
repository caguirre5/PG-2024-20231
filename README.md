# PG-2024-20231
# Videojuego con Integración de IA y Generación Procedural

Este proyecto se basa en el desarrollo de un videojuego 3D que integre **Modelos de Lenguaje Grande (LLMs)** e implementa técnicas de **Generación Procedural de Contenido (GPC)** para ofrecer una experiencia de juego inmersiva, única en cada sesión de juego y visualmente atractiva. 

El videojuego combina elementos de exploración, combate y recolección en un mundo medieval con escenarios generados dinámicamente. La integración de la inteligencia artificial permite interacciones únicas con NPCs (personajes no jugables), mientras que las técnicas de GPC aseguran que cada partida sea distinta y emocionante.

## Descripción del Proyecto

### Características principales
- **Exploración dinámica**: Un mapa 3D dividido en tres secciones diferenciadas: villa, laberinto y bosque, cada una con desafíos y elementos únicos generados proceduralmente.
- **Interacción con NPCs mediante IA**: Los personajes no jugables reaccionan y responden dinámicamente a las interacciones del jugador gracias a los modelos de lenguaje.
- **Generación procedural**:
  - Escenarios naturales (bosques, terrenos).
  - Mazmorras y laberintos únicos en cada partida.
  - Distribución aleatoria de objetos interactuables y ornamentales.
- **Mecánicas de juego**:
  - Movimiento del personaje en un entorno isométrico.
  - Recolección de objetos para alcanzar condiciones de victoria.
  - Interacciones con NPCs para obtener pistas y avanzar en la narrativa.

### Tecnologías utilizadas
- **Unity**: Motor de desarrollo para videojuegos 3D.
- **C#**: Lenguaje de programación utilizado para scripts personalizados.
- **Tilemap**: Herramienta de Unity para crear mapas modulares y optimizados.
- **API OpenAI**: API desarrollada por OpenAI para acceder a modelos de IA a través de consultas.

### Objetivo principal
Crear un videojuego experimental que demuestre cómo la integración de LLMs y GPC puede transformar el diseño de videojuegos, ofreciendo narrativas personalizadas, mundos generativos y experiencias de juego inmersivas.

---

## Instrucciones de Instalación

### Requisitos previos
- **Unity**: Instalar [Unity Hub](https://unity.com/download) y la versión recomendada del motor de Unity utilizada para este proyecto.
- **Sistema operativo**: Windows, macOS o Linux con soporte para Unity.
- **Controladores gráficos actualizados** para optimizar el rendimiento.

### Pasos para configurar el proyecto
1. **Clona este repositorio** en tu computadora:
    Enlace
2. **Abre Unity Hub** y agrega el proyecto:
- Haz clic en **Add project** y selecciona la carpeta donde se clonó el repositorio.
3. **Verifica la versión de Unity**:
- Asegúrate de que la versión de Unity instalada coincide con la utilizada en este proyecto.
4. **Configura las dependencias del proyecto**:
- Los paquetes necesarios (como Input System o ProBuilder) ya están configurados en el archivo `manifest.json`. Unity instalará automáticamente los paquetes faltantes.
5. **Ejecuta la escena principal**:
- Abre la escena principal desde la carpeta `Assets/Scenes/MainScene.unity`.
- Haz clic en **Play** para ejecutar el juego desde el editor.

---

## Consideraciones sobre la integración de la API de OpenAI

Para este proyecto, se utiliza la API de OpenAI para alimentar las interacciones con los NPCs mediante modelos de lenguaje grande. Sin embargo, por motivos de seguridad y costos asociados al uso de esta API, no se incluye la configuración de la clave API en el repositorio.

### Configuración de la API
1. **Obtener la clave API**:
- Regístrate en [OpenAI](https://platform.openai.com/) y genera tu propia clave.
2. **Configura la clave API en el entorno local**:
- Crea un archivo `.openai` en la carpeta de usuario y agrega el archivo JSON de autenticación que se generó al momento de crear la clave.

  El contenido del archivo JSON debe de verse algo asi:
  ```
  OPENAI_API_KEY=tu-clave-api-aquí
  {
	"api_key":"tu-clave-api-aquí",
	"organization":"tu-id-de-organizacion-aquí"
  }
  ```
- La biblioteca de OpenAI detecta automáticamente esta clave al realizar solicitudes a la API.

### Nota importante
La integración de la API tiene un costo basado en el uso (tokens procesados). Se recomienda monitorear su consumo para evitar costos inesperados. 

---

## Créditos y Recursos

- **Modelos 3D y assets**: Se utilizan assets de [Kenney](https://kenney.nl/) con licencia abierta, garantizando coherencia visual en el estilo low poly.
- **Motor de juego**: Unity, con herramientas avanzadas como Tilemap y ProBuilder.
- **Documentación de OpenAI**: [OpenAI API Documentation](https://platform.openai.com/docs/).

---

**Advertencia:** Este proyecto es experimental y no incluye fases de despliegue ni actualizaciones continuas. Su finalidad es demostrar el potencial de las tecnologías emergentes en el diseño de videojuegos.

## Demo del proyecto

- carpeta: demo/
- enlace: [Kantera Trailer](https://youtu.be/GZCEuE_XdGk)

## Informe final

- carpeta: docs/