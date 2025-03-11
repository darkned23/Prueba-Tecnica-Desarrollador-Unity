# Unity Game - RAWG API Integration

## Descripción

Este proyecto es un entorno 3D en Unity que integra la API de RAWG para obtener información de videojuegos y representar estos datos en forma de cartas recolectables dentro del juego.

## Características Implementadas

### 1. Entorno 3D

Se ha desarrollado un entorno en 3D utilizando un asset gratuito de la Unity Asset Store: [Lowpoly Environment Nature - Free Medieval Fantasy Series](https://assetstore.unity.com/packages/3d/environments/lowpoly-environment-nature-free-medieval-fantasy-series-187052).

### 2. Consumo de la API RAWG

- Se realiza la consulta de videojuegos utilizando la API de RAWG.
- Se obtienen páginas de datos con hasta 40 juegos por página.
- Se almacenan 10 páginas en un archivo JSON para evitar múltiples solicitudes a la API.
- Se consulta la información detallada de cada videojuego (descripción, plataformas, géneros, etc.) y se almacena de manera local.

Para más información sobre la API utilizada, puedes revisar la documentación oficial de RAWG: [RAWG API Docs](https://api.rawg.io/docs/#operation/games_read).

### 3. Representación de Ítems Recolectables

Los datos obtenidos de la API se asocian a un objeto `Card` dentro del juego. Cada `Card` contiene la siguiente información:

- **Título del videojuego**
- **Descripción**
- **Fecha de lanzamiento**
- **Plataformas**
- **Géneros**
- **Imagen de fondo** (BackgroundImage)
- **Puntuación en Metacritic**

Las `Card` tienen categorías visuales basadas en su puntuación de Metacritic, diferenciadas por colores según la calidad del videojuego.

### 4. Inventario y UI de Inspección

- Se implementó una UI de inventario donde se almacenan las `Card` recolectadas.
- No existe un límite de cartas que el jugador pueda recolectar.
- Cada `UICard` en el inventario tiene un botón que permite ver los detalles completos de la carta en una vista expandida.

### 5. Persistencia de Datos y Slots de Partidas

- Se implementó un sistema de guardado con **tres slots de partidas**.
- Se puede seleccionar un slot, asignar un nombre a la partida y eliminar o crear nuevos slots.
- Los datos de las partidas se serializan en **binario** para mejorar la seguridad del guardado.

### 6. Sistema de Tutorial

- Se diseñó un sistema de tutorial paso a paso para guiar al jugador en los controles y mecánicas del juego.
- Se explica cómo moverse, recoger y soltar `Card`, y administrar el inventario.

### 7. Navegación en UI

Se ha implementado un sistema de navegación en la interfaz que permite:

- Moverse entre distintas escenas.
- Visualizar el tutorial.
- Crear o iniciar una partida.
- Salir del juego.

### 8. Control del Jugador

El jugador puede:

- **Moverse** con el `Input System` de Unity.
- **Saltar**.
- **Recolectar o soltar** `Card`.
- **Guardar las `Card` en el inventario**.

### 9. Sonido y Feedback Auditivo

- Se añadió música de fondo.
- Se implementaron efectos de sonido para mejorar la retroalimentación auditiva al tomar y guardar cartas.

## Instalación y Ejecución

1. Clonar este repositorio:
   ```sh
   git clone https://github.com/darkned23/Prueba-Tecnica-Desarrollador-Unity
   ```
2. Abrir el proyecto en Unity (versión recomendada: Unity 6 (6000.0.36f1)).
3. Ejecutar la escena principal (`MainScene`).
4. ¡Disfruta del juego!

## Despliegue (Opcional)

Este proyecto ha sido compilado y desplegado en la plataforma **Windows**. Puedes descargarlo desde el siguiente enlace: https://darkned-23.itch.io/prueba-tecnica-desarrollador-unity

## Créditos

Desarrollado por **Jhon Edwar Gonzalez Arenas**

Este proyecto fue creado como parte de una prueba técnica para el proceso de selección de **Desarrollador Unity**.
